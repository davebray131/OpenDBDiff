using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public abstract class SQLServerSchemaBase : SchemaBase, ISQLServerSchemaBase
{
    protected const string GO = "GO\r\n";
    protected SQLServerSchemaBase(ISchemaBase parent, ObjectType objectType) : base("[", "]", objectType)
    {
        Parent = parent;
        ExtendedProperties = new SchemaList<ExtendedProperty, ISchemaBase>(parent);
    }

    public SchemaList<ExtendedProperty, ISchemaBase> ExtendedProperties { get; private set; }

    protected string OnOff(bool value, string isTrue = "ON", string isFalse = "OFF") => value ? isTrue : isFalse;

}
