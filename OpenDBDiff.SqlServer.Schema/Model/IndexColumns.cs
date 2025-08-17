using System;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class IndexColumns(ISchemaBase parent) : SchemaList<IndexColumn, ISchemaBase>(parent)
{

    /// <summary>
    /// Clona el objeto ColumnConstraints en una nueva instancia.
    /// </summary>
    public IndexColumns Clone()
    {
        var columns = new IndexColumns(Parent);
        for (var index = 0; index < Count; index++)
        {
            columns.Add(this[index].Clone(Parent));
        }
        return columns;
    }

    /// <summary>
    /// Compara dos campos y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(IndexColumns origin, IndexColumns destination)
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
                if (!IndexColumn.Compare(origin[j], item))
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
                if (!IndexColumn.Compare(destination[j], item))
            {
                return false;
            }
        }
        return true;
    }
}
