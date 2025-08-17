using System;
using System.Text;
using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GeneratePartitionFunctions(Generate root)
{
    private readonly Generate root = root;
    private static string GetSQL() => SQLQueries.SQLQueryFactory.Get("GetPartitionFunctions");

    private static string ToHex(byte[] stream)
    {
        var sHex = new StringBuilder(2 * stream.Length);
        for (var i = 0; i < stream.Length; i++)
        {
            sHex.AppendFormat("{0:X2} ", stream[i]);
        }

        return "0x" + sHex.ToString().Replace(" ", string.Empty);
    }

    public void Fill(Database database, string connectioString)
    {
        var lastObjectId = 0;
        PartitionFunction item = null;
        if (database.Options.Ignore.FilterPartitionFunction)
        {
            using var conn = new SqlConnection(connectioString);
            using var command = new SqlCommand(GetSQL(), conn);
            conn.Open();
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (lastObjectId != (int)reader["function_id"])
                {
                    lastObjectId = (int)reader["function_id"];
                    item = new PartitionFunction(database)
                    {
                        Id = (int)reader["function_id"],
                        Name = reader["name"].ToString(),
                        IsBoundaryRight = (bool)reader["IsRight"],
                        Precision = (byte)reader["precision"],
                        Scale = (byte)reader["scale"],
                        Size = (short)reader["max_length"],
                        Type = reader["TypeName"].ToString()
                    };
                    database.PartitionFunctions.Add(item);
                }

                switch (item.Type)
                {
                    case "binary":
                    case "varbinary":
                        item.Values.Add(ToHex((byte[])reader["value"]));
                        break;
                    case "date":
                        item.Values.Add($"'{(DateTime)reader["value"]:yyyy/MM/dd}'");
                        break;
                    case "smalldatetime":
                    case "datetime":
                        item.Values.Add($"'{(DateTime)reader["value"]:yyyy/MM/dd HH:mm:ss.fff}'");
                        break;
                    default:
                        item.Values.Add(reader["value"].ToString());
                        break;
                }

            }
        }
    }
}
