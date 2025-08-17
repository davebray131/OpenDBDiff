using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateSynonyms
{
    private readonly Generate root;

    public GenerateSynonyms(Generate root) => this.root = root;

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetSynonyms");

    public void Fill(Database database, string connectionString)
    {
        if (database.Options.Ignore.FilterSynonyms)
        {
            using var conn = new SqlConnection(connectionString);
            using var command = new SqlCommand(GetSQL(), conn);
            conn.Open();
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var item = new Synonym(database)
                {
                    Id = (int)reader["object_id"],
                    Name = reader["Name"].ToString(),
                    Owner = reader["Owner"].ToString(),
                    Value = reader["base_object_name"].ToString()
                };
                database.Synonyms.Add(item);
            }
        }
    }
}
