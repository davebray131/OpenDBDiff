using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Role(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.Role)
{
    public enum RoleTypeEnum
    {
        ApplicationRole = 1,
        DatabaseRole = 2
    }

    public override string FullName => $"[{Name}]";

    public RoleTypeEnum Type { get; set; }

    private string RoleType => (Type == RoleTypeEnum.ApplicationRole) ? "APPLICATION " : string.Empty;

    public string Password { get; set; }

    public override string ToSql()
    {
        var sql = $"CREATE {RoleType}ROLE {FullName} WITH PASSWORD = N'{Password}'";
        if (!string.IsNullOrEmpty(Owner))
        {
            sql += $" ,DEFAULT_SCHEMA=[{Owner}]";
        }

        return sql + "\r\nGO\r\n";
    }

    public override string ToSqlDrop() => $"DROP {RoleType}ROLE {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRole);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddRole);
        }
        if ((Status & ObjectStatus.Alter) == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRole);
            listDiff.Add(ToSql(), 0, ScriptAction.AddRole);
        }
        return listDiff;
    }


    public bool Compare(Role obj) => obj == null
            ? throw new ArgumentNullException(nameof(obj))
            : Type == obj.Type && Password.Equals(obj.Password) && Owner.Equals(obj.Owner);
}
