using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateFullText
    {
        private readonly Generate root;

        public GenerateFullText(Generate root)
        {
            this.root = root;
        }

        private static string GetSQL()
        {
            return SQLQueries.SQLQueryFactory.Get("GetFullTextCatalogs");
        }

        public void Fill(Database database, string connectionString)
        {
            if (database.Options.Ignore.FilterFullText)
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
                                FullText item = new FullText(database)
                                {
                                    Id = (int)reader["fulltext_catalog_id"],
                                    Name = reader["Name"].ToString(),
                                    Owner = reader["Owner"].ToString(),
                                    IsAccentSensity = (bool)reader["is_accent_sensitivity_on"],
                                    IsDefault = (bool)reader["is_default"]
                                };
                                if (!reader.IsDBNull(reader.GetOrdinal("path")))
                                    item.Path = reader["path"].ToString().Substring(0, reader["path"].ToString().Length - item.Name.Length);
                                if (!reader.IsDBNull(reader.GetOrdinal("FileGroupName")))
                                    item.FileGroupName = reader["FileGroupName"].ToString();
                                database.FullText.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }
}
