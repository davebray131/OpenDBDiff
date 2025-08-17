using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;

internal class DatabaseSQLCommand
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static string GetVersion(Database databaseSchema) =>
        "SELECT SERVERPROPERTY('productversion') AS Version, SERVERPROPERTY('EngineEdition') AS Edition";

    public static string Get(DatabaseInfo.SQLServerVersion version, DatabaseInfo.SQLServerEdition edition, Database databaseSchema) => version switch
    {
        DatabaseInfo.SQLServerVersion.SQLServer2005 => Get2005(databaseSchema),
        DatabaseInfo.SQLServerVersion.SQLServer2008 => Get2008(databaseSchema),
        DatabaseInfo.SQLServerVersion.SQLServer2008R2 => Get2008R2(databaseSchema),
        DatabaseInfo.SQLServerVersion.SQLServerAzure10 => GetAzure(databaseSchema),
        _ => edition == DatabaseInfo.SQLServerEdition.Azure ? GetAzure(databaseSchema) : Get2008R2(databaseSchema),
    };

    private static string Get2005(Database databaseSchema) =>
        $"SELECT DATABASEPROPERTYEX('{databaseSchema.Name}','IsFulltextEnabled') AS IsFullTextEnabled, DATABASEPROPERTYEX('{databaseSchema.Name}','Collation') AS Collation";

    private static string Get2008(Database databaseSchema) =>
        $"SELECT DATABASEPROPERTYEX('{databaseSchema.Name}','IsFulltextEnabled') AS IsFullTextEnabled, DATABASEPROPERTYEX('{databaseSchema.Name}','Collation') AS Collation";

    private static string Get2008R2(Database databaseSchema) =>
        $"SELECT DATABASEPROPERTYEX('{databaseSchema.Name}','IsFulltextEnabled') AS IsFullTextEnabled, DATABASEPROPERTYEX('{databaseSchema.Name}','Collation') AS Collation";

    private static string GetAzure(Database databaseSchema) =>
        $"SELECT 0 AS IsFullTextEnabled, DATABASEPROPERTYEX('{databaseSchema.Name}','Collation') AS Collation";
}
