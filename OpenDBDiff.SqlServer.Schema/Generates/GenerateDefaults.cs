using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateDefaults(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetDefaults");

    public void Fill(Database database, string connectionString)
    {
        if (database.Options.Ignore.FilterRules)
        {
            using var conn = new SqlConnection(connectionString);
            using var command = new SqlCommand(GetSQL(), conn);
            conn.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = new Default(database)
                {
                    Id = (int)reader["object_id"],
                    Name = reader["Name"].ToString(),
                    Owner = reader["Owner"].ToString(),
                    Value = reader["Definition"].ToString()
                };
                database.Defaults.Add(item);
            }
        }
    }
}
