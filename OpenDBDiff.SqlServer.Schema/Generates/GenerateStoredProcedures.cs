using System.Linq;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateStoredProcedures
    {
        private static int NameIndex = -1;
        private static int object_idIndex = -1;
        private static int ownerIndex = -1;
        private static int typeIndex = -1;

        private readonly Generate root;

        public GenerateStoredProcedures(Generate root)
        {
            this.root = root;
        }

        private static void InitIndex(SqlDataReader reader)
        {
            if (NameIndex == -1)
            {
                object_idIndex = reader.GetOrdinal("object_id");
                NameIndex = reader.GetOrdinal("Name");
                ownerIndex = reader.GetOrdinal("owner");
                typeIndex = reader.GetOrdinal("type");
            }
        }

        private static string GetSQLParameters()
        {
            return SQLQueries.SQLQueryFactory.Get("GetParameters");
        }

        private static string GetSQL(DatabaseInfo.SQLServerVersion version)
        {
            if (version == DatabaseInfo.SQLServerVersion.SQLServerAzure10)
            {
                return SQLQueries.SQLQueryFactory.Get("GetProcedures", DatabaseInfo.SQLServerVersion.SQLServerAzure10);
            }
            else
            {
                return SQLQueries.SQLQueryFactory.Get("GetProcedures");
            }
        }

        private static void FillParameters(Database database, string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(GetSQLParameters(), conn))
                {
                    conn.Open();
                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var objectName = reader["ObjectName"].ToString();

                            if (database.CLRProcedures.Contains(objectName))
                            {
                                Parameter param = new Parameter
                                {
                                    Name = reader["Name"].ToString(),
                                    Type = reader["TypeName"].ToString(),
                                    Size = (short)reader["max_length"],
                                    Scale = (byte)reader["scale"],
                                    Precision = (byte)reader["precision"],
                                    Output = (bool)reader["is_output"]
                                };
                                if (param.Type.Equals("nchar") || param.Type.Equals("nvarchar"))
                                {
                                    if (param.Size != -1)
                                        param.Size /= 2;
                                }
                                database.CLRProcedures[objectName].Parameters.Add(param);
                            }
                        }
                    }
                }
            }
        }

        public void Fill(Database database, string connectionString)
        {
            if (database.Options.Ignore.FilterStoredProcedure || database.Options.Ignore.FilterCLRStoredProcedure)
            {
                root.RaiseOnReading(new ProgressEventArgs("Reading stored procedures...", Constants.READING_PROCEDURES));
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(GetSQL(database.Info.Version), conn))
                    {
                        conn.Open();
                        command.CommandTimeout = 0;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InitIndex(reader);
                                root.RaiseOnReadingOne(reader[NameIndex]);

                                var objectType = reader[typeIndex].ToString().Trim();
                                switch (objectType)
                                {
                                    case "P":
                                        if (database.Options.Ignore.FilterStoredProcedure)
                                        {
                                            StoredProcedure item = new StoredProcedure(database)
                                            {
                                                Id = (int)reader[object_idIndex],
                                                Name = (string)reader[NameIndex],
                                                Owner = (string)reader[ownerIndex]
                                            };
                                            database.Procedures.Add(item);
                                        }
                                        break;

                                    case "PC":
                                        if (database.Options.Ignore.FilterCLRStoredProcedure)
                                        {
                                            CLRStoredProcedure item = new CLRStoredProcedure(database)
                                            {
                                                Id = (int)reader[object_idIndex],
                                                Name = reader[NameIndex].ToString(),
                                                Owner = reader[ownerIndex].ToString(),
                                                IsAssembly = true,
                                                AssemblyId = (int)reader["assembly_id"],
                                                AssemblyName = reader["assembly_name"].ToString(),
                                                AssemblyClass = reader["assembly_class"].ToString(),
                                                AssemblyExecuteAs = reader["ExecuteAs"].ToString(),
                                                AssemblyMethod = reader["assembly_method"].ToString()
                                            };
                                            database.CLRProcedures.Add(item);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                if (database.CLRProcedures.Any())
                    FillParameters(database, connectionString);
            }
        }
    }
}
