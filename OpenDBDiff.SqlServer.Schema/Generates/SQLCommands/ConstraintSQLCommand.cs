using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class ConstraintSQLCommand
{
    public static string GetUniqueKey(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2005 => GetUniqueKey2005(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetUniqueKeyAzure(),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetUniqueKeyAzure() : GetUniqueKey2008(),
    };

    public static string GetCheck(DatabaseInfo.SQLServerVersion version)
    {
        if (version == DatabaseInfo.SQLServerVersion.SQLServer2005)
        {
            return GetCheck2005();
        }
        //Fall back to highest compatible version
        return GetCheck2008();
    }

    public static string GetPrimaryKey(DatabaseInfo.SQLServerVersion version, Table table) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2000 => GetPrimaryKey2000(table),
        DatabaseInfo.SQLServerVersion.SQLServer2005 => GetPrimaryKey2005(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetPrimaryKeyAzure(),
        _ => GetPrimaryKey2008(),
    };

    private static string GetUniqueKeyAzure() =>
        """
        SELECT O.type as ObjectType, S.Name as Owner, I.object_Id AS id,'' as FileGroup, C.user_type_id, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        "WHERE is_unique_constraint = 1 AND O.type <> 'TF' ORDER BY I.object_id,I.Name
        """;

    //sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");//sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");

    private static string GetUniqueKey2008() =>
        """
        SELECT 
           O.type as ObjectType, S.Name as Owner, I.object_Id AS id,dsidx.Name as FileGroup, C.user_type_id, C.column_id, I.Index_id, 
           C.Name AS ColumnName, I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        WHERE is_unique_constraint = 1 AND O.type <> 'TF' ORDER BY I.object_id,I.Name
        """;

    private static string GetUniqueKey2005() =>
        """
        SELECT 
           O.type as ObjectType, S.Name as Owner, I.object_Id AS id,dsidx.Name as FileGroup, C.user_type_id, C.column_id, I.Index_id, C.Name AS ColumnName, 
           I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        WHERE is_unique_constraint = 1 AND O.type <> 'TF' ORDER BY I.object_id,I.Name
        """;

    private static string GetCheck2008() =>
        """
        SELECT
        CC.parent_object_id,
        O.type as ObjectType,
        CC.object_id AS ID,
        CC.parent_column_id,
        CC.name,
        CC.type,
        CC.definition,
        CC.is_disabled,
        CC.is_not_trusted AS WithCheck,
        CC.is_not_for_replication,
        0,
        schema_name(CC.schema_id) AS Owner
        FROM sys.check_constraints CC
        INNER JOIN sys.objects O ON O.object_id = CC.parent_object_id
        ORDER BY CC.parent_object_id,CC.name
        """;

    private static string GetCheck2005() =>
        """
        SELECT 
        CC.parent_object_id,
        O.Type as ObjectType,
        CC.object_id AS ID,
        CC.parent_column_id,
        CC.name,
        CC.type,
        CC.definition,
        CC.is_disabled,
        CC.is_not_trusted AS WithCheck,
        CC.is_not_for_replication,
        0,
        schema_name(CC.schema_id) AS Owner
        FROM sys.check_constraints CC
        INNER JOIN sys.objects O ON O.object_id = CC.parent_object_id
        ORDER BY CC.parent_object_id,CC.name
        """;

    private static string GetPrimaryKeyAzure() =>
        """
        SELECT 
          O.type as ObjectType, S.Name as Owner, IC.key_ordinal, C.user_type_id, I.object_id AS ID, '' AS FileGroup, C.column_id, I.Index_id, C.Name AS ColumnName, I.Name, 
          I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column, 
          CONVERT(bit,INDEXPROPERTY(I.object_id,I.name,'IsAutoStatistics')) AS IsAutoStatistics ");
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        WHERE is_primary_key = 1 AND O.type <> 'TF' ORDER BY I.object_id, IC.key_ordinal
        """;
    //sql.Append("LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id ");

    private static string GetPrimaryKey2008() =>
        """
        SELECT 
           O.type as ObjectType, S.Name as Owner, IC.key_ordinal, C.user_type_id, I.object_id AS ID, dsidx.Name AS FileGroup, C.column_id, I.Index_id, C.Name AS ColumnName, 
           I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column, CONVERT(bit,INDEXPROPERTY(I.object_id,I.name,'IsAutoStatistics')) AS IsAutoStatistics
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        WHERE is_primary_key = 1 AND O.type <> 'TF' ORDER BY I.object_id, IC.key_ordinal
        """;

    private static string GetPrimaryKey2005() =>
        """
        "SELECT 
           O.type as ObjectType, S.Name as Owner, IC.key_ordinal, C.user_type_id, I.object_id AS ID, dsidx.Name AS FileGroup, C.column_id, I.Index_id, C.Name AS ColumnName, 
           I.Name, I.type, I.fill_factor, I.is_padded, I.allow_row_locks, I.allow_page_locks, I.ignore_dup_key, I.is_disabled, IC.is_descending_key, IC.is_included_column, CONVERT(bit,INDEXPROPERTY(I.object_id,I.name,'IsAutoStatistics')) AS IsAutoStatistics
        FROM sys.indexes I
        INNER JOIN sys.objects O ON O.object_id = I.object_id
        INNER JOIN sys.schemas S ON S.schema_id = O.schema_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        WHERE is_primary_key = 1 AND O.type <> 'TF' ORDER BY I.object_id, IC.key_ordinal
        """;

    private static string GetPrimaryKey2000(Table table) =>
        $"""
        SELECT 
            CONVERT(tinyint, CASE WHEN SI.indid = 0 THEN 0 WHEN SI.indid = 1 THEN 1 WHEN SI.indid > 1 THEN 2 END) AS Type, f.groupname AS FileGroup,
            CONVERT(int, SI.indid) AS Index_id, CONVERT(int, SI.indid) AS ID, SI.name, SC.colid, SC.Name AS ColumnName, CONVERT(bit, 0) AS is_included_column, SIK.keyno AS key_ordinal, 
            CONVERT(bit, INDEXPROPERTY(SI.id, SI.name, 'IsPadIndex')) AS is_padded, CONVERT(bit, INDEXPROPERTY(SI.id, SI.name, 'IsRowLockDisallowed')) AS allow_row_locks, 
            CONVERT(bit, INDEXPROPERTY(SI.id, SI.name, 'IsPageLockDisallowed')) AS allow_page_locks, CONVERT(bit, INDEXPROPERTY(SI.id, SI.name, 'IsAutoStatistics')) AS IsAutoStatistics, 
            CONVERT(tinyint, INDEXPROPERTY(SI.id, SI.name, 'IndexFillFactor')) AS fill_factor, INDEXKEY_PROPERTY(SI.id, SI.indid, SC.colid, 'IsDescending') AS is_descending_key, 
            CONVERT(bit, 0) AS is_disabled, CONVERT(bit, 0) AS is_included_column
        FROM sysindexes SI INNER JOIN sysindexkeys SIK ON SI.indid = SIK.indid AND SIK.id = SI.ID
        INNER JOIN syscolumns SC ON SC.colid = SIK.colid AND SC.id = SI.ID
        inner join sysfilegroups f on f.groupid = SI.groupid
        WHERE(SI.status & 0x800) = 0x800 AND SI.id = {table.Id} ORDER BY SIK.keyno
        """;
}
