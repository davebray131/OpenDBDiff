using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Trigger : Code
{
    public Trigger(ISchemaBase parent)
        : base(parent, ObjectType.Trigger, ScriptAction.AddTrigger, ScriptAction.DropTrigger) => this.Parent = parent;

    /// <summary>
    /// Clona el objeto en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var trigger = new Trigger(parent)
        {
            Text = this.Text,
            Status = this.Status,
            Name = this.Name,
            IsDisabled = this.IsDisabled,
            InsteadOf = this.InsteadOf,
            NotForReplication = this.NotForReplication,
            Owner = this.Owner,
            Id = this.Id,
            IsDDLTrigger = this.IsDDLTrigger,
            Guid = this.Guid
        };
        return trigger;
    }

    public bool IsDDLTrigger { get; set; }

    public bool InsteadOf { get; set; }

    public bool IsDisabled { get; set; }

    public bool NotForReplication { get; set; }

    public override bool IsCodeType => true;

    public override string ToSqlDrop() => !IsDDLTrigger ? $"DROP TRIGGER {FullName}\r\nGO\r\n" : $"DROP TRIGGER {FullName} ON DATABASE\r\nGO\r\n";

    public string ToSQLEnabledDisabled()
    {
        return !IsDDLTrigger
            ? IsDisabled
                ? $"DISABLE TRIGGER [{Name}] ON {Parent.FullName}\r\nGO\r\n"
                : $"ENABLE TRIGGER [{Name}] ON {Parent.FullName}\r\nGO\r\n"
            : IsDisabled ? $"DISABLE TRIGGER [{Name}]\r\nGO\r\n" : $"ENABLE TRIGGER [{Name}]\r\nGO\r\n";
    }

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status == ObjectStatus.Drop)
        {
            list.Add(Drop());
        }

        if (Status == ObjectStatus.Create)
        {
            list.Add(Create());
        }

        if (HasState(ObjectStatus.Alter))
        {
            list.AddRange(Rebuild());
        }

        if (HasState(ObjectStatus.Disabled))
        {
            list.Add(ToSQLEnabledDisabled(), 0, ScriptAction.EnabledTrigger);
        }

        return list;
    }

    public override bool Compare(ICode obj)
    {
        return obj == null
            ? throw new ArgumentNullException("obj")
            : this.ToSql().Equals(obj.ToSql()) && this.InsteadOf == ((Trigger)obj).InsteadOf && this.IsDisabled == ((Trigger)obj).IsDisabled && this.NotForReplication == ((Trigger)obj).NotForReplication;
    }
}
