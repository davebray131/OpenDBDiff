using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDBDiff.Abstractions.Schema.Model;

public class SchemaList<T, P> : List<T>, ISchemaList<T, P>
    where T : ISchemaBase
    where P : ISchemaBase
{
    private readonly Dictionary<string, int> nameMap = [];
    private readonly SearchSchemaBase allObjects = null;
    private bool IsCaseSensitive = false;

    public SchemaList(P parent, SearchSchemaBase allObjects)
    {
        Parent = parent;
        this.allObjects = allObjects;
        Comparion = StringComparison.CurrentCultureIgnoreCase;
    }

    public SchemaList<T, P> Clone(P parentObject)
    {
        var options = new SchemaList<T, P>(parentObject, allObjects);
        ForEach(item =>
        {
            object cloned = item.Clone(parentObject);

            //Not everything implements the clone methd, so make sure we got some actual cloned data before adding it back to the list
            if (cloned != null)
            {
                options.Add((T)cloned);
            }
        });

        return options;
    }

    protected StringComparison Comparion { get; private set; }

    public SchemaList(P parent) => Parent = parent;

    public new void Add(T item)
    {
        var db = Parent.RootParent;
        if (!db.Options.Filters.IsItemIncluded(item))
        {
            return;
        }

        base.Add(item);
        allObjects?.Add(item);

        var name = item.FullName;
        IsCaseSensitive = item.RootParent.IsCaseSensitive;
        if (!IsCaseSensitive)
        {
            name = name.ToUpper();
        }

        if (!nameMap.ContainsKey(name))
        {
            nameMap.Add(name, base.Count - 1);
        }
    }
    /// <summary>
    /// Devuelve el objecto Padre perteneciente a la coleccion.
    /// </summary>
    public P Parent { get; private set; }

    /// <summary>
    /// Devuelve el objeto correspondiente a un ID especifico.
    /// </summary>
    /// <param name="id">ID del objecto a buscar</param>
    /// <returns>Si no encontro nada, devuelve null, de lo contrario, el objeto</returns>
    public T Find(int id) => Find(Item => Item.Id == id);

    /// <summary>
    /// Indica si el nombre del objecto existe en la coleccion de objectos del mismo tipo.
    /// </summary>
    /// <param name="table">
    /// Nombre del objecto a buscar.
    /// </param>
    /// <returns></returns>
    public bool Contains(string name) => IsCaseSensitive ? nameMap.ContainsKey(name) : nameMap.ContainsKey(name.ToUpper());

    public virtual T this[string name]
    {
        get
        {
            try
            {
                return IsCaseSensitive ? this[nameMap[name]] : this[nameMap[name.ToUpper()]];
            }
            catch
            {
                return default;
            }
        }
        set
        {
            if (IsCaseSensitive)
            {
                base[nameMap[name]] = value;
            }
            else
            {
                base[nameMap[name.ToUpper()]] = value;
            }
        }
    }

    public virtual SQLScriptList ToSqlDiff() => ToSqlDiff([]);
    public virtual SQLScriptList ToSqlDiff(ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();
        foreach (var item in this.Where(item => schemas.Count() == 0 || schemas.FirstOrDefault(sch => sch.Id == item.Id || (sch.Parent != null && sch.Parent.Id == item.Id)) != default(ISchemaBase)))
        {
            item.ResetWasInsertInDiffList();
            var childrenSchemas = schemas.Where(s => s.Parent != null && s.Parent.Id == item.Id).ToList();
            listDiff.AddRange(item.ToSqlDiff(childrenSchemas).WarnMissingScript(item));
        }
        return listDiff;
    }


    public virtual string ToSql() => string.Join
        (
            "\r\n",
            this
                .Where(item => !item.HasState(ObjectStatus.Drop))
                .Select(item => item.ToSql())
        );
}
