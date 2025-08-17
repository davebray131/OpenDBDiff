using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model.Util;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Function(ISchemaBase parent) : Code(parent, ObjectType.Function, ScriptAction.AddFunction, ScriptAction.DropFunction)
{

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var item = new Function(parent)
        {
            Text = Text,
            Status = Status,
            Name = Name,
            Id = Id,
            Owner = Owner,
            Guid = Guid,
            IsSchemaBinding = IsSchemaBinding
        };
        DependenciesIn.ForEach(item.DependenciesIn.Add);
        DependenciesOut.ForEach(item.DependenciesOut.Add);
        return item;
    }

    public override bool IsCodeType => true;

    public string ToSQLAlter() => ToSQLAlter(false);

    public string ToSQLAlter(bool quitSchemaBinding) => FormatCode.FormatAlter("FUNCTION", ToSql(), this, quitSchemaBinding);

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
            if (HasState(ObjectStatus.RebuildDependencies))
            {
                list.AddRange(RebuildDependencies());
            }

            if (!GetWasInsertInDiffList(ScriptAction.DropFunction))
            {
                if (HasState(ObjectStatus.Rebuild))
                {
                    list.Add(Drop());
                    list.Add(Create());
                }
                if (HasState(ObjectStatus.AlterBody))
                {
                    var iCount = DependenciesCount;
                    list.Add(ToSQLAlter(), iCount, ScriptAction.AlterFunction);
                }
            }
        }
        return list;
    }
}
