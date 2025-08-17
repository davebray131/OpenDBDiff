using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class User(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.User)
{
    public override string FullName => "[" + Name + "]";

    public string Login { get; set; }

    public override string ToSql()
    {
        var sql = $"CREATE USER {FullName} ";
        sql += !string.IsNullOrEmpty(Login) ? $"FOR LOGIN [{Login}] " : "WITHOUT LOGIN ";

        if (!string.IsNullOrEmpty(Owner))
        {
            sql += $"WITH DEFAULT_SCHEMA=[{Owner}]";
        }

        return sql.Trim() + "\r\nGO\r\n";
    }

    public override string ToSqlDrop() => $"DROP USER {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropUser);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddUser);
        }
        if ((Status & ObjectStatus.Alter) == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropUser);
            listDiff.Add(ToSql(), 0, ScriptAction.AddUser);
        }
        return listDiff;
    }

    public bool Compare(User obj) => obj == null ? throw new ArgumentNullException(nameof(obj)) : Login.Equals(obj.Login) && Owner.Equals(obj.Owner);
}
