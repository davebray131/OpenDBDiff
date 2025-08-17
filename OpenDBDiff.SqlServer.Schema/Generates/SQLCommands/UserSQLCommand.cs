using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal static class UserSQLCommand
{
    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2000 or DatabaseInfo.SQLServerVersion.SQLServer2005 or DatabaseInfo.SQLServerVersion.SQLServer2008 or DatabaseInfo.SQLServerVersion.SQLServer2008R2 => Get2008(),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetAzure(),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetAzure() : Get2008(),
    };

    private static string Get2008() =>
        """
        SELECT 
          is_fixed_role, type, ISNULL(suser_sname(sid),'') AS Login,Name,principal_id, ISNULL(default_schema_name,'') AS default_schema_name
        FROM sys.database_principals
        WHERE type IN ('S','U','A','R')
        ORDER BY Name
        """;

    private static string GetAzure() =>
        //to get LoginName in Azure (asside for the current login) you would have to link to master and query sys.sysusers or sys.sql_users
        //the CASE test below will at least get you the Current login

        """
        SELECT  
          is_fixed_role, type, CASE WHEN suser_sid()=sid THEN suser_sname() ELSE '' END  AS Login,Name,principal_id, ISNULL(default_schema_name,'') AS default_schema_name
        FROM sys.database_principals
        WHERE type IN ('S','U','A','R')
        ORDER BY Name
        """;
}
