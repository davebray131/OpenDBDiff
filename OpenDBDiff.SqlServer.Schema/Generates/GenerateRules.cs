using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateRules(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetRules");

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
                database.Rules.Add(new Rule(database)
                {
                    Id = (int)reader["object_id"],
                    Name = reader["Name"].ToString(),
                    Owner = reader["Owner"].ToString(),
                    Text = reader["Definition"].ToString()
                });
            }
        }
    }
}
