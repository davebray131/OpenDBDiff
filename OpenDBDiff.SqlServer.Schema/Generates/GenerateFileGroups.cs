using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateFileGroups(Generate root)
{
    private readonly Generate root = root;

    private static string GetSQLFile(FileGroup filegroup)
    {
        var query = SQLQueries.SQLQueryFactory.Get("GetDatabaseFile");

        return query.Replace("{ID}", filegroup.Id.ToString());
    }

    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetFileGroups");

    private static void FillFiles(FileGroup filegroup, string connectionString)
    {
        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(GetSQLFile(filegroup), conn);
        conn.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var item = new FileGroupFile(filegroup)
            {
                Id = (int)reader["file_id"],
                Name = reader["name"].ToString(),
                Owner = "",
                Growth = (int)reader["growth"],
                IsPercentGrowth = (bool)reader["is_percent_growth"],
                IsSparse = (bool)reader["is_sparse"],
                MaxSize = (int)reader["max_size"],
                PhysicalName = reader["physical_name"].ToString(),
                Size = (int)reader["size"],
                Type = (byte)reader["type"]
            };
            filegroup.Files.Add(item);
        }
    }

    public void Fill(Database database, string connectionString)
    {
        try
        {
            if (database.Options.Ignore.FilterTableFileGroup)
            {
                using var conn = new SqlConnection(connectionString);
                using var command = new SqlCommand(GetSQL(), conn);
                conn.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new FileGroup(database)
                    {
                        Id = (int)reader["ID"],
                        Name = reader["name"].ToString(),
                        Owner = "",
                        IsDefaultFileGroup = (bool)reader["is_default"],
                        IsReadOnly = (bool)reader["is_read_only"],
                        IsFileStream = reader["type"].Equals("FD")
                    };
                    FillFiles(item, connectionString);
                    database.FileGroups.Add(item);
                }
            }
        }
        catch
        {
            throw;
        }
    }
}
