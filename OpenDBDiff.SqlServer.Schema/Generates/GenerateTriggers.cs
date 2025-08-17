using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Errors;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateTriggers(Generate root)
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    private static string GetSQL(DatabaseInfo.SQLServerVersion version, SqlOption options) => version == DatabaseInfo.SQLServerVersion.SQLServerAzure10
            ? SQLQueries.SQLQueryFactory.Get("GetTriggers", version)
            : SQLQueries.SQLQueryFactory.Get("GetTriggers");

    public void Fill(Database database, string connectionString, List<MessageLog> messages)
    {
        var parentId = 0;
        ISchemaBase parent = null;
        string type;
        try
        {
            if (database.Options.Ignore.FilterTrigger)
            {
                root.RaiseOnReading(new ProgressEventArgs("Reading Triggers...", Constants.READING_TRIGGERS));
                using var conn = new SqlConnection(connectionString);
                using var command = new SqlCommand(GetSQL(database.Info.Version, database.Options), conn);
                conn.Open();
                command.CommandTimeout = 0;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    root.RaiseOnReadingOne(reader["Name"]);
                    type = reader["ObjectType"].ToString().Trim();
                    if (parentId != (int)reader["parent_id"])
                    {
                        parentId = (int)reader["parent_id"];
                        parent = type.Equals("V") ? database.Views.Find(parentId) : database.Tables.Find(parentId);
                    }
                    if (parent == null) { continue; }
                    if (reader["type"].Equals("TR"))
                    {
                        var item = new Trigger(parent)
                        {
                            Id = (int)reader["object_id"],
                            Name = reader["Name"].ToString(),
                            InsteadOf = (bool)reader["is_instead_of_trigger"],
                            IsDisabled = (bool)reader["is_disabled"],
                            IsDDLTrigger = false,
                            Owner = reader["Owner"].ToString()
                        };
                        if (database.Options.Ignore.FilterNotForReplication)
                        {
                            item.NotForReplication = (bool)reader["is_not_for_replication"];
                        }

                        if (type.Equals("V"))
                        {
                            ((View)parent).Triggers.Add(item);
                        }
                        else
                        {
                            ((Table)parent).Triggers.Add(item);
                        }
                    }
                    else
                    {
                        var item = new CLRTrigger(parent)
                        {
                            Id = (int)reader["object_id"],
                            Name = reader["Name"].ToString(),
                            IsDelete = (bool)reader["IsDelete"],
                            IsUpdate = (bool)reader["IsUpdate"],
                            IsInsert = (bool)reader["IsInsert"],
                            Owner = reader["Owner"].ToString(),
                            IsAssembly = true,
                            AssemblyId = (int)reader["assembly_id"],
                            AssemblyName = reader["assembly_name"].ToString(),
                            AssemblyClass = reader["assembly_class"].ToString(),
                            AssemblyExecuteAs = reader["ExecuteAs"].ToString(),
                            AssemblyMethod = reader["assembly_method"].ToString()
                        };
                        if (type.Equals("V"))
                        {
                            ((View)parent).CLRTriggers.Add(item);
                        }
                        else
                        {
                            ((Table)parent).CLRTriggers.Add(item);
                        }
                        /*if (!database.Options.Ignore.FilterIgnoreNotForReplication)
trigger.NotForReplication = (bool)reader["is_not_for_replication"];*/
                    }
                }
            }
        }
        catch (Exception ex)
        {
            messages.Add(new MessageLog(ex.Message, ex.StackTrace, MessageLog.LogType.Error));
        }
    }
}
