using System.Text;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class FunctionSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition)
    {
        switch (version)
        {
            case DatabaseInfo.SQLServerVersion.SQLServer2005:
                return Get2005();

            case DatabaseInfo.SQLServerVersion.SQLServer2008:
                return Get2008();

            case DatabaseInfo.SQLServerVersion.SQLServerAzure10:
                return GetAzure();

            default:
                return edition == DatabaseInfo.SQLServerEdition.Azure ? GetAzure() : Get2008();
        }
    }

    private static string Get2005()
    {
        var sql = "";
        sql += "select distinct ";
        sql += "T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ";
        sql += "ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, ";
        sql += "P.type, ";
        sql += "AF.name AS assembly_name, ";
        sql += "AM.assembly_class, ";
        sql += "AM.assembly_id, ";
        sql += "AM.assembly_method, ";
        sql += "ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P ";
        sql += "INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ";
        sql += "LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ";
        sql += "LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ";
        sql += "LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ";
        sql += "LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ";
        sql += "LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ";
        sql += "LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ";
        sql += "LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id  ";
        sql += "LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ";
        sql += "LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1 ";
        sql += "LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id ";
        sql += "WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id";
        return sql;
    }

    private static string Get2008()
    {
        var sql = new StringBuilder();
        _ = sql.AppendLine("SELECT DISTINCT ");
        _ = sql.AppendLine("T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ");
        _ = sql.AppendLine("ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, ");
        _ = sql.AppendLine("P.type, ");
        _ = sql.AppendLine("AF.name AS assembly_name, ");
        _ = sql.AppendLine("AM.assembly_class, ");
        _ = sql.AppendLine("AM.assembly_id, ");
        _ = sql.AppendLine("AM.assembly_method, ");
        _ = sql.AppendLine("ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P ");
        _ = sql.AppendLine("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
        _ = sql.AppendLine("LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id ");
        _ = sql.AppendLine("LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id ");
        _ = sql.AppendLine("LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ");
        _ = sql.AppendLine("LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id ");
        _ = sql.AppendLine("LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id ");
        _ = sql.AppendLine("LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ");
        _ = sql.AppendLine("LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id  ");
        _ = sql.AppendLine("LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id ");
        _ = sql.AppendLine("LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1 ");
        _ = sql.AppendLine("LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id ");
        _ = sql.AppendLine("WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id");
        return sql.ToString();
    }

    private static string GetAzure()
    {
        var sql = new StringBuilder();
        _ = sql.AppendLine("SELECT DISTINCT ");
        _ = sql.AppendLine("T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ");
        _ = sql.AppendLine("ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs, ");
        _ = sql.AppendLine("P.type, ");
        _ = sql.AppendLine("AF.name AS assembly_name, ");
        _ = sql.AppendLine("AM.assembly_class, ");
        _ = sql.AppendLine("AM.assembly_id, ");
        _ = sql.AppendLine("AM.assembly_method, ");
        _ = sql.AppendLine("ISNULL('[' + S3.Name + '].[' + object_name(D2.referencing_id) + ']','') AS DependOut, ");
        _ = sql.AppendLine("'[' + S2.Name + '].[' + object_name(D.referenced_id) + ']' AS TableName, D.referenced_id AS referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P ");
        _ = sql.AppendLine("INNER JOIN sys.schemas S ON S.schema_id = P.schema_id ");
        _ = sql.AppendLine("LEFT JOIN sys.sql_expression_dependencies D ON P.object_id = D.referencing_id ");
        _ = sql.AppendLine("LEFT JOIN sys.objects O ON O.object_id = D.referenced_id ");
        _ = sql.AppendLine("LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id ");
        _ = sql.AppendLine("LEFT JOIN sys.sql_expression_dependencies D2 ON P.object_id = D2.referenced_id ");
        _ = sql.AppendLine("LEFT JOIN sys.objects O2 ON O2.object_id = D2.referencing_id ");
        _ = sql.AppendLine("LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id ");
        _ = sql.AppendLine("CROSS JOIN (SELECT null as object_id, null as execute_as_principal_id, null as assembly_class, null as assembly_id, null as assembly_method) AS AM ");
        _ = sql.AppendLine("CROSS JOIN (SELECT null AS name) AS AF");
        _ = sql.AppendLine("LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1 ");
        _ = sql.AppendLine("LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id ");
        _ = sql.AppendLine("WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id");
        return sql.ToString();
    }
}
