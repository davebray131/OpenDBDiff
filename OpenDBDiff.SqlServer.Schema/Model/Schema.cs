using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Schema : SQLServerSchemaBase
{
    public Schema(Database parent)
        : base(parent, ObjectType.Schema)
    {
    }

    public override string ToSql() => $"CREATE SCHEMA[{Name}] AUTHORIZATION[{Owner}]\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => $"DROP SCHEMA [{Name}]\r\nGO\r\n";

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropSchema);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddSchema);
        }
        return listDiff;
    }
}
