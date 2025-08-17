using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Errors;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateExtendedProperties(Generate root)
{
    private readonly Generate root = root;
    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetExtendedProperties");

    private static string GetTypeDescription(string type) => type switch
    {
        "P" or "PC" => "PROCEDURE",
        "V" => "VIEW",
        "U" => "TABLE",
        "TR" or "TA" => "TRIGGER",
        "FS" or "FN" or "IF" or "TF" => "FUNCTION",
        _ => string.Empty
    };

    public void Fill(Database database, string connectionString, List<MessageLog> messages)
    {
        ISQLServerSchemaBase parent;
        try
        {
            if (database.Options.Ignore.FilterExtendedProperties)
            {
                using var conn = new SqlConnection(connectionString);
                using var command = new SqlCommand(GetSQL(), conn);
                conn.Open();
                command.CommandTimeout = 0;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var item = new ExtendedProperty(null);
                    if (((byte)reader["Class"]) == 5)
                    {
                        item.Level0type = "ASSEMBLY";
                        item.Level0name = reader["AssemblyName"].ToString();
                    }
                    if (((byte)reader["Class"]) == 1)
                    {
                        var ObjectType = GetTypeDescription(reader["type"].ToString().Trim());
                        item.Level0type = "SCHEMA";
                        item.Level0name = reader["Owner"].ToString();
                        if (!ObjectType.Equals("TRIGGER"))
                        {
                            item.Level1name = reader["ObjectName"].ToString();
                            item.Level1type = ObjectType;
                        }
                        else
                        {
                            item.Level1type = "TABLE";
                            item.Level1name = reader["ParentName"].ToString();
                            item.Level2name = reader["ObjectName"].ToString();
                            item.Level2type = ObjectType;
                        }
                    }
                    if (((byte)reader["Class"]) == 6)
                    {
                        item.Level0type = "SCHEMA";
                        item.Level0name = reader["OwnerType"].ToString();
                        item.Level1name = reader["TypeName"].ToString();
                        item.Level1type = "TYPE";
                    }
                    if (((byte)reader["Class"]) == 7)
                    {
                        item.Level0type = "SCHEMA";
                        item.Level0name = reader["Owner"].ToString();
                        item.Level1type = "TABLE";
                        item.Level1name = reader["ObjectName"].ToString();
                        item.Level2type = reader["class_desc"].ToString();
                        item.Level2name = reader["IndexName"].ToString();
                    }
                    item.Value = reader["Value"].ToString();
                    item.Name = reader["Name"].ToString();
                    parent = (ISQLServerSchemaBase)database.Find(item.FullName);
                    if (parent != null)
                    {
                        item.Parent = (ISchemaBase)parent;
                        parent.ExtendedProperties.Add(item);
                    }
                    else
                    {
                        messages.Add(new MessageLog(item.FullName + " not found in extended properties.", "", MessageLog.LogType.Error));
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
