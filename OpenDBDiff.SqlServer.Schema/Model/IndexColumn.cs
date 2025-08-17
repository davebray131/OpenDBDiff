using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class IndexColumn(ISchemaBase parentObject) : SQLServerSchemaBase(parentObject, ObjectType.IndexColumn), IComparable<IndexColumn>
{
    public new IndexColumn Clone(ISchemaBase parent)
    {
        var column = new IndexColumn(parent)
        {
            Id = Id,
            IsIncluded = IsIncluded,
            Name = Name,
            Order = Order,
            Status = Status,
            KeyOrder = KeyOrder,
            DataTypeId = DataTypeId
        };
        return column;
    }

    public int DataTypeId { get; set; }

    public int KeyOrder { get; set; }

    public bool IsIncluded { get; set; }

    public bool Order { get; set; }

    public static bool Compare(IndexColumn origin, IndexColumn destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null
            ? throw new ArgumentNullException(nameof(origin))
            : origin.IsIncluded == destination.IsIncluded && origin.Order == destination.Order && origin.KeyOrder == destination.KeyOrder;

    public override string ToSqlDrop() => string.Empty;

    public override string ToSqlAdd() => string.Empty;

    public override string ToSql() => string.Empty;

    public int CompareTo(IndexColumn other) =>
        other.IsIncluded == IsIncluded ? KeyOrder.CompareTo(other.KeyOrder) : other.IsIncluded.CompareTo(IsIncluded);
}
