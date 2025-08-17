using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class TableOption : SQLServerSchemaBase
{
    public TableOption(string name, string value, ISchemaBase parent)
        : base(parent, ObjectType.TableOption)
    {
        Name = name;
        Value = value;
    }

    public TableOption(ISchemaBase parent)
        : base(parent, ObjectType.TableOption)
    {
    }

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var option = new TableOption(parent)
        {
            Name = Name,
            Status = Status,
            Value = Value
        };
        return option;
    }

    public string Value { get; set; }

    /// <summary>
    /// Compara dos indices y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(TableOption origin, TableOption destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null ? throw new ArgumentNullException(nameof(origin)) : destination.Value.Equals(origin.Value);

    public override string ToSqlDrop() => Name.Equals("TextInRow")
            ? $"EXEC sp_tableoption {Parent.Name}, 'text in row','off'\r\nGO\r\n"
            : Name.Equals("LargeValues")
            ? $"EXEC sp_tableoption {Parent.Name}, 'large value types out of row','0'\r\nGO\r\n"
            : Name.Equals("VarDecimal")
            ? $"EXEC sp_tableoption {Parent.Name}, 'vardecimal storage format','0'\r\nGO\r\n"
            : Name.Equals("LockEscalation") ? "" : "";

    public override string ToSql()
    {
        if (Name.Equals("TextInRow"))
        {
            return $"EXEC sp_tableoption {Parent.Name}, 'text in row',{Value}\r\nGO\r\n";
        }

        if (Name.Equals("LargeValues"))
        {
            return $"EXEC sp_tableoption {Parent.Name}, 'large value types out of row',{Value}\r\nGO\r\n";
        }

        if (Name.Equals("VarDecimal"))
        {
            return $"EXEC sp_tableoption {Parent.Name}, 'vardecimal storage format','1'\r\nGO\r\n";
        }

        if (Name.Equals("LockEscalation"))
        {
            if ((!Value.Equals("TABLE")) || (Status != ObjectStatus.Original))
            {
                return $"ALTER TABLE {Parent.Name} SET (LOCK_ESCALATION = {Value})\r\nGO\r\n";
            }
        }
        return string.Empty;
    }

    public override string ToSqlAdd() => ToSql();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.AddOptions);
        }

        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSql(), 0, ScriptAction.DropOptions);
        }

        if (Status == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropOptions);
            listDiff.Add(ToSql(), 0, ScriptAction.AddOptions);
        }
        return listDiff;
    }
}
