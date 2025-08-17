using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class IndexColumn : SQLServerSchemaBase, IComparable<IndexColumn>
{
    public IndexColumn(ISchemaBase parentObject)
        : base(parentObject, ObjectType.IndexColumn)
    {
    }

    public new IndexColumn Clone(ISchemaBase parent)
    {
        var column = new IndexColumn(parent)
        {
            Id = this.Id,
            IsIncluded = this.IsIncluded,
            Name = this.Name,
            Order = this.Order,
            Status = this.Status,
            KeyOrder = this.KeyOrder,
            DataTypeId = this.DataTypeId
        };
        return column;
    }

    public int DataTypeId { get; set; }

    public int KeyOrder { get; set; }

    public bool IsIncluded { get; set; }

    public bool Order { get; set; }

    public static bool Compare(IndexColumn origin, IndexColumn destination)
    {
        return destination == null
            ? throw new ArgumentNullException("destination")
            : origin == null
            ? throw new ArgumentNullException("origin")
            : origin.IsIncluded == destination.IsIncluded && origin.Order == destination.Order && origin.KeyOrder == destination.KeyOrder;
    }

    public override string ToSqlDrop() => string.Empty;

    public override string ToSqlAdd() => string.Empty;

    public override string ToSql() => string.Empty;

    public int CompareTo(IndexColumn other) =>
        /*if (other.Name.Equals(this.Name))
{*/
        other.IsIncluded == this.IsIncluded ? this.KeyOrder.CompareTo(other.KeyOrder) : other.IsIncluded.CompareTo(this.IsIncluded);/*}
else
return this.Name.CompareTo(other.Name);*/
}
