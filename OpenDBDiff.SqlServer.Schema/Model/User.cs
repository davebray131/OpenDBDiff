using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class User : SQLServerSchemaBase
{
    public User(ISchemaBase parent)
        : base(parent, ObjectType.User)
    {
    }

    public override string FullName => "[" + Name + "]";

    public string Login { get; set; }

    public override string ToSql()
    {
        var sql = "";
        sql += "CREATE USER ";
        sql += FullName + " ";
        if (!string.IsNullOrEmpty(Login))
        {
            sql += "FOR LOGIN [" + Login + "] ";
        }
        else
        {
            sql += "WITHOUT LOGIN ";
        }

        if (!string.IsNullOrEmpty(Owner))
        {
            sql += "WITH DEFAULT_SCHEMA=[" + Owner + "]";
        }

        return sql.Trim() + "\r\nGO\r\n";
    }

    public override string ToSqlDrop() => "DROP USER " + FullName + "\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropUser);
        }
        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddUser);
        }
        if ((this.Status & ObjectStatus.Alter) == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropUser);
            listDiff.Add(ToSql(), 0, ScriptAction.AddUser);
        }
        return listDiff;
    }

    public bool Compare(User obj) => obj == null ? throw new ArgumentNullException("destination") : this.Login.Equals(obj.Login) && this.Owner.Equals(obj.Owner);
}
