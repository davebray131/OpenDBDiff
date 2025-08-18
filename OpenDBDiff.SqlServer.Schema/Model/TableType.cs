using System.Linq;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class TableType : SQLServerSchemaBase, ITable<TableType>
{
    public TableType(ISchemaBase parent)
        : base(parent, ObjectType.TableType)
    {
        Columns = new Columns<TableType>(this);
        Constraints = new SchemaList<Constraint, TableType>(this, ((Database)parent).AllObjects);
        Indexes = new SchemaList<Index, TableType>(this, ((Database)parent).AllObjects);
    }

    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var tableType = new TableType(parent)
        {
            Owner = Owner,
            Name = Name,
            Id = Id,
            Guid = Guid,
            Status = Status,
            Columns = null,
            Constraints = null,
            Indexes = null
        };

        tableType.Columns = Columns.Clone(tableType);
        tableType.Constraints = Constraints.Clone(tableType);
        tableType.Indexes = Indexes.Clone(tableType);

        return tableType;
    }

    public Columns<TableType> Columns { get; private set; }

    public SchemaList<Constraint, TableType> Constraints { get; private set; }

    public SchemaList<Index, TableType> Indexes { get; private set; }

    public override string ToSql()
    {
        var sql = string.Empty;
        if (Columns.Any())
        {
            sql += "CREATE TYPE " + FullName + " AS TABLE\r\n(\r\n";
            sql += Columns.ToSql() + "\r\n";
            sql += Constraints.ToSql();
            sql += ")";
            sql += "\r\nGO\r\n";
        }
        return sql;
    }

    public override string ToSqlDrop() => $"DROP TYPE {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScript Create()
    {
        var action = ScriptAction.AddTableType;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlAdd(), 0, action);
        }
        return null;
    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropTableType;
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
                list.Add(Drop());
            }
            if (HasState(ObjectStatus.Create))
            {
                list.Add(Create());
            }
            if (Status == ObjectStatus.Alter)
            {
                list.Add(ToSqlDrop() + ToSql(), 0, ScriptAction.AddTableType);
            }
            return list;
        }
        catch
        {
            return null;
        }
    }
}
