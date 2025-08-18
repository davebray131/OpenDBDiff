using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class IndexSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2005 => Get2005(),
        DatabaseInfo.SQLServerVersion.SQLServer2008 or DatabaseInfo.SQLServerVersion.SQLServer2008R2 => Get2008(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetAzure(),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetAzure() : Get2008(),
    };

    private static string Get2005() =>
       $"""
        SELECT 
          OO.type AS ObjectType, IC.key_ordinal, C.user_type_id, I.object_id, dsidx.Name as FileGroup, C.column_id,C.Name AS ColumnName, I.Name, 
          I.index_id, I.type, is_unique, ignore_dup_key, is_primary_key, is_unique_constraint, fill_factor, is_padded, is_disabled, allow_row_locks, 
          allow_page_locks, IC.is_descending_key, IC.is_included_column, ISNULL(ST.no_recompute,0) AS NoAutomaticRecomputation
        FROM sys.indexes I
        INNER JOIN sys.objects OO ON OO.object_id = I.object_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.stats AS ST ON ST.stats_id = I.index_id AND ST.object_id = I.object_id
        WHERE I.type IN (1,2,3)
        AND is_unique_constraint = 0 AND is_primary_key = 0
        AND objectproperty(I.object_id, 'IsMSShipped') <> 1
        ORDER BY I.object_id, I.Name, IC.column_id
        """;

    private static string Get2008() =>
        """
        SELECT 
          ISNULL(I.filter_definition,'') AS FilterDefinition, OO.type AS ObjectType, IC.key_ordinal, C.user_type_id, I.object_id, dsidx.Name as FileGroup, C.column_id,
          C.Name AS ColumnName, I.Name, I.index_id, I.type, is_unique, ignore_dup_key, is_primary_key, is_unique_constraint, fill_factor, is_padded, is_disabled, 
          allow_row_locks, allow_page_locks, IC.is_descending_key, IC.is_included_column, ISNULL(ST.no_recompute,0) AS NoAutomaticRecomputation
        FROM sys.indexes I
        INNER JOIN sys.objects OO ON OO.object_id = I.object_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.stats AS ST ON ST.stats_id = I.index_id AND ST.object_id = I.object_id
        WHERE I.type IN (1,2,3)
        AND is_unique_constraint = 0 AND is_primary_key = 0
        AND objectproperty(I.object_id, 'IsMSShipped') <> 1
        ORDER BY I.object_id, I.Name, IC.column_id
        """;

    private static string GetAzure() =>
        """
        SELECT 
          ISNULL(I.filter_definition,'') AS FilterDefinition, OO.type AS ObjectType, IC.key_ordinal, C.user_type_id, I.object_id, '' as FileGroup, C.column_id,
          C.Name AS ColumnName, I.Name, I.index_id, I.type, is_unique, ignore_dup_key, is_primary_key, is_unique_constraint, fill_factor, is_padded, is_disabled, 
          allow_row_locks, allow_page_locks, IC.is_descending_key, IC.is_included_column, ISNULL(ST.no_recompute,0) AS NoAutomaticRecomputation
        FROM sys.indexes I
        INNER JOIN sys.objects OO ON OO.object_id = I.object_id
        INNER JOIN sys.index_columns IC ON IC.index_id = I.index_id AND IC.object_id = I.object_id
        INNER JOIN sys.columns C ON C.column_id = IC.column_id AND IC.object_id = C.object_id
        LEFT JOIN sys.stats AS ST ON ST.stats_id = I.index_id AND ST.object_id = I.object_id
        WHERE I.type IN (1,2,3)
        AND is_unique_constraint = 0 AND is_primary_key = 0
        AND objectproperty(I.object_id, 'IsMSShipped') <> 1
        ORDER BY I.object_id, I.Name, IC.column_id
        """;

    //INNER JOIN sys.data_spaces AS dsidx ON dsidx.data_space_id = I.data_space_id
}
