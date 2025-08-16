using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateFullTextIndex
    {
        private readonly Generate root;

        public GenerateFullTextIndex(Generate root)
        {
            this.root = root;
        }

        public void Fill(Database database, string connectionString)
        {
            //not supported in azure yet
            if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10) return;

            int parentId = 0;
            Table parent = null;
            root.RaiseOnReading(new ProgressEventArgs("Reading FullText Index...", Constants.READING_INDEXES));
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(FullTextIndexSQLCommand.Get(database.Info.Version), conn))
                {
                    conn.Open();
                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        FullTextIndex item = null;
                        while (reader.Read())
                        {
                            root.RaiseOnReadingOne(reader["Name"]);
                            bool change;
                            if (parentId != (int)reader["object_id"])
                            {
                                parentId = (int)reader["object_id"];
                                parent = database.Tables.Find(parentId);
                                change = true;
                            }
                            else
                                change = false;
                            if (change)
                            {
                                item = new FullTextIndex(parent)
                                {
                                    Name = reader["Name"].ToString(),
                                    Owner = parent.Owner,
                                    FullText = reader["FullTextCatalogName"].ToString(),
                                    Index = reader["IndexName"].ToString(),
                                    IsDisabled = !(bool)reader["is_enabled"],
                                    ChangeTrackingState = reader["ChangeTracking"].ToString()
                                };
                                if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
                                    item.FileGroup = reader["FileGroupName"].ToString();
                                parent.FullTextIndex.Add(item);
                            }
                            FullTextIndexColumn ccon = new FullTextIndexColumn
                            {
                                ColumnName = reader["ColumnName"].ToString(),
                                Language = reader["LanguageName"].ToString()
                            };
                            item.Columns.Add(ccon);
                        }
                    }
                }
            }
        }
    }
}
