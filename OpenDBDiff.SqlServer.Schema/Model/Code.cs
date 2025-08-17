using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model.Util;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Schema.Model;

public abstract class Code : SQLServerSchemaBase, ICode
{
    protected string sql = null;
    protected string typeName = "";
    private readonly int deepMax = 0;
    private readonly int deepMin = 0;
    private readonly ScriptAction addAction;
    private readonly ScriptAction dropAction;

    public Code(ISchemaBase parent, ObjectType type, ScriptAction addAction, ScriptAction dropAction)
        : base(parent, type)
    {
        DependenciesIn = [];
        DependenciesOut = [];
        typeName = GetObjectTypeName(ObjectType);
        /*Por el momento, solo los Assemblys manejan deep de dependencias*/
        if (this.ObjectType == ObjectType.Assembly)
        {
            deepMax = 501;
            deepMin = 500;
        }
        this.addAction = addAction;
        this.dropAction = dropAction;
    }

    public override SQLScript Create()
    {
        var iCount = DependenciesCount;
        if (iCount > 0)
        {
            iCount *= -1;
        }

        if (!GetWasInsertInDiffList(addAction))
        {
            SetWasInsertInDiffList(addAction);
            return new SQLScript(this.ToSqlAdd(), iCount, addAction);
        }
        else
        {
            return null;
        }
    }

    public override SQLScript Drop()
    {
        var iCount = DependenciesCount;
        if (!GetWasInsertInDiffList(dropAction))
        {
            SetWasInsertInDiffList(dropAction);
            return new SQLScript(this.ToSqlDrop(), iCount, dropAction);
        }
        else
        {
            return null;
        }
    }

    private static string GetObjectTypeName(ObjectType type)
    {
        if (type == ObjectType.Rule)
        {
            return "RULE";
        }

        if (type == ObjectType.Trigger)
        {
            return "TRIGGER";
        }

        if (type == ObjectType.View)
        {
            return "VIEW";
        }

        return type == ObjectType.Function
            ? "FUNCTION"
            : type == ObjectType.StoredProcedure
            ? "PROCEDURE"
            : type == ObjectType.CLRStoredProcedure
            ? "PROCEDURE"
            : type == ObjectType.CLRTrigger
            ? "TRIGGER"
            : type == ObjectType.CLRFunction ? "FUNCTION" : type == ObjectType.Assembly ? "ASSEMBLY" : "";
    }

    /// <summary>
    /// Names collection of dependant objects of the object
    /// </summary>
    public List<string> DependenciesOut { get; set; }

    /// <summary>
    /// Names collection of objects which the object depends on
    /// </summary>
    public List<string> DependenciesIn { get; set; }

    public bool IsSchemaBinding { get; set; }

    public string Text { get; set; }

    public override int DependenciesCount
    {
        get
        {
            var iCount = 0;
            if (this.DependenciesOut.Any())
            {
                Dictionary<string, bool> depencyTracker = [];
                iCount = DependenciesCountFilter(this.FullName, depencyTracker);
            }
            return iCount;
        }
    }

