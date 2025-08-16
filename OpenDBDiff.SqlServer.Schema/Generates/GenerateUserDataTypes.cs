using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Errors;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates
{
    public class GenerateUserDataTypes
    {
        private readonly Generate root;

        public GenerateUserDataTypes(Generate root)
        {
            this.root = root;
        }

        private static string GetSQLColumnsDependencies()
        {
            return SQLQueries.SQLQueryFactory.Get("GetSQLColumnsDependencies");
        }

        private static void FillColumnsDependencies(SchemaList<UserDataType, Database> types, string connectionString)
        {
            if (types == null) throw new ArgumentNullException("types");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(GetSQLColumnsDependencies(), conn))
                {
                    conn.Open();
                    command.CommandTimeout = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types[reader["TypeName"].ToString()].Dependencies.Add(new ObjectDependency(reader["TableName"].ToString(), reader["ColumnName"].ToString(), ConvertType.GetObjectType(reader["Type"].ToString())));
                        }
                    }
                }
            }
        }

        public void Fill(Database database, string connectionString, List<MessageLog> messages)
        {
            //not supported in azure yet http://msdn.microsoft.com/en-us/library/ee336233.aspx
            if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10) return;

            try
            {
                if (database.Options.Ignore.FilterUserDataType)
                {
                    root.RaiseOnReading(new ProgressEventArgs("Reading UDT...", Constants.READING_UDT));
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(UserDataTypeCommand.Get(database.Info.Version, database.Info.Edition), conn))
                        {
                            conn.Open();
                            command.CommandTimeout = 0;
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    root.RaiseOnReadingOne(reader["Name"]);
                                    UserDataType item = new UserDataType(database)
                                    {
                                        Id = (int)reader["tid"],
                                        AllowNull = (bool)reader["is_nullable"],
                                        Size = (short)reader["max_length"],
                                        Name = reader["Name"].ToString(),
                                        Owner = reader["owner"].ToString(),
                                        Precision = int.Parse(reader["precision"].ToString()),
                                        Scale = int.Parse(reader["scale"].ToString())
                                    };
                                    if (!string.IsNullOrEmpty(reader["defaultname"].ToString()))
                                    {
                                        item.Default.Name = reader["defaultname"].ToString();
                                        item.Default.Owner = reader["defaultowner"].ToString();
                                    }
                                    if (!string.IsNullOrEmpty(reader["rulename"].ToString()))
                                    {
                                        item.Rule.Name = reader["rulename"].ToString();
                                        item.Rule.Owner = reader["ruleowner"].ToString();
                                    }
                                    item.Type = reader["basetypename"].ToString();
                                    item.IsAssembly = (bool)reader["is_assembly_type"];
                                    item.AssemblyId = (int)reader["assembly_id"];
                                    item.AssemblyName = reader["assembly_name"].ToString();
                                    item.AssemblyClass = reader["assembly_class"].ToString();
                                    database.UserTypes.Add(item);
                                }
                            }
                        }
                    }
                    if (database.Options.Ignore.FilterTable)
                        FillColumnsDependencies(database.UserTypes, connectionString);
                }
            }
            catch (Exception ex)
            {
                messages.Add(new MessageLog(ex.Message, ex.StackTrace, MessageLog.LogType.Error));
            }
        }
    }
}
