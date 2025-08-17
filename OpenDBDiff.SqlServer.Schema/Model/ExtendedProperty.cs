using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class ExtendedProperty(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.ExtendedProperty), ISchemaBase
{
    public override string FullName
    {
        get
        {
            var normal = $"[{Level0name}]{(string.IsNullOrEmpty(Level1name) ? "" : ".[" + Level1name + "]")}{(string.IsNullOrEmpty(Level2name) ? "" : ".[" + Level2name + "]")}";
            return string.IsNullOrEmpty(Level1type) || string.IsNullOrEmpty(Level2type)
                ? normal
                : !Level2type.Equals("TRIGGER") ? normal : $"[{Level0name}].[{Level2name}]";
        }
    }

    public string Level2name { get; set; }

    public string Level2type { get; set; }

    public string Level1name { get; set; }

    public string Level1type { get; set; }

    public string Level0name { get; set; }

    public string Level0type { get; set; }

    public string Value { get; set; }

    public override SQLScript Create()
    {
        var action = ScriptAction.AddExtendedProperty;
        return new SQLScript(ToSqlAdd(), 0, action);
    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropExtendedProperty;
        return new SQLScript(ToSqlDrop(), 0, action);
    }

    public override ObjectStatus Status { get; set; }

    public override string ToSqlAdd()
    {
        var sql = $"EXEC sys.sp_addextendedproperty @name=N'{Name}', @value=N'{Value}' ,";
        sql += $"@level0type=N'{Level0type}',@level0name=N'{Level0name}'";
        if (!string.IsNullOrEmpty(Level1name))
        {
            sql += $", @level1type=N'{Level1type}',@level1name=N'{Level1name}'";
        }

        if (!string.IsNullOrEmpty(Level2name))
        {
            sql += $", @level2type=N'{Level2type}',@level2name=N'{Level2name}'";
        }

        return sql + "\r\nGO\r\n";
    }

    public override string ToSqlDrop()
    {
        var sql = $"EXEC sys.sp_dropextendedproperty @name=N'{Name}', @value=N'{Value}' ,";
        sql += $"@level0type=N'{Level0type}',@level0name=N'{Level0name}'";
        if (!string.IsNullOrEmpty(Level1name))
        {
            sql += $", @level1type=N'{Level1type}',@level1name=N'{Level1name}'";
        }

        if (!string.IsNullOrEmpty(Level2name))
        {
            sql += $", @level2type=N'{Level2type}',@level2name=N'{Level2name}'";
        }

        return sql + "\r\nGO\r\n";
    }

    public override string ToSql() => ToSqlAdd();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Parent.Status != ObjectStatus.Create)
        {
            if (Status == ObjectStatus.Create)
            {
                list.Add(Create());
            }

            if (Status == ObjectStatus.Drop)
            {
                list.Add(Drop());
            }
        }
        return list;
    }
}
