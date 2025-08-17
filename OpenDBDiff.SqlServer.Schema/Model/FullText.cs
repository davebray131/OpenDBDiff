using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class FullText(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.FullText)
{
    public override string FullName => $"[{Name}]";

    public string Path { get; set; }

    public bool IsDefault { get; set; }

    public bool IsAccentSensity { get; set; }

    public string FileGroupName { get; set; }

    public override string ToSql()
    {
        var database = (Database)Parent;

        var sql = $"CREATE FULLTEXT CATALOG {FullName} WITH ACCENT_SENSITIVITY = {OnOff(IsAccentSensity)}\r\n";

        if (!string.IsNullOrEmpty(Path))
        {
            if (!database.Options.Ignore.FilterFullTextPath)
            {
                sql += "--";
            }

            sql += $"IN PATH N'{Path}'\r\n";
        }
        if (IsDefault)
        {
            sql += "AS DEFAULT\r\n";
        }

        sql += $"AUTHORIZATION [{Owner}]\r\n";
        return sql + "GO\r\n";
    }

    private string ToSqlAlterDefault() =>
        IsDefault ? $"ALTER FULLTEXT CATALOG {FullName} AS DEFAULT\r\nGO\r\n" : string.Empty;
    private string ToSqlAlterOwner() =>
        $"ALTER AUTHORIZATION ON FULLTEXT CATALOG::{FullName}\r\nTO [{Owner}]\r\nGO\r\n";

    private string ToSqlAlter()
    {
        var sql = $"ALTER FULLTEXT CATALOG {FullName}\r\n";
        sql += $"REBUILD WITH ACCENT_SENSITIVITY = {OnOff(IsAccentSensity)}";
        sql += "\r\nGO\r\n";
        return sql;
    }

    public override string ToSqlDrop() => $"DROP FULLTEXT CATALOG {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropFullText);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddFullText);
        }
        if (HasState(ObjectStatus.Alter))
        {
            listDiff.Add(ToSqlAlter(), 0, ScriptAction.AddFullText);
        }
        if (HasState(ObjectStatus.Disabled))
        {
            listDiff.Add(ToSqlAlterDefault(), 0, ScriptAction.AddFullText);
        }
        if (HasState(ObjectStatus.ChangeOwner))
        {
            listDiff.Add(ToSqlAlterOwner(), 0, ScriptAction.AddFullText);
        }
        return listDiff;
    }

    /// <summary>
    /// Compara dos Synonyms y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public bool Compare(FullText destination)
    {
        var database = (Database)Parent;
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (!IsAccentSensity.Equals(destination.IsAccentSensity))
        {
            return false;
        }

        if (!IsDefault.Equals(destination.IsDefault))
        {
            return false;
        }

        if ((!string.IsNullOrEmpty(FileGroupName)) && (!string.IsNullOrEmpty(destination.FileGroupName)))
        {
            if (!FileGroupName.Equals(destination.FileGroupName))
            {
                return false;
            }
        }

        if (database.Options.Ignore.FilterFullTextPath)
        {
            if ((!string.IsNullOrEmpty(Path)) && (!string.IsNullOrEmpty(destination.Path)))
            {
                return Path.Equals(destination.Path, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        return true;
    }
}
