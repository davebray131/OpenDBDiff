using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;


public class GenerateDDLTriggers(Generate root)
{
    private readonly Generate root = root;
    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetDDLTriggers");

    public void Fill(Database database, string connectionString)
    {
        if (database.Options.Ignore.FilterDDLTriggers)
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();
            using var command = new SqlCommand(GetSQL(), conn);
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var trigger = new Trigger(database)
                {
                    Text = reader["Text"].ToString(),
                    Name = reader["Name"].ToString(),
                    InsteadOf = (bool)reader["is_instead_of_trigger"],
                    IsDisabled = (bool)reader["is_disabled"],
                    IsDDLTrigger = true,
                    NotForReplication = (bool)reader["is_not_for_replication"],
                    Owner = ""
                };
                database.DDLTriggers.Add(trigger);
            }
        }
    }
}