    private int DependenciesCountFilter(string FullName, Dictionary<string, bool> depencyTracker)
    {
        var count = 0;
        ICode item;
        try
        {
            item = (ICode)((Database)Parent).Find(FullName);
            if (item != null)
            {
                for (var j = 0; j < item.DependenciesOut.Count; j++)
                {
                    if (!depencyTracker.ContainsKey(FullName.ToUpper()))
                    {
                        depencyTracker.Add(FullName.ToUpper(), true);
                    }
                    count += 1 + DependenciesCountFilter(item.DependenciesOut[j], depencyTracker);
                }
            }
            return count;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// Indicates if there are dependant tables on the object which must be rebuild
    /// </summary>
    /// <returns></returns>
    public bool HasToRebuild
    {
        get
        {
            for (var j = 0; j < DependenciesIn.Count; j++)
            {
                var item = ((Database)Parent).Find(DependenciesIn[j]);
                if (item != null)
                {
                    if ((item.Status == ObjectStatus.Rebuild) || (item.Status == ObjectStatus.RebuildDependencies))
                    {
                        return true;
                    }
                }
            }
            ;
            return IsSchemaBinding;
        }
    }

    private SQLScriptList RebuildDependencies(List<string> depends, int deepMin, int deepMax)
    {
        var newDeepMax = (deepMax != 0) ? deepMax + 1 : 0;
        var newDeepMin = (deepMin != 0) ? deepMin - 1 : 0;
        var list = new SQLScriptList();
        for (var j = 0; j < depends.Count; j++)
        {
            var item = ((Database)Parent).Find(depends[j]);
            if (item != null)
            {
                if ((item.Status != ObjectStatus.Create) && (item.Status != ObjectStatus.Drop))
                {
                    if ((item.ObjectType != ObjectType.CLRStoredProcedure) && (item.ObjectType != ObjectType.Assembly) && (item.ObjectType != ObjectType.UserDataType) && (item.ObjectType != ObjectType.View) && (item.ObjectType != ObjectType.Function))
                    {
                        newDeepMin = 0;
                        newDeepMax = 0;
                    }
                    if (item.Status != ObjectStatus.Drop)
                    {
                        if (!(item.Parent.HasState(ObjectStatus.Rebuild) && (item.ObjectType == ObjectType.Trigger)))
                        {
                            list.Add(item.Drop(), newDeepMin);
                        }
                    }
                    if ((this.Status != ObjectStatus.Drop) && (item.Status != ObjectStatus.Create))
                    {
                        list.Add(item.Create(), newDeepMax);
                    }

                    if (item.IsCodeType)
                    {
                        list.AddRange(RebuildDependencies(((ICode)item).DependenciesOut, newDeepMin, newDeepMax));
                    }
                }
            }
        }
        ;
        return list;
    }

    /// <summary>
    /// Rebuilds the object and all its dependant objects.
    /// </summary>
    /// <returns></returns>
    public SQLScriptList Rebuild()
    {
        var list = new SQLScriptList();
        list.AddRange(RebuildDependencies());
        if (this.Status != ObjectStatus.Create)
        {
            list.Add(Drop(), deepMin);
        }

        if (this.Status != ObjectStatus.Drop)
        {
            list.Add(Create(), deepMax);
        }

        return list;
    }

    /// <summary>
    /// Rebuilds the dependant objects.
    /// </summary>
    /// <returns></returns>
    public SQLScriptList RebuildDependencies() => RebuildDependencies(this.DependenciesOut, deepMin, deepMax);

    public override string ToSql()
    {
        if (string.IsNullOrEmpty(sql))
        {
            sql = FormatCode.FormatCreate(typeName, Text, this);
        }

        return sql;
    }

    public override string ToSqlAdd()
    {
        var sql = ToSql();
        sql += this.ExtendedProperties.ToSql();
        return sql;
    }

    public override string ToSqlDrop() => string.Format("DROP {0} {1}\r\nGO\r\n", typeName, FullName);

    public virtual bool CompareExceptWhitespace(ICode obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        string sql1;
        string sql2;

        var whitespace = new Regex(@"\s");
        sql1 = whitespace.Replace(this.ToSql(), "");
        sql2 = whitespace.Replace(obj.ToSql(), "");

        return ((Database)RootParent).Options.Comparison.CaseSensityInCode == Options.SqlOptionComparison.CaseSensityOptions.CaseInsensity
            ? sql1.Equals(sql2, StringComparison.InvariantCultureIgnoreCase)
            : sql1.Equals(sql2, StringComparison.InvariantCulture);
    }

    public virtual bool Compare(ICode obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        var sql1 = this.ToSql();
        var sql2 = obj.ToSql();
        if (((Database)RootParent).Options.Comparison.IgnoreWhiteSpacesInCode)
        {
            var whitespace = new Regex(@"\s");
            sql1 = whitespace.Replace(this.ToSql(), "");
            sql2 = whitespace.Replace(obj.ToSql(), "");
        }
        return ((Database)RootParent).Options.Comparison.CaseSensityInCode == SqlOptionComparison.CaseSensityOptions.CaseInsensity
            ? sql1.Equals(sql2, StringComparison.InvariantCultureIgnoreCase)
            : sql1.Equals(sql2, StringComparison.InvariantCulture);
    }
}
