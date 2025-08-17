using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class ViewSQLCommand
{
    #region View

    public static string GetView(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2000 or DatabaseInfo.SQLServerVersion.SQLServer2005 or DatabaseInfo.SQLServerVersion.SQLServer2008 or DatabaseInfo.SQLServerVersion.SQLServer2008R2 => GetViewSql2008(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetViewSqlAzure(),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetViewSqlAzure() : GetViewSql2008(),
    };

    private static string GetViewSql2008() =>
        """
        SELECT DISTINCT
          ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, 
          D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.views P
        INNER JOIN sys.schemas S ON S.schema_id = P.schema_id
        LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id
        LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id
        LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id
        LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id
        LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id
        ORDER BY P.object_id
        """;
    private static string GetViewSqlAzure() =>
        //Avoid using sql_dependencies. Use sys.sql_expression_dependencies instead. http://msdn.microsoft.com/en-us/library/ms174402.aspx
        """
        SELECT DISTINCT 
          ISNULL('[' + S3.Name + '].[' + object_name(D2.referencing_id) + ']','') AS DependOut,
          '[' + S2.Name + '].[' + object_name(D.referenced_id) + ']' AS TableName,
          D.referenced_id AS referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound,
          P.object_id, S.name as owner, P.name as name
        FROM sys.views P
        INNER JOIN sys.schemas S ON S.schema_id = P.schema_id
        LEFT JOIN sys.sql_expression_dependencies D ON P.object_id = D.referencing_id
        LEFT JOIN sys.objects O ON O.object_id = D.referenced_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id
        LEFT JOIN sys.sql_expression_dependencies D2 ON P.object_id = D2.referenced_id
        LEFT JOIN sys.objects O2 ON O2.object_id = D2.referencing_id
        LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id
        ORDER BY P.object_id
        """;

    #endregion View
}
