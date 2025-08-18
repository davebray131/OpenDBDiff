using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class XMLSchema(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.XMLSchema)
{

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public new XMLSchema Clone(ISchemaBase parent) => new(parent)
    {
        Text = Text,
        Status = Status,
        Name = Name,
        Id = Id,
        Owner = Owner,
        Guid = Guid,
        Dependencies = Dependencies
    };

    public List<ObjectDependency> Dependencies { get; set; } = [];

    public string Text { get; set; }

    public override string ToSql()
    {
        var sql = new StringBuilder();
        sql.Append("CREATE XML SCHEMA COLLECTION ");
        sql.Append($"{FullName} AS ");
        sql.Append($"N'{Text}'");
        sql.Append("\r\nGO\r\n");
        return sql.ToString();
    }

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => $"DROP XML SCHEMA COLLECTION {FullName}\r\nGO\r\n";

    private SQLScriptList ToSQLChangeColumns()
    {
        Hashtable fields = [];
        var list = new SQLScriptList();
        if ((Status == ObjectStatus.Alter) || (Status == ObjectStatus.Rebuild))
        {
            foreach (var dependency in Dependencies)
            {
                var itemDepens = ((Database)Parent).Find(dependency.Name);
                if (dependency.IsCodeType)
                {
                    list.AddRange(((ICode)itemDepens).Rebuild());
                }
                if (dependency.Type == ObjectType.Table)
                {
                    var column = ((Table)itemDepens).Columns[dependency.ColumnName];
                    if ((column.Parent.Status != ObjectStatus.Drop) && (column.Parent.Status != ObjectStatus.Create) && column.Status != ObjectStatus.Create)
                    {
                        if (!fields.ContainsKey(column.FullName))
                        {
                            if (column.HasToRebuildOnlyConstraint)
                            {
                                column.Parent.Status = ObjectStatus.RebuildDependencies;
                            }

                            list.AddRange(column.RebuildConstraint(true));
                            list.Add($"ALTER TABLE {column.Parent.FullName} ALTER COLUMN {column.ToSQLRedefine(null, 0, "")}\r\nGO\r\n", 0, ScriptAction.AlterColumn);
                            /*Si la columna va a ser eliminada o la tabla va a ser reconstruida, no restaura la columna*/
                            if ((column.Status != ObjectStatus.Drop) && (column.Parent.Status != ObjectStatus.Rebuild))
                            {
                                list.AddRange(column.Alter(ScriptAction.AlterColumnRestore));
                            }

                            fields.Add(column.FullName, column.FullName);
                        }
                    }
                }
            }
        }
        return list;
    }

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            list.Add(ToSqlDrop(), 0, ScriptAction.DropXMLSchema);
        }
        if (Status == ObjectStatus.Create)
        {
            list.Add(ToSql(), 0, ScriptAction.AddXMLSchema);
        }
        if (Status == ObjectStatus.Alter)
        {
            list.AddRange(ToSQLChangeColumns());
            list.Add(ToSqlDrop() + ToSql(), 0, ScriptAction.AddXMLSchema);
        }
        return list;
    }

    public bool Compare(XMLSchema obj) => obj == null ? throw new ArgumentNullException(nameof(obj)) : Text.Equals(obj.Text);
}
