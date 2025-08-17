using System;
using System.Globalization;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Misc;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Options;
#if DEBUG
#endif

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateDatabase
{
    private readonly string connectioString;
    private readonly SqlOption objectFilter;

    public bool UseDefaultVersionOnVersionParseError { get; private set; }

    /// <summary>
    /// Constructor de la clase.
    /// </summary>
    /// <param name="connectioString">Connection string de la base</param>
    public GenerateDatabase(string connectioString, SqlOption filter)
    {
        this.connectioString = connectioString;
        this.objectFilter = filter;
    }

    public DatabaseInfo Get(Database database)
    {
        var item = new DatabaseInfo();
        using (var conn = new SqlConnection(connectioString))
        {
            using (var command = new SqlCommand(DatabaseSQLCommand.GetVersion(database), conn))
            {
                conn.Open();

                item.Server = conn.DataSource;
                item.Database = conn.Database;

                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var versionValue = reader["Version"] as string;
                    try
                    {
                        // used to use the decimal as well when Azure was 10.25
                        var version = new Version(versionValue);
                        item.VersionNumber = float.Parse(string.Format("{0}.{1}", version.Major, version.Minor), CultureInfo.InvariantCulture);

                        if (reader.FieldCount > 1 && !reader.IsDBNull(1))
                        {
                            if (int.TryParse(reader[1].ToString(), out var edition)
                                && Enum.IsDefined(typeof(DatabaseInfo.SQLServerEdition), edition))
                            {
                                item.SetEdition((DatabaseInfo.SQLServerEdition)edition);
                            }
                        }

                    }
                    catch (Exception notAGoodIdeaToCatchAllErrors)
                    {
                        var exception = new SchemaException(
                            string.Format("Error parsing ProductVersion. ({0})", versionValue ?? "[null]")
                            , notAGoodIdeaToCatchAllErrors);

                        if (!UseDefaultVersionOnVersionParseError)
                        {
                            throw exception;
                        }
                    }
                }
            }

            using (var command = new SqlCommand(DatabaseSQLCommand.Get(item.Version, item.Edition, database), conn))
            {
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    item.Collation = reader["Collation"].ToString();
                    item.HasFullTextEnabled = ((int)reader["IsFulltextEnabled"]) == 1;
                }
            }

        }

        return item;
    }
}
