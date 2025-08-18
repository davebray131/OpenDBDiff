using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class FullTextIndexSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2005 => Get2005(),
        _ => Get2008(),
    };

    private static string Get2005() =>
        """
        SELECT 
          FI.object_id, T.Name AS TableName, FC.name AS FullTextCatalogName, I.name AS IndexName, 
          FI.is_enabled, '['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, 
          C.name as ColumnName, FI.change_tracking_state_desc AS ChangeTracking, FL.name AS LanguageName 
        FROM sys.fulltext_indexes FI 
        INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  
        INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  
        INNER JOIN sys.tables T ON T.object_id = FI.object_id 
        INNER JOIN sys.schemas S ON S.schema_id = T.schema_id 
        INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id 
        INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id 
        INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id 
        ORDER BY OBJECT_NAME(FI.object_id), I.name  
        """;

    private static string Get2008() =>
        """
        SELECT 
          FI.object_id, T.Name AS TableName, FC.name AS FullTextCatalogName, I.name AS IndexName, FI.is_enabled, 
         '['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, C.name as ColumnName, FL.name AS LanguageName,
         DS.name AS FileGroupName, FI.change_tracking_state_desc AS ChangeTracking 
        FROM sys.fulltext_indexes FI 
        INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  
        INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  
        INNER JOIN sys.tables T ON T.object_id = FI.object_id 
        INNER JOIN sys.schemas S ON S.schema_id = T.schema_id 
        INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id 
        INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id 
        INNER JOIN sys.data_spaces DS ON DS.data_space_id = FI.data_space_id 
        INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id 
        ORDER BY OBJECT_NAME(FI.object_id), I.name  
        """;
}
