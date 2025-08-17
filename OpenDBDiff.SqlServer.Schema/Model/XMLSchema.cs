using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class XMLSchema : SQLServerSchemaBase
{
    public XMLSchema(ISchemaBase parent)
        : base(parent, ObjectType.XMLSchema) => this.Dependencies = [];

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public new XMLSchema Clone(ISchemaBase parent)
    {
        var item = new XMLSchema(parent)
        {
            Text = this.Text,
            Status = this.Status,
            Name = this.Name,
            Id = this.Id,
            Owner = this.Owner,
            Guid = this.Guid,
            Dependencies = this.Dependencies
        };
        return item;
    }

    public List<ObjectDependency> Dependencies { get; set; }

    public string Text { get; set; }

    public override string ToSql()
    {
        var sql = new StringBuilder();
        _ = sql.Append("CREATE XML SCHEMA COLLECTION ");
        _ = sql.Append(this.FullName + " AS ");
        _ = sql.Append("N'" + this.Text + "'");
        _ = sql.Append("\r\nGO\r\n");
        return sql.ToString();
    }

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => $"DROP XML SCHEMA COLLECTION {FullName}\r\nGO\r\n";

    private SQLScriptList ToSQLChangeColumns()
    {
        Hashtable fields = [];
        var list = new SQLScriptList();
        if ((this.Status == ObjectStatus.Alter) || (this.Status == ObjectStatus.Rebuild))
        {
            foreach (var dependency in this.Dependencies)
            {
                var itemDepens = ((Database)this.Parent).Find(dependency.Name);
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
                            list.Add("ALTER TABLE " + column.Parent.FullName + " ALTER COLUMN " + column.ToSQLRedefine(null, 0, "") + "\r\nGO\r\n", 0, ScriptAction.AlterColumn);
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

        if (this.Status == ObjectStatus.Drop)
        {
            list.Add(ToSqlDrop(), 0, ScriptAction.DropXMLSchema);
        }
        if (this.Status == ObjectStatus.Create)
        {
            list.Add(ToSql(), 0, ScriptAction.AddXMLSchema);
        }
        if (this.Status == ObjectStatus.Alter)
        {
            list.AddRange(ToSQLChangeColumns());
            list.Add(ToSqlDrop() + ToSql(), 0, ScriptAction.AddXMLSchema);
        }
        return list;
    }

    public bool Compare(XMLSchema obj) => obj == null ? throw new ArgumentNullException("obj") : this.Text.Equals(obj.Text);
}
