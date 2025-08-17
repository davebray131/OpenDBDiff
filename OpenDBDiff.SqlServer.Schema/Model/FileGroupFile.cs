using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class FileGroupFile(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.File)
{
    public override ISchemaBase Clone(ISchemaBase parent) =>
        new FileGroupFile(parent)
        {
            Growth = Growth,
            Id = Id,
            IsPercentGrowth = IsPercentGrowth,
            IsSparse = IsSparse,
            MaxSize = MaxSize,
            Name = Name,
            PhysicalName = PhysicalName,
            Size = Size,
            Type = Type
        };

    public int Size { get; set; }

    public bool IsSparse { get; set; }

    public bool IsPercentGrowth { get; set; }

    private string TypeGrowth => Growth == 0 ? "" : IsPercentGrowth ? "%" : "KB";

    public int Growth { get; set; }

    public int MaxSize { get; set; }

    public string PhysicalName { get; set; }

    public int Type { get; set; }

    private string GetNameNewFileGroup(string path)
    {
        var result = "";
        var flies = path.Split('\\');
        for (var index = 0; index < flies.Length - 1; index++)
        {
            if (!string.IsNullOrEmpty(flies[index]))
            {
                result += $"{flies[index]}\\";
            }
        }

        result += $"{Parent.Parent.Name}_{Name}_DB.ndf";
        return result;
    }

    /// <summary>
    /// Compara dos triggers y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(FileGroupFile origin, FileGroupFile destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null
            ? throw new ArgumentNullException(nameof(origin))
            : origin.Growth == destination.Growth && origin.IsPercentGrowth == destination.IsPercentGrowth && origin.IsSparse == destination.IsSparse && origin.MaxSize == destination.MaxSize && origin.PhysicalName.Equals(destination.PhysicalName);

    public override string ToSql() => Type != 2
            ? $"ALTER DATABASE {Parent.Parent.FullName}\r\nADD{((Type != 1) ? "" : " LOG")} FILE ( NAME = N'{Name}', FILENAME = N'{PhysicalName}' , SIZE = {Size * 1000}KB , FILEGROWTH = {Growth * 1000}{TypeGrowth}) TO FILEGROUP {Parent.FullName}\r\nGO\r\n"
            : $"ALTER DATABASE {Parent.Parent.FullName}\r\nADD{((Type != 1) ? "" : " LOG")} FILE ( NAME = N'{Name}', FILENAME = N'{PhysicalName}') TO FILEGROUP {Parent.FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => Type != 2
            ? $"ALTER DATABASE {Parent.Parent.FullName}\r\nADD{((Type != 1) ? "" : " LOG")} FILE ( NAME = N'{Name}', FILENAME = N'{GetNameNewFileGroup(PhysicalName)}' , SIZE = {Size * 1000}KB , FILEGROWTH = {Growth * 1000}{TypeGrowth}) TO FILEGROUP {Parent.FullName}\r\nGO\r\n"
            : $"ALTER DATABASE {Parent.Parent.FullName}\r\nADD{((Type != 1) ? "" : " LOG")} FILE ( NAME = N'{Name}', FILENAME = N'{GetNameNewFileGroup(PhysicalName)}') TO FILEGROUP {Parent.FullName}\r\nGO\r\n";

    public string ToSQLAlter() => Type != 2
            ? $"ALTER DATABASE {Parent.Parent.FullName} MODIFY FILE ( NAME = N'{Name}', FILENAME = N'{PhysicalName}' , SIZE = {Size * 1000}KB , FILEGROWTH = {Growth * 1000}{TypeGrowth})"
            : $"ALTER DATABASE {Parent.Parent.FullName} MODIFY FILE ( NAME = N'{Name}', FILENAME = N'{PhysicalName}')";

    public override string ToSqlDrop() => $"ALTER DATABASE {Parent.Parent.FullName} REMOVE FILE {FullName}\r\nGO\r\n";
}
