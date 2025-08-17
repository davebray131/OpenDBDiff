using System;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

/// <summary>
/// Clase de constraints de Columnas (Default Constraint y Check Constraint)
/// </summary>
public class ColumnConstraint : SQLServerSchemaBase
{
    public ColumnConstraint(Column parent)
        : base(parent, ObjectType.Constraint)
    {
    }

    /// <summary>
    /// Clona el objeto ColumnConstraint en una nueva instancia.
    /// </summary>
    public ColumnConstraint Clone(Column parent)
    {
        var ccons = new ColumnConstraint(parent)
        {
            Name = this.Name,
            Type = this.Type,
            Definition = this.Definition,
            Status = this.Status,
            Disabled = this.Disabled,
            Owner = this.Owner
        };
        return ccons;
    }

    /// <summary>
    /// Indica si la constraint esta deshabilitada.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Indica si la constraint va a ser usada en replicacion.
    /// </summary>
    public bool NotForReplication { get; set; }


    /// <summary>
    /// Gets or sets a value indicating whether [with no check].
    /// </summary>
    /// <value><c>true</c> if [with no check]; otherwise, <c>false</c>.</value>
    public bool WithNoCheck { get; set; }

    /// <summary>
    /// Valor de la constraint.
    /// </summary>
    public string Definition { get; set; }

    /// <summary>
    /// Indica el tipo de constraint (Default o Check constraint).
    /// </summary>
    public Constraint.ConstraintType Type { get; set; }

    /// <summary>
    /// Convierte el schema de la constraint en XML.
    /// </summary>
    public string ToXML()
    {
        return this.Type == Constraint.ConstraintType.Default
            ? $"<COLUMNCONSTRAINT name=\"{Name}\" type=\"DF\" value=\"{Definition}\"/>\n"
            : this.Type == Constraint.ConstraintType.Check
            ? $"<COLUMNCONSTRAINT name=\"{Name}\" type=\"C\" value=\"{Definition}\" notForReplication=\"" + (NotForReplication ? "1" : "0") + "\"/>\n"
            : string.Empty;
    }

    /// <summary>
    /// Compara dos campos y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(ColumnConstraint origin, ColumnConstraint destination)
    {
        return destination == null
            ? throw new ArgumentNullException("destination")
            : origin == null
            ? throw new ArgumentNullException("origin")
            : origin.NotForReplication == destination.NotForReplication && origin.Disabled == destination.Disabled && (origin.Definition.Equals(destination.Definition) || origin.Definition.Equals("(" + destination.Definition + ")"));
    }

    public override SQLScript Create()
    {
        var action = ScriptAction.AddConstraint;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(this.ToSqlAdd(), 0, action);
        }
        return null;

    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropConstraint;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(this.ToSqlDrop(), 0, action);
        }
        else
        {
            return null;
        }
    }

    public bool CanCreate
    {
        get
        {
            var tableStatus = this.Parent.Parent.Status;
            var columnStatus = this.Parent.Status;
            return (columnStatus != ObjectStatus.Drop) && ((tableStatus == ObjectStatus.Alter) || (tableStatus == ObjectStatus.Original) || (tableStatus == ObjectStatus.RebuildDependencies)) && (this.Status == ObjectStatus.Original);
        }
    }

    /// <summary>
    /// Devuelve el schema de la constraint en formato SQL.
    /// </summary>
    public override string ToSql() => this.Type == Constraint.ConstraintType.Default ? $" CONSTRAINT [{Name}] DEFAULT {Definition}" : string.Empty;

    /// <summary>
    /// Toes the SQL add.
    /// </summary>
    /// <returns></returns>
    public override string ToSqlAdd()
    {
        return this.Type == Constraint.ConstraintType.Default
            ? $"ALTER TABLE {((Table)Parent.Parent).FullName} ADD{ToSql()} FOR [{Parent.Name}]\r\nGO\r\n"
            : this.Type == Constraint.ConstraintType.Check ? $"ALTER TABLE {((Table)Parent.Parent).FullName} ADD{ToSql()}\r\nGO\r\n" : "";
    }

    /// <summary>
    /// Toes the SQL drop.
    /// </summary>
    /// <returns></returns>
    public override string ToSqlDrop() => $"ALTER TABLE {((Table)Parent.Parent).FullName} DROP CONSTRAINT [{Name}]\r\nGO\r\n";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (this.HasState(ObjectStatus.Drop))
        {
            list.Add(Drop());
        }

        if (this.HasState(ObjectStatus.Create))
        {
            list.Add(Create());
        }

        if (this.Status == ObjectStatus.Alter)
        {
            list.Add(Drop());
            list.Add(Create());
        }
        return list;
    }
}
