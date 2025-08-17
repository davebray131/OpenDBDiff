using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Constraint : SQLServerSchemaBase
{
    public enum ConstraintType
    {
        None = 0,
        PrimaryKey = 1,
        ForeignKey = 2,
        Default = 3,
        Unique = 4,
        Check = 5
    }

    public Constraint(ISchemaBase parent)
        : this(parent, false)
    {
    }

    public Constraint(ISchemaBase parent, bool hasIndex)
        : base(parent, ObjectType.Constraint)
    {
        Columns = new ConstraintColumns(this);
        if (hasIndex)
        {
            Index = new Index(parent);
        }
    }

    /// <summary>
    /// Clona el objeto Column en una nueva instancia.
    /// </summary>
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var col = new Constraint(parent)
        {
            Id = Id,
            Name = Name,
            NotForReplication = NotForReplication,
            RelationalTableFullName = RelationalTableFullName,
            Status = Status,
            Type = Type,
            WithNoCheck = WithNoCheck,
            OnDeleteCascade = OnDeleteCascade,
            OnUpdateCascade = OnUpdateCascade,
            Owner = Owner,
            Columns = Columns.Clone(),
            Index = (Index)Index?.Clone(parent),
            IsDisabled = IsDisabled,
            Definition = Definition,
            Guid = Guid
        };
        return col;
    }

    /// <summary>
    /// Informacion sobre le indice asociado al Constraint.
    /// </summary>
    public Index Index { get; set; }

    /// <summary>
    /// Coleccion de columnas de la constraint.
    /// </summary>
    public ConstraintColumns Columns { get; set; }

    /// <summary>
    /// Indica si la constraint tiene asociada un indice Clustered.
    /// </summary>
    public bool HasClusteredIndex => Index != null && Index.Type == Index.IndexTypeEnum.Clustered;

    /// <summary>
    /// Gets or sets a value indicating whether this constraint is disabled.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this constraint is disabled; otherwise, <c>false</c>.
    /// </value>
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Gets or sets the on delete cascade (only for FK).
    /// </summary>
    /// <value>The on delete cascade.</value>
    public int OnDeleteCascade { get; set; }

    /// <summary>
    /// Gets or sets the on update cascade (only for FK).
    /// </summary>
    /// <value>The on update cascade.</value>
    public int OnUpdateCascade { get; set; }

    /// <summary>
    /// Valor de la constraint (se usa para los Check Constraint).
    /// </summary>
    public string Definition { get; set; }

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
    /// Indica el tipo de constraint (PrimaryKey, ForeignKey, Unique o Default).
    /// </summary>
    public ConstraintType Type { get; set; }

    /// <summary>
    /// ID de la tabla relacionada a la que hace referencia (solo aplica a FK)
    /// </summary>
    public int RelationalTableId { get; set; }

    /// <summary>
    /// Nombre de la tabla relacionada a la que hace referencia (solo aplica a FK)
    /// </summary>
    public string RelationalTableFullName { get; set; }

    /// <summary>
    /// Compara dos campos y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(Constraint origin, Constraint destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (origin.NotForReplication != destination.NotForReplication)
        {
            return false;
        }

        if ((origin.RelationalTableFullName == null) && (destination.RelationalTableFullName != null))
        {
            return false;
        }

        if (origin.RelationalTableFullName != null)
        {
            if (!origin.RelationalTableFullName.Equals(destination.RelationalTableFullName, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
        }

        if ((origin.Definition == null) && (destination.Definition != null))
        {
            return false;
        }

        if (origin.Definition != null)
        {
            if ((!origin.Definition.Equals(destination.Definition)) && (!origin.Definition.Equals("(" + destination.Definition + ")")))
            {
                return false;
            }
        }
        /*Solo si la constraint esta habilitada, se chequea el is_trusted*/
        if (!destination.IsDisabled)
        {
            if (origin.WithNoCheck != destination.WithNoCheck)
            {
                return false;
            }
        }

        return origin.OnUpdateCascade == destination.OnUpdateCascade && origin.OnDeleteCascade == destination.OnDeleteCascade && ConstraintColumns.Compare(origin.Columns, destination.Columns) && (origin.Index == null || destination.Index == null || Index.Compare(origin.Index, destination.Index));
    }

    private string ToSQLGeneric(ConstraintType consType)
    {
        Database database = null;
        ISchemaBase current = this;
        while (database == null && current.Parent != null)
        {
            database = current.Parent as Database;
            current = current.Parent;
        }
        var isAzure10 = database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10;
        //string typeConstraint = "";
        var sql = new StringBuilder();
        _ = Parent.ObjectType != ObjectType.TableType ? sql.Append($"CONSTRAINT [{Name}] ") : sql.Append("\t");

        _ = consType == ConstraintType.PrimaryKey ? sql.Append("PRIMARY KEY") : sql.Append("UNIQUE");

        if (Index != null)
        {
            sql.Append(" ");
            sql.Append(Index.Type.ToString().ToUpperInvariant());
        }

        sql.Append("\r\n\t(\r\n");

        Columns.Sort();

        for (var j = 0; j < Columns.Count; j++)
        {
            sql.Append("\t\t[" + Columns[j].Name + "]");
            _ = Columns[j].Order ? sql.Append(" DESC") : sql.Append(" ASC");

            if (j != Columns.Count - 1)
            {
                sql.Append(",");
            }

            sql.AppendLine();
        }
        sql.Append("\t)");

        if (Index != null)
        {
            List<string> withList = [];

            if (Parent.ObjectType == ObjectType.TableType)
            {
                withList.Add($"IGNORE_DUP_KEY = {OnOff(Index.IgnoreDupKey)}");
            }
            else
            {
                if (!isAzure10)
                {
                    withList.Add($"PAD_INDEX = {OnOff(Index.IsPadded)}");
                }
                withList.Add($"STATISTICS_NORECOMPUTE = {OnOff(Index.IsAutoStatistics)}");
                withList.Add($"IGNORE_DUP_KEY = {OnOff(Index.IgnoreDupKey)}");

                if (!isAzure10)
                {
                    withList.Add($"ALLOW_ROW_LOCKS = {OnOff(Index.AllowRowLocks)}");
                    withList.Add($"ALLOW_PAGE_LOCKS = {OnOff(Index.AllowPageLocks)}");

                    if (Index.FillFactor != 0)
                    {
                        withList.Add($", FILLFACTOR = {Index.FillFactor.ToString(CultureInfo.InvariantCulture)}");
                    }
                }
            }

            if (withList.Count > 0)
            {
                sql.Append($" WITH ({string.Join(", ", withList)})");
            }

            if (!isAzure10)
            {
                if (!string.IsNullOrEmpty(Index.FileGroup))
                {
                    sql.Append(" ON [" + Index.FileGroup + "]");
                }
            }
        }

        return sql.ToString();
    }

    /// <summary>
    /// Devuelve el schema de la tabla en formato SQL.
    /// </summary>
    public override string ToSql()
    {
        if (Type == ConstraintType.PrimaryKey)
        {
            return ToSQLGeneric(ConstraintType.PrimaryKey);
        }
        if (Type == ConstraintType.ForeignKey)
        {
            var sql = new StringBuilder();
            var sqlReference = new StringBuilder();
            var indexc = 0;

            Columns.Sort();
            sql.Append("CONSTRAINT [" + Name + "] FOREIGN KEY\r\n\t(\r\n");
            foreach (var column in Columns)
            {
                sql.Append($"\t\t[{column.Name}]");
                _ = sqlReference.Append($"\t\t[{column.ColumnRelationalName}]");
                if (indexc != Columns.Count - 1)
                {
                    sql.Append(",");
                    _ = sqlReference.Append(",");
                }
                sql.AppendLine();
                _ = sqlReference.AppendLine();
                indexc++;
            }
            sql.Append("\t)\r\n");
            sql.Append($"\tREFERENCES {RelationalTableFullName}\r\n\t(\r\n");
            sql.Append(sqlReference + "\t)");

            if (OnUpdateCascade == 1)
            {
                sql.Append(" ON UPDATE CASCADE");
            }

            if (OnDeleteCascade == 1)
            {
                sql.Append(" ON DELETE CASCADE");
            }

            if (OnUpdateCascade == 2)
            {
                sql.Append(" ON UPDATE SET NULL");
            }

            if (OnDeleteCascade == 2)
            {
                sql.Append(" ON DELETE SET NULL");
            }

            if (OnUpdateCascade == 3)
            {
                sql.Append(" ON UPDATE SET DEFAULT");
            }

            if (OnDeleteCascade == 3)
            {
                sql.Append(" ON DELETE SET DEFAULT");
            }

            sql.Append(NotForReplication ? " NOT FOR REPLICATION" : "");
            return sql.ToString();
        }
        if (Type == ConstraintType.Unique)
        {
            return ToSQLGeneric(ConstraintType.Unique);
        }
        if (Type == ConstraintType.Check)
        {
            var sqlcheck = "";
            if (Parent.ObjectType != ObjectType.TableType)
            {
                sqlcheck = $"CONSTRAINT [{Name}] ";
            }

            return sqlcheck + "CHECK " + (NotForReplication ? "NOT FOR REPLICATION" : "") + " (" + Definition + ")";
        }
        return string.Empty;
    }

    public override string ToSqlAdd() => $"ALTER TABLE {Parent.FullName}" + (WithNoCheck ? " WITH NOCHECK" : "") + " ADD " + ToSql() + "\r\nGO\r\n";

    public override string ToSqlDrop() => ToSqlDrop(null);

    public override SQLScript Create()
    {
        var action = ScriptAction.AddConstraint;
        if (Type == ConstraintType.ForeignKey)
        {
            action = ScriptAction.AddConstraintFK;
        }

        if (Type == ConstraintType.PrimaryKey)
        {
            action = ScriptAction.AddConstraintPK;
        }

        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlAdd(), ((Table)Parent).DependenciesCount, action);
        }
        else
        {
            return null;
        }
    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropConstraint;
        if (Type == ConstraintType.ForeignKey)
        {
            action = ScriptAction.DropConstraintFK;
        }

        if (Type == ConstraintType.PrimaryKey)
        {
            action = ScriptAction.DropConstraintPK;
        }

        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlDrop(), ((Table)Parent).DependenciesCount, action);
        }
        else
        {
            return null;
        }
    }

    public string ToSqlDrop(string FileGroupName)
    {
        var sql = $"ALTER TABLE {((Table)Parent).FullName} DROP CONSTRAINT [{Name}]";
        if (!string.IsNullOrEmpty(FileGroupName))
        {
            sql += $" WITH (MOVE TO [{FileGroupName}])";
        }

        sql += "\r\nGO\r\n";
        return sql;
    }

    public string ToSQLEnabledDisabled() => IsDisabled
            ? $"ALTER TABLE {Parent.FullName} NOCHECK CONSTRAINT [{Name}\r\nGO\r\n"
            : $"ALTER TABLE {Parent.FullName} CHECK CONSTRAINT [{Name}]\r\nGO\r\n";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status != ObjectStatus.Original)
        {
            RootParent.ActionMessage[Parent.FullName].Add(this);
        }

        if (HasState(ObjectStatus.Drop))
        {
            if (Parent.Status != ObjectStatus.Rebuild)
            {
                list.Add(Drop());
            }
        }
        if (HasState(ObjectStatus.Create))
        {
            list.Add(Create());
        }

        if (HasState(ObjectStatus.Alter))
        {
            list.Add(Drop());
            list.Add(Create());
        }
        if (HasState(ObjectStatus.Disabled))
        {
            list.Add(ToSQLEnabledDisabled(), ((Table)Parent).DependenciesCount, ScriptAction.AlterConstraint);
        }
        /*if (this.Status == StatusEnum.ObjectStatusType.ChangeFileGroup)
        {
            list.Add(this.ToSQLDrop(this.Index.FileGroup), ((Table)Parent).DependenciesCount, actionDrop);
            list.Add(this.ToSQLAdd(), ((Table)Parent).DependenciesCount, actionAdd);
        }*/
        return list;
    }
}
