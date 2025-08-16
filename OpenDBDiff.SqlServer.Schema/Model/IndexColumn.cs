using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model
{
    public class IndexColumn : SQLServerSchemaBase, IComparable<IndexColumn>
    {
        public IndexColumn(ISchemaBase parentObject)
            : base(parentObject, ObjectType.IndexColumn)
        {
        }

        public new IndexColumn Clone(ISchemaBase parent)
        {
            IndexColumn column = new IndexColumn(parent)
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

        public Boolean IsIncluded { get; set; }

        public Boolean Order { get; set; }

        public static Boolean Compare(IndexColumn origin, IndexColumn destination)
        {
            if (destination == null) throw new ArgumentNullException("destination");
            if (origin == null) throw new ArgumentNullException("origin");
            if (origin.IsIncluded != destination.IsIncluded) return false;
            if (origin.Order != destination.Order) return false;
            if (origin.KeyOrder != destination.KeyOrder) return false;
            return true;
        }

        public override string ToSqlDrop()
        {
            return "";
        }

        public override string ToSqlAdd()
        {
            return "";
        }

        public override string ToSql()
        {
            return "";
        }

        public int CompareTo(IndexColumn other)
        {
            /*if (other.Name.Equals(this.Name))
            {*/
            if (other.IsIncluded == this.IsIncluded)
                return this.KeyOrder.CompareTo(other.KeyOrder);
            else
                return other.IsIncluded.CompareTo(this.IsIncluded);
            /*}
            else
                return this.Name.CompareTo(other.Name);*/
        }
    }
}
