using System;
using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class PartitionScheme : SQLServerSchemaBase
{
    public PartitionScheme(ISchemaBase parent)
        : base(parent, ObjectType.PartitionFunction) => FileGroups = [];

    public List<string> FileGroups { get; set; }

    public string PartitionFunction { get; set; }

    public override string ToSqlAdd()
    {
        var sql = "CREATE PARTITION SCHEME " + FullName + "\r\n";
        sql += " AS PARTITION " + PartitionFunction + "\r\n";
        sql += "TO (";
        FileGroups.ForEach(item => sql += "[" + item + "],");
        sql = sql.Substring(0, sql.Length - 1);
        sql += ")\r\nGO\r\n";
        return sql;
    }

    public override string ToSqlDrop() => $"DROP PARTITION SCHEME {FullName}\r\nGO\r\n";

    public override string ToSql() => ToSqlAdd();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropPartitionScheme);
        }
        if (this.Status == ObjectStatus.Rebuild)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropPartitionScheme);
            listDiff.Add(ToSqlAdd(), 0, ScriptAction.AddPartitionScheme);
        }
        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSqlAdd(), 0, ScriptAction.AddPartitionScheme);
        }
        return listDiff;
    }

    public static bool Compare(PartitionScheme origin, PartitionScheme destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException("destination");
        }

        if (origin == null)
        {
            throw new ArgumentNullException("origin");
        }

        if (!origin.PartitionFunction.Equals(destination.PartitionFunction))
        {
            return false;
        }

        if (origin.FileGroups.Count != destination.FileGroups.Count)
        {
            return false;
        }

        for (var j = 0; j < origin.FileGroups.Count; j++)
        {
            if (origin.CompareFullNameTo(origin.FileGroups[j], destination.FileGroups[j]) != 0)
            {
                return false;
            }
        }
        return true;
    }
}
