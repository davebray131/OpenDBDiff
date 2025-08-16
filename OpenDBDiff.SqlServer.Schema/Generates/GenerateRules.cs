using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateRules
    {
        private readonly Generate root;

        public GenerateRules(Generate root)
        {
            this.root = root;
        }

        private static string GetSQL()
        {
            return SQLQueries.SQLQueryFactory.Get("GetRules");
        }

        public void Fill(Database database, string connectionString)
        {
            if (database.Options.Ignore.FilterRules)
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
                                Rule item = new Rule(database)
                                {
                                    Id = (int)reader["object_id"],
                                    Name = reader["Name"].ToString(),
                                    Owner = reader["Owner"].ToString(),
                                    Text = reader["Definition"].ToString()
                                };
                                database.Rules.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }
}
