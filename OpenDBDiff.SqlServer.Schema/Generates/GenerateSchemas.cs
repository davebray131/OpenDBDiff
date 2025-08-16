using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateSchemas
    {
        private readonly Generate root;

        public GenerateSchemas(Generate root)
        {
            this.root = root;
        }

        private static string GetSQL()
        {
            return SQLQueries.SQLQueryFactory.Get("GetSchemas");
        }

        public void Fill(Database database, string connectioString)
        {
            if (database.Options.Ignore.FilterSchema)
            {
                using (SqlConnection conn = new SqlConnection(connectioString))
                {
                    using (SqlCommand command = new SqlCommand(GetSQL(), conn))
                    {
                        conn.Open();
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Model.Schema item = new Model.Schema(database)
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
            }
        }
    }
}
