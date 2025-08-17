using System.Text;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class ViewSQLCommand
{
    #region View

    public static string GetView(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition)
    {
        switch (version)
        {
            case DatabaseInfo.SQLServerVersion.SQLServer2000:
            case DatabaseInfo.SQLServerVersion.SQLServer2005:
            case DatabaseInfo.SQLServerVersion.SQLServer2008:
            case DatabaseInfo.SQLServerVersion.SQLServer2008R2:
                return GetViewSql2008();

            case DatabaseInfo.SQLServerVersion.SQLServerAzure10:
                return GetViewSqlAzure();

            default:
                return edition == DatabaseInfo.SQLServerEdition.Azure ? GetViewSqlAzure() : GetViewSql2008();
        }
    }

    private static string GetViewSql2008()
    {
        var sql = "";
        sql += "select distinct ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.views P ";
        sql += "INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ";
        sql += "LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ";
        sql += "LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ";
        sql += "LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ";
        sql += "LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ";
        sql += "LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ";
        sql += "LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ";
        sql += "ORDER BY P.object_id";
        return sql;
    }

    private static string GetViewSqlAzure()
    {
        var sql = new StringBuilder();
        //Avoid using sql_dependencies. Use sys.sql_expression_dependencies instead. http://msdn.microsoft.com/en-us/library/ms174402.aspx
        _ = sql.Append("SELECT DISTINCT ISNULL('[' + S3.Name + '].[' + object_name(D2.referencing_id) + ']','') AS DependOut, ");
        _ = sql.Append("'[' + S2.Name + '].[' + object_name(D.referenced_id) + ']' AS TableName, ");
        _ = sql.Append("D.referenced_id AS referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, ");
        _ = sql.Append("P.object_id, S.name as owner, P.name as name ");
        _ = sql.Append("FROM sys.views P ");
        _ = sql.Append("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
        _ = sql.Append("LEFT JOIN sys.sql_expression_dependencies D ON P.object_id = D.referencing_id ");
        _ = sql.Append("LEFT JOIN sys.objects O ON O.object_id = D.referenced_id ");
        _ = sql.Append("LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ");
        _ = sql.Append("LEFT JOIN sys.sql_expression_dependencies D2 ON P.object_id = D2.referenced_id ");
        _ = sql.Append("LEFT JOIN sys.objects O2 ON O2.object_id = D2.referencing_id ");
        _ = sql.Append("LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ");
        _ = sql.Append("ORDER BY P.object_id ");
        return sql.ToString();
    }

    #endregion View
}
