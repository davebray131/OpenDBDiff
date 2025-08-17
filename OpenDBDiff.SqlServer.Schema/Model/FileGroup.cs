using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class FileGroup : SQLServerSchemaBase
{
    public FileGroup(ISchemaBase parent)
        : base(parent, ObjectType.FileGroup) => Files = new FileGroupFiles(this);

    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var file = new FileGroup(parent)
        {
            IsDefaultFileGroup = this.IsDefaultFileGroup,
            IsReadOnly = this.IsReadOnly,
            Name = this.Name,
            Id = this.Id
        };
        file.Files = this.Files.Clone(file);
        file.Guid = this.Guid;
        file.IsFileStream = this.IsFileStream;
        return file;
    }

    public FileGroupFiles Files { get; set; }

    public bool IsFileStream { get; set; }

    public bool IsDefaultFileGroup { get; set; }

    public bool IsReadOnly { get; set; }

    public static bool Compare(FileGroup origin, FileGroup destination)
    {
        return destination == null
            ? throw new ArgumentNullException("destination")
            : origin == null
            ? throw new ArgumentNullException("origin")
            : origin.IsReadOnly == destination.IsReadOnly && origin.IsDefaultFileGroup == destination.IsDefaultFileGroup && origin.IsFileStream == destination.IsFileStream;
    }

    private string ToSQL(string action)
    {
        var sql = "ALTER DATABASE [" + Parent.Name + "] " + action + " ";
        sql += "FILEGROUP [" + Name + "]";
        if (action.Equals("MODIFY"))
        {
            if (IsDefaultFileGroup)
            {
                sql += " DEFAULT";
            }
        }
        else
            if (IsFileStream)
        {
            sql += " CONTAINS FILESTREAM";
        }

        if (IsReadOnly)
        {
            sql += " READONLY";
        }

        sql += "\r\nGO\r\n";
        return sql;
    }

    public override string ToSql()
    {
        var sql = ToSQL("ADD");
        foreach (var file in this.Files)
        {
            sql += file.ToSql();
        }

        if (IsDefaultFileGroup)
        {
            sql += ToSQL("MODIFY");
        }

        return sql;
    }

    public override string ToSqlAdd()
    {
        var sql = ToSQL("ADD");
        foreach (var file in this.Files)
        {
            sql += file.ToSqlAdd();
        }

        if (IsDefaultFileGroup)
        {
            sql += ToSQL("MODIFY");
        }

        return sql;
    }

    public string ToSQLAlter() => ToSQL("MODIFY");

    public override string ToSqlDrop()
    {
        var sql = Files.ToSQLDrop();
        return sql + "ALTER DATABASE [" + Parent.Name + "] REMOVE FILEGROUP [" + Name + "]\r\nGO\r\n\r\n";
    }

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.Add(this.ToSqlDrop(), 1, ScriptAction.DropFileGroup);
        }

        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(this.ToSqlAdd(), 1, ScriptAction.AddFileGroup);
        }

        if (this.Status == ObjectStatus.Alter)
        {
            listDiff.Add(this.ToSQLAlter(), 1, ScriptAction.AlterFileGroup);
        }

        return listDiff;
    }
}
