using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model.Util;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class StoredProcedure : Code
{
    public StoredProcedure(ISchemaBase parent)
        : base(parent, ObjectType.StoredProcedure, ScriptAction.AddStoredProcedure, ScriptAction.DropStoredProcedure)
    {

    }

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var item = new StoredProcedure(parent)
        {
            Text = Text,
            Status = Status,
            Name = Name,
            Id = Id,
            Owner = Owner,
            Guid = Guid
        };
        return item;
    }

    public override bool IsCodeType => true;

    public override string ToSql() => FormatCode.FormatCreate("PROC(EDURE)?", Text, this);

    public string ToSQLAlter() => FormatCode.FormatAlter("PROC(EDURE)?", ToSql(), this, false);

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status != ObjectStatus.Original)
        {
            RootParent.ActionMessage.Add(this);
        }

        if (HasState(ObjectStatus.Drop))
        {
            list.Add(Drop());
        }

        if (HasState(ObjectStatus.Create))
        {
            list.Add(Create());
        }

        if (HasState(ObjectStatus.Alter))
        {
            list.Add(ToSQLAlter(), 0, ScriptAction.AlterProcedure);
        }

        if (HasState(ObjectStatus.AlterWhitespace))
        {
            list.Add(ToSQLAlter(), 0, ScriptAction.AlterProcedure);
        }

        return list;
    }
}
