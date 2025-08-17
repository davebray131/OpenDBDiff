using System;
using OpenDBDiff.Abstractions.Schema;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class TablePartition(Table parent) : SQLServerSchemaBase(parent, ObjectType.Partition)
{
    public string CompressType { get; set; }

    public override string ToSql() => throw new NotImplementedException();

    public override string ToSqlDrop() => throw new NotImplementedException();

    public override string ToSqlAdd() => throw new NotImplementedException();
}
