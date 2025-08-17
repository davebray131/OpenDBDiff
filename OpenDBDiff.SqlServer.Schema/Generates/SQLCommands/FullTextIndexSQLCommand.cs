using System.Text;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class FullTextIndexSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version)
    {
        switch (version)
        {
            case DatabaseInfo.SQLServerVersion.SQLServer2005:
                return Get2005();

            default:
                return Get2008();
        }
    }

    private static string Get2005()
    {
        var sql = new StringBuilder();
        _ = sql.Append("SELECT ");
        _ = sql.Append("FI.object_id, ");
        _ = sql.Append("T.Name AS TableName,  ");
        _ = sql.Append("FC.name AS FullTextCatalogName, ");
        _ = sql.Append("I.name AS IndexName, ");
        _ = sql.Append("FI.is_enabled, ");
        _ = sql.Append("'['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, ");
        _ = sql.Append("C.name as ColumnName, ");
        _ = sql.Append("FI.change_tracking_state_desc AS ChangeTracking, ");
        _ = sql.Append("FL.name AS LanguageName ");
        _ = sql.Append("FROM sys.fulltext_indexes FI ");
        _ = sql.Append("INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  ");
        _ = sql.Append("INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  ");
        _ = sql.Append("INNER JOIN sys.tables T ON T.object_id = FI.object_id ");
        _ = sql.Append("INNER JOIN sys.schemas S ON S.schema_id = T.schema_id ");
        _ = sql.Append("INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id ");
        _ = sql.Append("INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id ");
        _ = sql.Append("INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id ");
        _ = sql.Append("ORDER BY OBJECT_NAME(FI.object_id), I.name  ");
        return sql.ToString();
    }

    private static string Get2008()
    {
        var sql = new StringBuilder();
        _ = sql.Append("SELECT ");
        _ = sql.Append("FI.object_id, ");
        _ = sql.Append("T.Name AS TableName,  ");
        _ = sql.Append("FC.name AS FullTextCatalogName, ");
        _ = sql.Append("I.name AS IndexName, ");
        _ = sql.Append("FI.is_enabled, ");
        _ = sql.Append("'['+ S.name + '].['+ T.name + '].[' + FC.name + ']' AS Name, ");
        _ = sql.Append("C.name as ColumnName, ");
        _ = sql.Append("FL.name AS LanguageName,");
        _ = sql.Append("DS.name AS FileGroupName, ");
        _ = sql.Append("FI.change_tracking_state_desc AS ChangeTracking ");
        _ = sql.Append("FROM sys.fulltext_indexes FI ");
        _ = sql.Append("INNER JOIN sys.fulltext_catalogs FC ON FC.fulltext_catalog_id = FI.fulltext_catalog_id  ");
        _ = sql.Append("INNER JOIN sys.indexes I ON I.index_id = FI.unique_index_id and I.object_id = FI.object_id  ");
        _ = sql.Append("INNER JOIN sys.tables T ON T.object_id = FI.object_id ");
        _ = sql.Append("INNER JOIN sys.schemas S ON S.schema_id = T.schema_id ");
        _ = sql.Append("INNER JOIN sys.fulltext_index_columns FIC ON FIC.object_id = FI.object_id ");
        _ = sql.Append("INNER JOIN sys.columns C ON C.object_id = FIC.object_id AND C.column_id = FIC.column_id ");
        _ = sql.Append("INNER JOIN sys.data_spaces DS ON DS.data_space_id = FI.data_space_id ");
        _ = sql.Append("INNER JOIN sys.fulltext_languages FL ON FL.lcid = FIC.language_id ");
        _ = sql.Append("ORDER BY OBJECT_NAME(FI.object_id), I.name  ");
        return sql.ToString();
    }
}
