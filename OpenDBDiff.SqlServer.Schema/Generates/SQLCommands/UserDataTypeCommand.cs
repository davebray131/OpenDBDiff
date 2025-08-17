using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class UserDataTypeCommand
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2000 => Get2000(),
        DatabaseInfo.SQLServerVersion.SQLServer2005 => Get2005(),
        _ => Get2008(),
    };

    public static string Get2008() =>
        """
        SELECT 
          ISNULL(AF.name,'') AS assembly_name, ISNULL(AT.assembly_id,0) AS assembly_id, ISNULL(assembly_class,'') AS assembly_class, T.max_length, S2.name as defaultowner, 
          O2.name as defaultname, S1.name as ruleowner, O.name as rulename, ISNULL(T2.Name,'') AS basetypename, S.Name AS Owner, T.Name, T.is_assembly_type, T.user_type_id AS tid, 
          T.is_nullable, T.precision, T.scale
        FROM sys.types T
        INNER JOIN sys.schemas S ON S.schema_id = T.schema_id
        LEFT JOIN sys.types T2 ON T2.user_type_id = T.system_type_id
        LEFT JOIN sys.objects O ON O.type = 'R' and O.object_id = T.rule_object_id
        LEFT JOIN sys.schemas S1 ON S1.schema_id = O.schema_id
        LEFT JOIN sys.objects O2 ON O2.type = 'D' and O2.object_id = T.default_object_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O2.schema_id
        LEFT JOIN sys.assembly_types AT ON AT.user_type_id = T.user_type_id AND T.is_assembly_type = 1
        LEFT JOIN sys.assemblies AF ON AF.assembly_id = AT.assembly_id
        WHERE T.is_user_defined = 1 AND T.is_table_type = 0 ORDER BY T.Name
        """;

    public static string Get2005() =>
        """
        SELECT 
          ISNULL(AF.name,'') AS assembly_name, ISNULL(AT.assembly_id,0) AS assembly_id, ISNULL(assembly_class,'') AS assembly_class, T.max_length, 
          S2.name as defaultowner, O2.name as defaultname, S1.name as ruleowner, O.name as rulename, ISNULL(T2.Name,'') AS basetypename, S.Name AS Owner, 
          T.Name, T.is_assembly_type, T.user_type_id AS tid, T.is_nullable, T.precision, T.scale from sys.types T ";
        INNER JOIN sys.schemas S ON S.schema_id = T.schema_id
        LEFT JOIN sys.types T2 ON T2.user_type_id = T.system_type_id
        LEFT JOIN sys.objects O ON O.type = 'R' and O.object_id = T.rule_object_id
        LEFT JOIN sys.schemas S1 ON S1.schema_id = O.schema_id
        LEFT JOIN sys.objects O2 ON O2.type = 'D' and O2.object_id = T.default_object_id
        LEFT JOIN sys.schemas S2 ON S2.schema_id = O2.schema_id
        LEFT JOIN sys.assembly_types AT ON AT.user_type_id = T.user_type_id AND T.is_assembly_type = 1
        LEFT JOIN sys.assemblies AF ON AF.assembly_id = AT.assembly_id
        WHERE T.is_user_defined = 1 ORDER BY T.Name
        """;

    public static string Get2000() => "";
}
