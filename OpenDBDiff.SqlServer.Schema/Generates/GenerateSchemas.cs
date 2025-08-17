using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateSchemas(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetSchemas");

    public void Fill(Database database, string connectioString)
    {
        if (database.Options.Ignore.FilterSchema)
        {
            using var conn = new SqlConnection(connectioString);
            using var command = new SqlCommand(GetSQL(), conn);
            conn.Open();
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = new Model.Schema(database)
                {
                    Id = (int)reader["schema_id"],
                    Name = reader["name"].ToString(),
                    Owner = reader["owner"].ToString()
                };
                database.Schemas.Add(item);
            }
        }
    }
}
