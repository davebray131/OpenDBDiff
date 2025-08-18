using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class UserDataType : SQLServerSchemaBase
{
    public UserDataType(ISchemaBase parent)
        : base(parent, ObjectType.UserDataType)
    {
        Default = new Default(this);
        Rule = new Rule(this);
        Dependencies = [];
    }

    public List<ObjectDependency> Dependencies { get; private set; }

    public Rule Rule { get; private set; }

    public Default Default { get; private set; }

    public string AssemblyName { get; set; }

    public bool IsAssembly { get; set; }

    public string AssemblyClass { get; set; }

    public int AssemblyId { get; set; }

    /// <summary>
    /// Cantidad de digitos que permite el campo (solo para campos Numeric).
    /// </summary>
    public int Scale { get; set; }

    /// <summary>
    /// Cantidad de decimales que permite el campo (solo para campos Numeric).
    /// </summary>
    public int Precision { get; set; }

    public bool AllowNull { get; set; }

    public int Size { get; set; }

    public string Type { get; set; }

    public string AssemblyFullName => IsAssembly ? AssemblyName + "." + AssemblyClass : string.Empty;

    /// <summary>
    /// Clona el objeto Column en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var item = new UserDataType(parent)
        {
            Name = Name,
            Id = Id,
            Owner = Owner,
            Guid = Guid,
            AllowNull = AllowNull,
            Precision = Precision,
            Scale = Scale,
            Size = Size,
            Type = Type,
            Default = Default.Clone(this),
            Rule = Rule.Clone(this),
            Dependencies = Dependencies,
            IsAssembly = IsAssembly,
            AssemblyClass = AssemblyClass,
            AssemblyId = AssemblyId,
            AssemblyName = AssemblyName
        };
        return item;
    }

    public static bool CompareRule(UserDataType origin, UserDataType destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if ((origin.Rule.Name != null) && (destination.Rule.Name == null))
        {
            return false;
        }

        if ((origin.Rule.Name == null) && (destination.Rule.Name != null))
        {
            return false;
        }

        if (origin.Rule.Name != null)
        {
            if (!origin.Rule.Name.Equals(destination.Rule.Name))
            {
                return false;
            }
        }

        return true;
    }

    public static bool CompareDefault(UserDataType origin, UserDataType destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if ((origin.Default.Name != null) && (destination.Default.Name == null))
        {
            return false;
        }

        if ((origin.Default.Name == null) && (destination.Default.Name != null))
        {
            return false;
        }

        if (origin.Default.Name != null)
        {
            if (!origin.Default.Name.Equals(destination.Default.Name))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToSql()
    {
        var sql = "CREATE TYPE " + FullName;
        if (!IsAssembly)
        {
            sql += $" FROM [{Type}]";
            if (Type.Equals("binary") || Type.Equals("varbinary") || Type.Equals("varchar") || Type.Equals("char") ||
                Type.Equals("nchar") || Type.Equals("nvarchar"))
            {
                sql += "(" + Size.ToString(CultureInfo.InvariantCulture) + ")";
            }

            if (Type.Equals("numeric") || Type.Equals("decimal"))
            {
                sql += " (" + Precision.ToString(CultureInfo.InvariantCulture) + "," +
                       Scale.ToString(CultureInfo.InvariantCulture) + ")";
            }

            sql += AllowNull ? " NULL" : " NOT NULL";
        }
        else
        {
            sql += $" EXTERNAL NAME [{AssemblyName}].[{AssemblyClass}]";
        }
        sql += "\r\nGO\r\n";
        return sql + ToSQLAddBinds();
    }

    public override string ToSqlDrop() => $"DROP TYPE {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    private string ToSQLAddBinds()
    {
        var sql = string.Empty;
        if (!string.IsNullOrEmpty(Default.Name))
        {
            sql += Default.ToSQLAddBind();
        }

        if (!string.IsNullOrEmpty(Rule.Name))
        {
            sql += Rule.ToSQLAddBind();
        }

        return sql;
    }

    private SQLScriptList RebuildDependencies(Table table)
    {
        var list = new SQLScriptList();
        var items = ((Database)table.Parent).Dependencies.Find(table.Id);
        items.ForEach(item =>
                          {
                              var realItem = ((Database)table.Parent).Find(item.FullName);
                              if (realItem.IsCodeType)
                              {
                                  list.AddRange(((ICode)realItem).Rebuild());
                              }
                          });
        return list;
    }

    private SQLScriptList ToSQLChangeColumns()
    {
        var fields = new Hashtable();
        var list = new SQLScriptList();
        var listDependencies = new SQLScriptList();
        if ((Status == ObjectStatus.Alter) || (Status == ObjectStatus.Rebuild))
        {
            foreach (var dependency in Dependencies)
            {
                var itemDepens = ((Database)Parent).Find(dependency.Name);
                /*Si la dependencia es una funcion o una vista, reconstruye el objecto*/
                if (dependency.IsCodeType)
                {
                    if (itemDepens != null)
                    {
                        list.AddRange(((ICode)itemDepens).Rebuild());
                    }
                }
                /*Si la dependencia es una tabla, reconstruye los indices, constraint y columnas asociadas*/
                if (dependency.Type == ObjectType.Table)
                {
                    var column = ((Table)itemDepens).Columns[dependency.ColumnName];
                    if ((column.Parent.Status != ObjectStatus.Drop) &&
                        (column.Parent.Status != ObjectStatus.Create) &&
                        ((column.Status != ObjectStatus.Create) || column.IsComputed))
                    {
                        if (!fields.ContainsKey(column.FullName))
                        {
                            listDependencies.AddRange(RebuildDependencies((Table)itemDepens));
                            if (column.HasToRebuildOnlyConstraint)
                            {
                                //column.Parent.Status = ObjectStatusType.AlterRebuildDependenciesStatus;
                                list.AddRange(column.RebuildDependencies());
                            }

                            if (!column.IsComputed)
                            {
                                list.AddRange(column.RebuildConstraint(true));
                                list.Add(
                                    $"ALTER TABLE {column.Parent.FullName} ALTER COLUMN {column.ToSQLRedefine(Type, Size, null)}\r\nGO\r\n", 0,
                                    ScriptAction.AlterColumn);
                                /*Si la columna va a ser eliminada o la tabla va a ser reconstruida, no restaura la columna*/
                                if ((column.Status != ObjectStatus.Drop) &&
                                    (column.Parent.Status != ObjectStatus.Rebuild))
                                {
                                    list.AddRange(column.Alter(ScriptAction.AlterColumnRestore));
                                }
                            }
                            else
                            {
                                if (column.Status != ObjectStatus.Create)
                                {
                                    if (!column.GetWasInsertInDiffList(ScriptAction.AlterColumnFormula))
                                    {
                                        column.SetWasInsertInDiffList(ScriptAction.AlterColumnFormula);
                                        list.Add(column.ToSqlDrop(), 0, ScriptAction.AlterColumnFormula);
                                        var drops =
                                            ((Database)column.Parent.Parent).Dependencies.Find(column.Parent.Id,
                                                                                                column.Id, 0);
                                        drops.ForEach(item =>
                                                          {
                                                              if (item.Status != ObjectStatus.Create)
                                                              {
                                                                  list.Add(item.Drop());
                                                              }

                                                              if (item.Status != ObjectStatus.Drop)
                                                              {
                                                                  list.Add(item.Create());
                                                              }
                                                          });
                                        /*Si la columna va a ser eliminada o la tabla va a ser reconstruida, no restaura la columna*/
                                        if ((column.Status != ObjectStatus.Drop) &&
                                            (column.Parent.Status != ObjectStatus.Rebuild))
                                        {
                                            list.Add(column.ToSqlAdd(), 0,
                                                     ScriptAction.AlterColumnFormulaRestore);
                                        }
                                    }
                                }
                            }
                            fields.Add(column.FullName, column.FullName);
                        }
                    }
                }
            }
        }
        list.AddRange(listDependencies);
        return list;
    }

    private bool HasAnotherUDTClass()
    {
        if (IsAssembly)
        {
            /*If another UDT exists in the same assembly to bre created. It must be deleted BEFORE creating the new one.*/
            var other =
                ((Database)Parent).UserTypes.Find(
                    item =>
                    (item.Status == ObjectStatus.Drop) &&
                    (item.AssemblyName + "." + item.AssemblyClass).Equals(AssemblyName + "." + AssemblyClass));
            if (other != null)
            {
                return true;
            }
        }
        return false;
    }

    private string SQLDropOlder()
    {
        var other =
            ((Database)Parent).UserTypes.Find(
                item =>
                (item.Status == ObjectStatus.Drop) &&
                (item.AssemblyName + "." + item.AssemblyClass).Equals(AssemblyName + "." + AssemblyClass));
        return other.ToSqlDrop();
    }

    public override SQLScript Create()
    {
        var action = ScriptAction.AddUserDataType;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlAdd(), 0, action);
        }
        else
        {
            return null;
        }
    }

    public override SQLScript Drop()
    {
        const ScriptAction action = ScriptAction.DropUserDataType;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlDrop(), 0, action);
        }
        return null;
    }

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        try
        {
            var list = new SQLScriptList();
            if (Status == ObjectStatus.Drop)
            {
                if (!HasAnotherUDTClass())
                {
                    list.Add(Drop());
                }
            }
            if (HasState(ObjectStatus.Create))
            {
                list.Add(Create());
            }
            if (Status == ObjectStatus.Alter)
            {
                if (Default.Status == ObjectStatus.Create)
                {
                    list.Add(Default.ToSQLAddBind(), 0, ScriptAction.AddUserDataType);
                }

                if (Default.Status == ObjectStatus.Drop)
                {
                    list.Add(Default.ToSQLAddUnBind(), 0, ScriptAction.UnbindRuleType);
                }

                if (Rule.Status == ObjectStatus.Create)
                {
                    list.Add(Rule.ToSQLAddBind(), 0, ScriptAction.AddUserDataType);
                }

                if (Rule.Status == ObjectStatus.Drop)
                {
                    list.Add(Rule.ToSQLAddUnBind(), 0, ScriptAction.UnbindRuleType);
                }
            }
            if (Status == ObjectStatus.Rebuild)
            {
                list.AddRange(ToSQLChangeColumns());
                if (!GetWasInsertInDiffList(ScriptAction.DropUserDataType))
                {
                    list.Add(ToSqlDrop() + ToSql(), 0, ScriptAction.AddUserDataType);
                }
                else
                {
                    list.Add(Create());
                }
            }
            if (HasState(ObjectStatus.DropOlder))
            {
                list.Add(SQLDropOlder(), 0, ScriptAction.AddUserDataType);
            }
            return list;
        }
        catch
        {
            return null;
        }
    }

    public bool Compare(UserDataType obj) => obj == null
            ? throw new ArgumentNullException(nameof(obj))
            : Scale == obj.Scale && Precision == obj.Precision && AllowNull == obj.AllowNull && Size == obj.Size && Type.Equals(obj.Type) && IsAssembly == obj.IsAssembly && AssemblyClass.Equals(obj.AssemblyClass) && AssemblyName.Equals(obj.AssemblyName) && CompareDefault(this, obj) && CompareRule(this, obj);
}
