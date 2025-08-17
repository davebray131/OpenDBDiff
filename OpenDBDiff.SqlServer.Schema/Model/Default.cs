using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Default(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.Default)
{
    public new Default Clone(ISchemaBase parent) =>
        new(parent)
        {
            Id = Id,
            Name = Name,
            Owner = Owner,
            Value = Value
        };

    public string Value { get; set; }

    public string ToSQLAddBind() => $"EXEC sp_bindefault N'{Name}', N'{Parent.Name}'\r\nGO\r\n";

    public string ToSQLAddUnBind() => $"EXEC sp_unbindefault @objname=N'{Parent.Name}'\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => $"DROP DEFAULT {FullName}\r\nGO\r\n";

    public override string ToSql() => string.Empty;

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRule);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddRule);
        }
        if (Status == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRule);
            listDiff.Add(ToSql(), 0, ScriptAction.AddRule);
        }
        return listDiff;
    }
}
