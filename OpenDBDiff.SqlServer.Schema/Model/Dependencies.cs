using System;
using System.Collections.Generic;
using System.Linq;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

internal class Dependencies : List<Dependency>
{
    public Database Database { get; private set; }

    public void Add(Database database, int tableId, int columnId, int ownerTableId, int typeId, ISchemaBase constraint)
    {
        var dependency = new Dependency
        {
            SubObjectId = columnId,
            ObjectId = tableId,
            OwnerTableId = ownerTableId,

            FullName = constraint.FullName,
            Type = constraint.ObjectType,
            DataTypeId = typeId
        };
        Database = database;
        base.Add(dependency);
    }

    public void Add(Database database, int objectId, ISchemaBase objectSchema)
    {
        var dependency = new Dependency
        {
            ObjectId = objectId,
            FullName = objectSchema.FullName,
            Type = objectSchema.ObjectType
        };
        Database = database;
        base.Add(dependency);
    }

    /// <summary>
    /// Devuelve todos las constraints dependientes de una tabla.
    /// </summary>
    public List<ISchemaBase> FindNotOwner(int tableId, ObjectType type)
    {
        try
        {
            List<ISchemaBase> cons = [];
            ForEach(dependency =>
            {
                if (dependency.Type == type)
                {
                    var item = Database.Find(dependency.FullName);
                    if (dependency.Type == ObjectType.Constraint)
                    {
                        if ((dependency.ObjectId == tableId) && (((Constraint)item).Type == Constraint.ConstraintType.ForeignKey))
                        {
                            cons.Add(item);
                        }
                    }
                    else
                        if (dependency.ObjectId == tableId)
                    {
                        cons.Add(item);
                    }
                }

            });
            return cons;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    /// <summary>
    /// Devuelve todos las constraints dependientes de una tabla.
    /// </summary>
    /*public void Set(int tableId, Constraint constraint)
    {
        this.ForEach(item =>
        {
            if (item.Type == ObjectType.Constraint)
                if ((item.ObjectId == tableId) && (item.ObjectSchema.Name.Equals(constraint.Name)))
                    item.ObjectSchema = constraint;
        });
    }*/

    /// <summary>
    /// Devuelve todos las constraints dependientes de una tabla.
    /// </summary>
    public List<ISchemaBase> Find(int tableId) => Find(tableId, 0, 0);

    public int DependenciesCount(int objectId, ObjectType type)
    {
        Dictionary<int, bool> depencyTracker = [];
        return DependenciesCount(objectId, type, depencyTracker);
    }

    private int DependenciesCount(int tableId, ObjectType type, Dictionary<int, bool> depencyTracker)
    {
        var count = 0;
        var putItem = false;
        int relationalTableId;
        var constraints = FindNotOwner(tableId, type);
        for (var index = 0; index < constraints.Count; index++)
        {
            var cons = constraints[index];
            if (cons.ObjectType == type)
            {
                if (type == ObjectType.Constraint)
                {
                    relationalTableId = ((Constraint)cons).RelationalTableId;
                    putItem = relationalTableId == tableId;
                }
            }
            if (putItem)
            {
                if (!depencyTracker.ContainsKey(tableId))
                {
                    depencyTracker.Add(tableId, true);
                    count += 1 + DependenciesCount(cons.Parent.Id, type, depencyTracker);
                }
            }
        }
        return count;
    }

    /// <summary>
    /// Devuelve todos las constraints dependientes de una tabla y una columna.
    /// </summary>
    public List<ISchemaBase> Find(int tableId, int columnId, int dataTypeId)
    {
        List<string> cons = [];
        List<ISchemaBase> real = [];

        cons = (from depends in this
                where (depends.Type == ObjectType.Constraint || depends.Type == ObjectType.Index) &&
                (depends.DataTypeId == dataTypeId || dataTypeId == 0) && (depends.SubObjectId == columnId || columnId == 0) && (depends.ObjectId == tableId)
                select depends.FullName)
                    .Concat(from depends in this
                            where (depends.Type == ObjectType.View || depends.Type == ObjectType.Function) &&
                            (depends.ObjectId == tableId)
                            select depends.FullName).ToList();

        cons.ForEach(item =>
            {
                var schema = Database.Find(item);
                if (schema != null)
                {
                    real.Add(schema);
                }
            }
        );
        return real;
    }
}
