using System;
using System.Collections.Generic;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Index(ISchemaBase parent) : SQLServerSchemaBase(parent, ObjectType.Index)
{
    public enum IndexTypeEnum
    {
        Heap = 0,
        Clustered = 1,
        Nonclustered = 2,
        XML = 3,
        GEO = 4
    }

    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var index = new Index(parent)
        {
            AllowPageLocks = AllowPageLocks,
            AllowRowLocks = AllowRowLocks,
            Columns = Columns.Clone(),
            FillFactor = FillFactor,
            FileGroup = FileGroup,
            Id = Id,
            IgnoreDupKey = IgnoreDupKey,
            IsAutoStatistics = IsAutoStatistics,
            IsDisabled = IsDisabled,
            IsPadded = IsPadded,
            IsPrimaryKey = IsPrimaryKey,
            IsUniqueKey = IsUniqueKey,
            Name = Name,
            SortInTempDb = SortInTempDb,
            Status = Status,
            Type = Type,
            Owner = Owner,
            FilterDefintion = FilterDefintion
        };
        ExtendedProperties.ForEach(index.ExtendedProperties.Add);
        return index;
    }

    public string FileGroup { get; set; }

    public bool SortInTempDb { get; set; }

    public string FilterDefintion { get; set; } = "";

    public IndexColumns Columns { get; set; } = new IndexColumns(parent);

    public bool IsAutoStatistics { get; set; }

    public bool IsUniqueKey { get; set; }

    public bool IsPrimaryKey { get; set; }

    public IndexTypeEnum Type { get; set; }

    public short FillFactor { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsPadded { get; set; }

    public bool IgnoreDupKey { get; set; }

    public bool AllowPageLocks { get; set; }

    public bool AllowRowLocks { get; set; }

    public override string FullName => $"{Parent.FullName}.[{Name}]";

    /// <summary>
    /// Compara dos indices y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool Compare(Index origin, Index destination) => destination == null
            ? throw new ArgumentNullException(nameof(destination))
            : origin == null
            ? throw new ArgumentNullException(nameof(origin))
            : origin.AllowPageLocks == destination.AllowPageLocks && origin.AllowRowLocks == destination.AllowRowLocks && origin.FillFactor == destination.FillFactor && origin.IgnoreDupKey == destination.IgnoreDupKey && origin.IsAutoStatistics == destination.IsAutoStatistics && origin.IsDisabled == destination.IsDisabled && origin.IsPadded == destination.IsPadded && origin.IsPrimaryKey == destination.IsPrimaryKey && origin.IsUniqueKey == destination.IsUniqueKey && origin.Type == destination.Type && origin.SortInTempDb == destination.SortInTempDb && origin.FilterDefintion.Equals(destination.FilterDefintion) && IndexColumns.Compare(origin.Columns, destination.Columns) && CompareFileGroup(origin, destination);

    public static bool CompareExceptIsDisabled(Index origin, Index destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (origin.AllowPageLocks != destination.AllowPageLocks)
        {
            return false;
        }

        if (origin.AllowRowLocks != destination.AllowRowLocks)
        {
            return false;
        }

        if (origin.FillFactor != destination.FillFactor)
        {
            return false;
        }

        if (origin.IgnoreDupKey != destination.IgnoreDupKey)
        {
            return false;
        }

        if (origin.IsAutoStatistics != destination.IsAutoStatistics)
        {
            return false;
        }

        if (origin.IsPadded != destination.IsPadded)
        {
            return false;
        }

        if (origin.IsPrimaryKey != destination.IsPrimaryKey)
        {
            return false;
        }

        if (origin.IsUniqueKey != destination.IsUniqueKey)
        {
            return false;
        }

        if (origin.Type != destination.Type)
        {
            return false;
        }

        if (origin.SortInTempDb != destination.SortInTempDb)
        {
            return false;
        }

        if (!origin.FilterDefintion.Equals(destination.FilterDefintion))
        {
            return false;
        }

        if (!IndexColumns.Compare(origin.Columns, destination.Columns))
        {
            return false;
        }
        //return true;
        return CompareFileGroup(origin, destination);
    }

    private static bool CompareFileGroup(Index origin, Index destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (origin.FileGroup != null)
        {
            if (!origin.FileGroup.Equals(destination.FileGroup))
            {
                return false;
            }
        }
        return true;
    }

    public override string ToSql()
    {
        Database database = null;
        ISchemaBase current = this;
        while (database == null && current.Parent != null)
        {
            database = current.Parent as Database;
            current = current.Parent;
        }
        var isAzure10 = database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10;

        var sql = new StringBuilder();
        var includes = "";
        if ((Type == IndexTypeEnum.Clustered) && IsUniqueKey)
        {
            sql.Append("CREATE UNIQUE CLUSTERED ");
        }

        if ((Type == IndexTypeEnum.Clustered) && (!IsUniqueKey))
        {
            sql.Append("CREATE CLUSTERED ");
        }

        if ((Type == IndexTypeEnum.Nonclustered) && IsUniqueKey)
        {
            sql.Append("CREATE UNIQUE NONCLUSTERED ");
        }

        if ((Type == IndexTypeEnum.Nonclustered) && (!IsUniqueKey))
        {
            sql.Append("CREATE NONCLUSTERED ");
        }

        if (Type == IndexTypeEnum.XML)
        {
            sql.Append("CREATE PRIMARY XML ");
        }

        sql.AppendLine($"INDEX [{Name}] ON {Parent.FullName}\r\n(");
        /*Ordena la coleccion de campos del Indice en funcion de la propieda IsIncluded*/
        Columns.Sort();
        for (var j = 0; j < Columns.Count; j++)
        {
            if (!Columns[j].IsIncluded)
            {
                sql.Append("\t[" + Columns[j].Name + "]");
                if (Type != IndexTypeEnum.XML)
                {
                    _ = Columns[j].Order ? sql.Append(" DESC") : sql.Append(" ASC");
                }
                if (j < Columns.Count - 1)
                {
                    sql.Append(",");
                }

                sql.AppendLine();
            }
            else
            {
                if (string.IsNullOrEmpty(includes))
                {
                    includes = ") INCLUDE (";
                }

                includes += $"[{Columns[j].Name}],";
            }
        }
        if (!string.IsNullOrEmpty(includes))
        {
            includes = includes.Substring(0, includes.Length - 1);
        }

        sql.Append(includes);
        sql.Append(")");
        if (!string.IsNullOrEmpty(FilterDefintion))
        {
            sql.AppendLine($"\r\n WHERE {FilterDefintion}");
        }

        List<string> withList = [];

        if (Parent.ObjectType == ObjectType.TableType)
        {
            withList.Add($"IGNORE_DUP_KEY = {OnOff(IgnoreDupKey && IsUniqueKey)}");
        }
        else
        {
            if (!isAzure10)
            {
                withList.Add($"PAD_INDEX = {OnOff(IsPadded)}");
            }

            withList.Add($"STATISTICS_NORECOMPUTE = {OnOff(IsAutoStatistics)}");

            if (Type != IndexTypeEnum.XML)
            {
                withList.Add($"IGNORE_DUP_KEY = {OnOff(IgnoreDupKey && IsUniqueKey)}");
            }

            if (!isAzure10)
            {
                withList.Add($"ALLOW_ROW_LOCKS = {OnOff(AllowRowLocks)}");
                withList.Add($"ALLOW_PAGE_LOCKS = {OnOff(AllowPageLocks)}");

                if (FillFactor != 0)
                {
                    withList.Add($"FILLFACTOR = {FillFactor}");
                }
            }
        }
        if (withList.Count > 0)
        {
            sql.Append($" WITH ({string.Join(", ", withList)})");
        }

        if (!isAzure10)
        {
            if (!string.IsNullOrEmpty(FileGroup))
            {
                sql.Append($" ON [{FileGroup}]");
            }
        }
        sql.AppendLine("\r\nGO");
        if (IsDisabled)
        {
            sql.AppendLine($"ALTER INDEX [{Name}] ON {((Table)Parent).FullName} DISABLE\r\nGO");
        }

        sql.Append(ExtendedProperties.ToSql());
        return sql.ToString();
    }

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => ToSqlDrop(null);

    private string ToSqlDrop(string FileGroupName)
    {
        var sql = new StringBuilder($"DROP INDEX [{Name}] ON {Parent.FullName}");
        if (!string.IsNullOrEmpty(FileGroupName))
        {
            sql.Append($" WITH (MOVE TO [{FileGroupName}])");
        }

        sql.AppendLine("\r\nGO");
        return sql.ToString();
    }

    public override SQLScript Create()
    {
        var action = ScriptAction.AddIndex;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlAdd(), Parent.DependenciesCount, action);
        }
        return null;
    }

    public override SQLScript Drop()
    {
        var action = ScriptAction.DropIndex;
        if (!GetWasInsertInDiffList(action))
        {
            SetWasInsertInDiffList(action);
            return new SQLScript(ToSqlDrop(), Parent.DependenciesCount, action);
        }
        return null;
    }

    private string ToSqlEnabled() => $"ALTER INDEX [{Name}] ON {Parent.FullName} {OnOff(IsDisabled, "DISABLE", "REBUILD")}\r\nGO\r\n";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();
        if (Status != ObjectStatus.Original)
        {
            var actionMessage = RootParent.ActionMessage[Parent.FullName];
            actionMessage?.Add(this);
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
            list.Add(ToSqlEnabled(), Parent.DependenciesCount, ScriptAction.AlterIndex);
        }
        /*if (this.Status == StatusEnum.ObjectStatusType.ChangeFileGroup)
        {
            listDiff.Add(this.ToSQLDrop(this.FileGroup), ((Table)Parent).DependenciesCount, StatusEnum.ScripActionType.DropIndex);
            listDiff.Add(this.ToSQLAdd(), ((Table)Parent).DependenciesCount, StatusEnum.ScripActionType.AddIndex);
        }*/
        list.AddRange(ExtendedProperties.ToSqlDiff());
        return list;
    }
}
