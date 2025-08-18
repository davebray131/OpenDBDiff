using OpenDBDiff.Abstractions.Schema;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class ObjectDependency(string name, string columnName, ObjectType? type = null)
{
    public string Name { get; set; } = name;

    public string ColumnName { get; set; } = columnName;

    public ObjectType Type { get; set; } = type ?? ObjectType.None;

    public bool IsCodeType => (Type == ObjectType.StoredProcedure) || (Type == ObjectType.Trigger) || (Type == ObjectType.View) || (Type == ObjectType.Function);
}
