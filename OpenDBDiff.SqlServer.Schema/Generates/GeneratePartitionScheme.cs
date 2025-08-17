using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GeneratePartitionScheme(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetPartitionSchemes");

    public void Fill(Database database, string connectioString)
    {
        var lastObjectId = 0;
        PartitionScheme item = null;
        if (database.Options.Ignore.FilterPartitionScheme)
        {
            using var conn = new SqlConnection(connectioString);
            using var command = new SqlCommand(GetSQL(), conn);
            conn.Open();
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (lastObjectId != (int)reader["ID"])
                {
                    lastObjectId = (int)reader["ID"];
                    item = new PartitionScheme(database)
                    {
                        Id = (int)reader["ID"],
                        Name = reader["name"].ToString(),
                        PartitionFunction = reader["FunctionName"].ToString()
                    };
                    database.PartitionSchemes.Add(item);
                }
                item.FileGroups.Add(reader["FileGroupName"].ToString());
            }
        }
    }
}
