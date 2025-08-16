using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateFileGroups
    {
        private readonly Generate root;

        public GenerateFileGroups(Generate root)
        {
            this.root = root;
        }

        private static string GetSQLFile(FileGroup filegroup)
        {
            string query = SQLQueries.SQLQueryFactory.Get("GetDatabaseFile");

            return query.Replace("{ID}", filegroup.Id.ToString());
        }

        private static string GetSQL()
        {
            return SQLQueries.SQLQueryFactory.Get("GetFileGroups");
        }

        private static void FillFiles(FileGroup filegroup, string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(GetSQLFile(filegroup), conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FileGroupFile item = new FileGroupFile(filegroup)
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
                }
            }
        }

        public void Fill(Database database, string connectionString)
        {
            try
            {
                if (database.Options.Ignore.FilterTableFileGroup)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(GetSQL(), conn))
                        {
                            conn.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    FileGroup item = new FileGroup(database)
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
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
