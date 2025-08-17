using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Role : SQLServerSchemaBase
{
    public enum RoleTypeEnum
    {
        ApplicationRole = 1,
        DatabaseRole = 2
    }

    public Role(ISchemaBase parent)
        : base(parent, ObjectType.Role)
    {
    }

    public override string FullName => $"[{Name}]";

    public RoleTypeEnum Type { get; set; }

    public string Password { get; set; }

    public override string ToSql()
    {
        var sql = "";
        sql += "CREATE " + ((Type == RoleTypeEnum.ApplicationRole) ? "APPLICATION" : "") + " ROLE ";
        sql += FullName + " ";
        sql += "WITH PASSWORD = N'" + Password + "'";
        if (!string.IsNullOrEmpty(Owner))
        {
            sql += " ,DEFAULT_SCHEMA=[" + Owner + "]";
        }

        return sql.Trim() + "\r\nGO\r\n";
    }

    public override string ToSqlDrop() => "DROP " + ((Type == RoleTypeEnum.ApplicationRole) ? "APPLICATION" : "") + " ROLE " + FullName + "\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRole);
        }
        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddRole);
        }
        if ((this.Status & ObjectStatus.Alter) == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropRole);
            listDiff.Add(ToSql(), 0, ScriptAction.AddRole);
        }
        return listDiff;
    }


    public bool Compare(Role obj)
    {
        return obj == null
            ? throw new ArgumentNullException("destination")
            : this.Type == obj.Type && this.Password.Equals(obj.Password) && this.Owner.Equals(obj.Owner);
    }
}
