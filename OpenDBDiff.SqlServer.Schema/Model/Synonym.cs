using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Synonym(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.Synonym)
{
    public override ISchemaBase Clone(ISchemaBase parent) =>
        new Synonym(parent)
        {
            Id = Id,
            Name = Name,
            Owner = Owner,
            Value = Value,
            Guid = Guid
        };

    public string Value { get; set; }

    public override string ToSql() => $"CREATE SYNONYM {FullName} FOR {Value}\r\nGO\r\n";

    public override string ToSqlDrop() => $"DROP SYNONYM {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropSynonyms);
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.AddSynonyms);
        }
        if (Status == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropSynonyms);
            listDiff.Add(ToSql(), 0, ScriptAction.AddSynonyms);
        }
        return listDiff;
    }

    /// <summary>
    /// Compara dos Synonyms y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(Synonym origin, Synonym destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null ? throw new ArgumentNullException(nameof(origin)) : origin.Value.Equals(destination.Value);
}
