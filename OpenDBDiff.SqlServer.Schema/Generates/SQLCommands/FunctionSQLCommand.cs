using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class FunctionSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2005 => Get2005(),
        DatabaseInfo.SQLServerVersion.SQLServer2008 => Get2008(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetAzure(),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetAzure() : Get2008(),
    };

    private static string Get2005() =>
        """
        SELECT DISTINCT
          T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale, ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs,
          P.type, AF.name AS assembly_name, AM.assembly_class, AM.assembly_id, AM.assembly_method,
          ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P
        INNER JOIN sys.schemas S ON S.schema_id = P.schema_id
        LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id
        LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id
        LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id
        LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id
        LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id
        LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id
        LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id
        LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1
        LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id
        WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id
        """;

    private static string Get2008() =>
        """
        SELECT DISTINCT
          T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale,ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs,
          P.type,AF.name AS assembly_name,AM.assembly_class,AM.assembly_id, AM.assembly_method,
          ISNULL('[' + S3.Name + '].[' + object_name(D2.object_id) + ']','') AS DependOut, '[' + S2.Name + '].[' + object_name(D.referenced_major_id) + ']' AS TableName, D.referenced_major_id, 
          OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, P.object_id, S.name as owner, P.name as name from sys.objects P
        INNER JOIN sys.schemas S ON S.schema_id = P.schema_id
        LEFT JOIN sys.sql_dependencies D ON P.object_id = D.object_id
        LEFT JOIN sys.objects O ON O.object_id = D.referenced_major_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id
        LEFT JOIN sys.sql_dependencies D2 ON P.object_id = D2.referenced_major_id
        LEFT JOIN sys.objects O2 ON O2.object_id = D2.object_id
        LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id
        LEFT JOIN sys.assembly_modules AM ON AM.object_id = P.object_id
        LEFT JOIN sys.assemblies AF ON AF.assembly_id = AM.assembly_id
        LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1
        LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id
        WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id
        """;

    private static string GetAzure() =>
        """
        SELECT DISTINCT
          T.name AS ReturnType, PP.max_length, PP.precision, PP.Scale,ISNULL(CONVERT(varchar,AM.execute_as_principal_id),'CALLER') as ExecuteAs,
          P.type,AF.name AS assembly_name,AM.assembly_class,AM.assembly_id,AM.assembly_method,
          ISNULL('[' + S3.Name + '].[' + object_name(D2.referencing_id) + ']','') AS DependOut,
          '[' + S2.Name + '].[' + object_name(D.referenced_id) + ']' AS TableName, D.referenced_id AS referenced_major_id, OBJECTPROPERTY (P.object_id,'IsSchemaBound') AS IsSchemaBound, 
          P.object_id, S.name as owner, P.name as name from sys.objects P
        INNER JOIN sys.schemas S ON S.schema_id = P.schema_id
        LEFT JOIN sys.sql_expression_dependencies D ON P.object_id = D.referencing_id
        LEFT JOIN sys.objects O ON O.object_id = D.referenced_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O.schema_id
        LEFT JOIN sys.sql_expression_dependencies D2 ON P.object_id = D2.referenced_id
        LEFT JOIN sys.objects O2 ON O2.object_id = D2.referencing_id
        LEFT JOIN sys.schemas S3 ON S3.schema_id = O2.schema_id
        CROSS JOIN (SELECT null as object_id, null as execute_as_principal_id, null as assembly_class, null as assembly_id, null as assembly_method) AS AM
        CROSS JOIN (SELECT null AS name) AS AF
        LEFT JOIN sys.parameters PP ON PP.object_id = AM.object_id AND PP.parameter_id = 0 and PP.is_output = 1
        LEFT JOIN sys.types T ON T.system_type_id = PP.system_type_id
        WHERE P.type IN ('IF','FN','TF','FS') ORDER BY P.object_id
        """;
}
