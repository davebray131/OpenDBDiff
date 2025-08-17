using System;
using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class FullTextIndex(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.FullTextIndex)
{
    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var index = new FullTextIndex(parent)
        {
            ChangeTrackingState = ChangeTrackingState,
            FullText = FullText,
            Name = Name,
            FileGroup = FileGroup,
            Id = Id,
            Index = Index,
            IsDisabled = IsDisabled,
            Status = Status,
            Owner = Owner,
            Columns = Columns
        };
        ExtendedProperties.ForEach(index.ExtendedProperties.Add);
        return index;
    }

    public string FileGroup { get; set; }

    public bool IsDisabled { get; set; }

    public string Index { get; set; }

    public string FullText { get; set; }

    public string ChangeTrackingState { get; set; }

    public override string FullName => Name;

    public List<FullTextIndexColumn> Columns { get; set; } = [];

    public override SQLScript Create()
    {
        var action = ScriptAction.AddFullTextIndex;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlAdd(), Parent.DependenciesCount, action);
        }
        else
        {
            return null;
        }
    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropFullTextIndex;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlDrop(), Parent.DependenciesCount, action);
        }
        else
        {
            return null;
        }
    }

    public override string ToSqlAdd()
    {
        var sql = $"CREATE FULLTEXT INDEX ON {Parent.FullName}( ";
        Columns.ForEach(item => { sql += $"[{item.ColumnName}] LANGUAGE [{item.Language}],"; });
        sql = sql.Substring(0, sql.Length - 1);
        sql += ")\r\n";
        if (((Database)RootParent).Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
        {
            sql += $"KEY INDEX {Index} ON ([{FullText}]";
            sql += $", FILEGROUP [{FileGroup}]";
            sql += $") WITH (CHANGE_TRACKING {ChangeTrackingState})";
        }
        else
        {
            sql += $"KEY INDEX {Index} ON [{FullText}]";
            sql += $" WITH CHANGE_TRACKING {ChangeTrackingState}";
        }
        sql += "\r\nGO\r\n";
        if (!IsDisabled)
        {
            sql += $"ALTER FULLTEXT INDEX ON {Parent.FullName} ENABLE\r\nGO\r\n";
        }

        return sql;
    }

    public string ToSqlEnabled() => $"ALTER FULLTEXT INDEX ON {Parent.FullName} {OnOff(IsDisabled, "DISABLE", "ENABLE")}\r\nGO\r\n";

    public override string ToSqlDrop() => $"DROP FULLTEXT INDEX ON {Parent.FullName}\r\nGO\r\n";

    public override string ToSql() => ToSqlAdd();

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status != ObjectStatus.Original)
        {
            RootParent.ActionMessage[Parent.FullName].Add(this);
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
            list.Add(Drop());
            list.Add(Create());
        }
        if (Status == ObjectStatus.Disabled)
        {
            list.Add(ToSqlEnabled(), Parent.DependenciesCount, ScriptAction.AlterFullTextIndex);
        }
        /*if (this.Status == StatusEnum.ObjectStatusType.ChangeFileGroup)
        {
            listDiff.Add(this.ToSQLDrop(this.FileGroup), ((Table)Parent).DependenciesCount, StatusEnum.ScripActionType.DropIndex);
            listDiff.Add(this.ToSQLAdd(), ((Table)Parent).DependenciesCount, StatusEnum.ScripActionType.AddIndex);
        }*/
        list.AddRange(ExtendedProperties.ToSqlDiff());
        return list;
    }

    public bool Compare(FullTextIndex destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : ChangeTrackingState.Equals(destination.ChangeTrackingState) && FullText.Equals(destination.FullText) && Index.Equals(destination.Index) && IsDisabled == destination.IsDisabled && Columns.Count == destination.Columns.Count && !Columns.Exists(item => { return !destination.Columns.Exists(item2 => item2.ColumnName.Equals(item.ColumnName)); }) && !destination.Columns.Exists(item => { return !Columns.Exists(item2 => item2.ColumnName.Equals(item.ColumnName)); });
}
