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
            IsDefaultFileGroup = IsDefaultFileGroup,
            IsReadOnly = IsReadOnly,
            Name = Name,
            Id = Id,
            Guid = Guid,
            IsFileStream = IsFileStream,
        };
        file.Files = Files.Clone(file);
        return file;
    }

    public FileGroupFiles Files { get; set; }

    public bool IsFileStream { get; set; }

    public bool IsDefaultFileGroup { get; set; }

    public bool IsReadOnly { get; set; }

    public static bool Compare(FileGroup origin, FileGroup destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null
            ? throw new ArgumentNullException(nameof(origin))
            : origin.IsReadOnly == destination.IsReadOnly && origin.IsDefaultFileGroup == destination.IsDefaultFileGroup && origin.IsFileStream == destination.IsFileStream;

    private string ToSQL(string action)
    {
        var sql = $"ALTER DATABASE [{Parent.Name}] {action} ";
        sql += $"FILEGROUP [{Name}]";
        if (action.Equals("MODIFY"))
        {
            if (IsDefaultFileGroup)
            {
                sql += " DEFAULT";
            }
        }
        else if (IsFileStream)
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

        Files.ForEach(file => sql += file.ToSql());
        if (IsDefaultFileGroup)
        {
            sql += ToSQL("MODIFY");
        }
        return sql;
    }

    public override string ToSqlAdd()
    {
        var sql = ToSQL("ADD");
        Files.ForEach(file => sql += file.ToSqlAdd());

        if (IsDefaultFileGroup)
        {
            sql += ToSQL("MODIFY");
        }

        return sql;
    }

    public string ToSQLAlter() => ToSQL("MODIFY");

    public override string ToSqlDrop() =>
       $"{Files.ToSQLDrop()}ALTER DATABASE [{Parent.Name}] REMOVE FILEGROUP [{Name}]\r\nGO\r\n\r\n";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        switch (Status)
        {
            case ObjectStatus.Drop:
                listDiff.Add(ToSqlDrop(), 1, ScriptAction.DropFileGroup);
                break;
            case ObjectStatus.Create:
                listDiff.Add(ToSqlAdd(), 1, ScriptAction.AddFileGroup);
                break;
            case ObjectStatus.Alter:
                listDiff.Add(ToSQLAlter(), 1, ScriptAction.AlterFileGroup);
                break;
        }

        return listDiff;
    }
}
