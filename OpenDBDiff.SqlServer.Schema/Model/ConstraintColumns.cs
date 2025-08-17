using System;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class ConstraintColumns(Constraint parent) : SchemaList<ConstraintColumn, Constraint>(parent)
{

    /// <summary>
    /// Clona el objeto ColumnConstraints en una nueva instancia.
    /// </summary>
    public ConstraintColumns Clone()
    {
        var columns = new ConstraintColumns(Parent);
        for (var index = 0; index < Count; index++)
        {
            columns.Add(this[index].Clone());
        }
        return columns;
    }

    /// <summary>
    /// Compara dos campos y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(ConstraintColumns origin, ConstraintColumns destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (origin.Count != destination.Count)
        {
            return false;
        }

        for (var j = 0; j < origin.Count; j++)
        {
            var item = destination[origin[j].FullName];
            if (item == null)
            {
                return false;
            }
            else
                if (!ConstraintColumn.Compare(origin[j], item))
            {
                return false;
            }
        }
        for (var j = 0; j < destination.Count; j++)
        {
            var item = origin[destination[j].FullName];
            if (item == null)
            {
                return false;
            }
            else
                if (!ConstraintColumn.Compare(destination[j], item))
            {
                return false;
            }
        }
        return true;
    }
}
