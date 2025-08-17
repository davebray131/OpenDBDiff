using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenDBDiff.SqlServer.Schema.Model;

/// <summary>
/// Constructor de la clase.
/// </summary>
/// <param name="parent">
/// Objeto Database padre.
/// </param>
public class FileGroupFiles(FileGroup parent) : List<FileGroupFile>
{
    private readonly Hashtable hash = [];

    /// <summary>
    /// Clona el objeto FileGroups en una nueva instancia.
    /// </summary>
    public FileGroupFiles Clone(FileGroup parentObject)
    {
        var columns = new FileGroupFiles(parentObject);
        for (var index = 0; index < Count; index++)
        {
            columns.Add((FileGroupFile)this[index].Clone(parentObject));
        }
        return columns;
    }

    /// <summary>
    /// Indica si el nombre del FileGroup existe en la coleccion de tablas del objeto.
    /// </summary>
    /// <param name="table">
    /// Nombre de la tabla a buscar.
    /// </param>
    /// <returns></returns>
    public bool Find(string table) => hash.ContainsKey(table);

    /// <summary>
    /// Agrega un objeto columna a la coleccion de columnas.
    /// </summary>
    public new void Add(FileGroupFile file)
    {
        if (file != null)
        {
            hash.Add(file.FullName, file);
            base.Add(file);
        }
        else
        {
            throw new ArgumentNullException(nameof(file));
        }
    }

    public FileGroupFile this[string name]
    {
        get => (FileGroupFile)hash[name];
        set
        {
            hash[name] = value;
            for (var index = 0; index < base.Count; index++)
            {
                if (base[index].Name.Equals(name))
                {
                    base[index] = value;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Devuelve la tabla perteneciente a la coleccion de campos.
    /// </summary>
    public FileGroup Parent { get; private set; } = parent;

    public string ToSQL()
    {
        var sql = new StringBuilder();
        for (var index = 0; index < Count; index++)
        {
            sql.Append(this[index].ToSql());
        }
        return sql.ToString();
    }

    public string ToSQLDrop()
    {
        var sql = new StringBuilder();
        for (var index = 0; index < Count; index++)
        {
            sql.Append(this[index].ToSqlDrop());
        }
        return sql.ToString();
    }
}
