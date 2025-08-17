using System;
using System.Collections.Generic;

namespace OpenDBDiff.Abstractions.Schema.Model;

public class SearchSchemaBase
{
    private readonly Dictionary<string, ObjectType> objectTypes;
    private readonly Dictionary<string, string> objectParent;
    private readonly Dictionary<int, string> objectId;

    public SearchSchemaBase()
    {
        objectTypes = [];
        objectParent = [];
        objectId = [];
    }

    public void Add(ISchemaBase item)
    {
        if (objectTypes.ContainsKey(item.FullName.ToUpper()))
        {
            objectTypes.Remove(item.FullName.ToUpper());
        }

        objectTypes.Add(item.FullName.ToUpper(), item.ObjectType);
        if ((item.ObjectType == ObjectType.Constraint) || (item.ObjectType == ObjectType.Index) || (item.ObjectType == ObjectType.Trigger) || (item.ObjectType == ObjectType.CLRTrigger))
        {
            if (objectParent.ContainsKey(item.FullName.ToUpper()))
            {
                objectParent.Remove(item.FullName.ToUpper());
            }

            objectParent.Add(item.FullName.ToUpper(), item.Parent.FullName);

            if (objectId.ContainsKey(item.Id))
            {
                objectId.Remove(item.Id);
            }

            objectId.Add(item.Id, item.FullName);
        }
    }


    public Nullable<ObjectType> GetType(string FullName) => objectTypes.ContainsKey(FullName.ToUpper()) ? objectTypes[FullName.ToUpper()] : null;

    public string GetParentName(string FullName) => objectParent[FullName.ToUpper()];

    public string GetFullName(int Id) => objectId[Id];
}
