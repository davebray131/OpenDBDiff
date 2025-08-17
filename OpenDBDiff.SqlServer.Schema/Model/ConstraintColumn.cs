using System;
using OpenDBDiff.Abstractions.Schema;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class ConstraintColumn : SQLServerSchemaBase, IComparable<ConstraintColumn>
{
    public ConstraintColumn(Constraint parentObject)
        : base(parentObject, ObjectType.ConstraintColumn)
    {
    }

    public ConstraintColumn Clone()
    {
        var ccol = new ConstraintColumn((Constraint)this.Parent)
        {
            ColumnRelationalName = this.ColumnRelationalName,
            ColumnRelationalId = this.ColumnRelationalId,
            Name = this.Name,
            IsIncluded = this.IsIncluded,
            Order = this.Order,
            KeyOrder = this.KeyOrder,
            Id = this.Id,
            DataTypeId = this.DataTypeId,
            ColumnRelationalDataTypeId = this.ColumnRelationalDataTypeId
        };
        return ccol;
    }

    public int DataTypeId { get; set; }

    public int ColumnRelationalDataTypeId { get; set; }

    public int ColumnRelationalId { get; set; }

    /// <summary>
    /// Gets or sets the column key order in the index.
    /// </summary>
    /// <value>The key order.</value>
    public int KeyOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this column is included in the index leaf page.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this column is included; otherwise, <c>false</c>.
    /// </value>
    public bool IsIncluded { get; set; }

    /// <summary>
    /// Orden de la columna (Ascendente o Descendente). Se usa solo en Primary Keys.
    /// </summary>
    public bool Order { get; set; }

    public string ColumnRelationalName { get; set; }

    public override string ToSqlDrop() => string.Empty;

    public override string ToSqlAdd() => string.Empty;

    public override string ToSql() => string.Empty;

    public static bool Compare(ConstraintColumn origin, ConstraintColumn destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException("destination");
        }

        if (origin == null)
        {
            throw new ArgumentNullException("origin");
        }

        if ((origin.ColumnRelationalName == null) && (destination.ColumnRelationalName != null))
        {
            return false;
        }

        if (origin.ColumnRelationalName != null)
        {
            if (!origin.ColumnRelationalName.Equals(destination.ColumnRelationalName, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
        }
        return origin.IsIncluded == destination.IsIncluded && origin.Order == destination.Order && origin.KeyOrder == destination.KeyOrder;
    }

    public int CompareTo(ConstraintColumn other) => this.ColumnRelationalId.CompareTo(other.ColumnRelationalId);
}
