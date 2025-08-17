using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Attributes;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model.Util;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class View : Code
{
    public View(ISchemaBase parent)
        : base(parent, ObjectType.View, ScriptAction.AddView, ScriptAction.DropView)
    {
        Indexes = new SchemaList<Index, View>(this, ((Database)parent).AllObjects);
        Triggers = new SchemaList<Trigger, View>(this, ((Database)parent).AllObjects);
        CLRTriggers = new SchemaList<CLRTrigger, View>(this, ((Database)parent).AllObjects);
    }

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var item = new View(parent)
        {
            Text = Text,
            Status = Status,
            Name = Name,
            Id = Id,
            Owner = Owner,
            IsSchemaBinding = IsSchemaBinding,
            DependenciesIn = DependenciesIn,
            DependenciesOut = DependenciesOut
        };
        item.Indexes = Indexes.Clone(item);
        item.Triggers = Triggers.Clone(item);
        return item;
    }

    [SchemaNode("CLR Triggers")]
    public SchemaList<CLRTrigger, View> CLRTriggers { get; set; }

    [SchemaNode("Triggers")]
    public SchemaList<Trigger, View> Triggers { get; set; }

    [SchemaNode("Indexes", "Index")]
    public SchemaList<Index, View> Indexes { get; set; }

    public override bool IsCodeType => true;

    public override string ToSqlAdd()
    {
        var sql = ToSql();
        Indexes.ForEach(item =>
            {
                if (item.Status != ObjectStatus.Drop)
                {
                    item.SetWasInsertInDiffList(ScriptAction.AddIndex);
                    sql += item.ToSql();
                }
            }
        );
        Triggers.ForEach(item =>
            {
                if (item.Status != ObjectStatus.Drop)
                {
                    item.SetWasInsertInDiffList(ScriptAction.AddTrigger);
                    sql += item.ToSql();
                }
            }
        );

        sql += ExtendedProperties.ToSql();
        return sql;
    }

    public string ToSQLAlter() => ToSQLAlter(false);

    public string ToSQLAlter(bool quitSchemaBinding) => FormatCode.FormatAlter("VIEW", ToSql(), this, quitSchemaBinding);

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status != ObjectStatus.Original)
        {
            RootParent.ActionMessage.Add(this);
        }

        if (HasState(ObjectStatus.Drop))
        {
            list.Add(Drop());
        }

        if (HasState(ObjectStatus.Create))
        {
            list.Add(Create());
        }

        if (HasState(ObjectStatus.Alter))
        {
            if (HasState(ObjectStatus.RebuildDependencies))
            {
                list.AddRange(RebuildDependencies());
            }

            if (HasState(ObjectStatus.Rebuild))
            {
                list.Add(Drop());
                list.Add(Create());
            }
            if (HasState(ObjectStatus.AlterBody))
            {
                var iCount = DependenciesCount;
                list.Add(ToSQLAlter(), iCount, ScriptAction.AlterView);
            }
            if (!GetWasInsertInDiffList(ScriptAction.DropFunction) && (!GetWasInsertInDiffList(ScriptAction.AddFunction)))
            {
                list.AddRange(Indexes.ToSqlDiff());
            }

            list.AddRange(Triggers.ToSqlDiff());
        }
        return list;
    }
}
