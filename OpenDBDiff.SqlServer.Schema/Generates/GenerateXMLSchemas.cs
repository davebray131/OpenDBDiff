using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateXMLSchemas(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQLColumnsDependencies() => SQLQueries.SQLQueryFactory.Get("GetXMLSchemaCollections");

    private static string GetSQLXMLSchema() => SQLQueries.SQLQueryFactory.Get("GetSQLXMLSchema");

    private static void FillColumnsDependencies(SchemaList<XMLSchema, Database> items, string connectionString)
    {
        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(GetSQLColumnsDependencies(), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            items[reader["XMLName"].ToString()].Dependencies.Add(new ObjectDependency(reader["TableName"].ToString(), reader["ColumnName"].ToString(), ConvertType.GetObjectType(reader["Type"].ToString())));
        }
    }

    public void Fill(Database database, string connectionString)
    {
        //TODO XML_SCHEMA_NAMESPACE function not supported in Azure, is there a workaround?
        //not supported in azure yet
        if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10)
        {
            return;
        }

        if (database.Options.Ignore.FilterXMLSchema)
        {
            root.RaiseOnReading(new ProgressEventArgs("Reading XML Schema...", Constants.READING_XMLSCHEMAS));
            using (var conn = new SqlConnection(connectionString))
            {
                using var command = new SqlCommand(GetSQLXMLSchema(), conn);
                conn.Open();
                command.CommandTimeout = 0;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    root.RaiseOnReadingOne(reader["name"]);
                    var item = new XMLSchema(database)
                    {
                        Id = (int)reader["ID"],
                        Name = reader["name"].ToString(),
                        Owner = reader["owner"].ToString(),
                        Text = reader["Text"].ToString()
                    };
                    database.XmlSchemas.Add(item);

                }
            }
            if (database.Options.Ignore.FilterTable)
            {
                FillColumnsDependencies(database.XmlSchemas, connectionString);
            }
        }
    }
}
