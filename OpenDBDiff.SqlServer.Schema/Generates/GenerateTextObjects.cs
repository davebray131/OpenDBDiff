using System;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateTextObjects
{
    private readonly Generate root;

    public GenerateTextObjects(Generate root) => this.root = root;

    private static string GetSQL(SqlOption options)
    {
        var filterQuery = SQLQueries.SQLQueryFactory.Get("GetTextObjectsQuery");
        var filter = "";
        if (options.Ignore.FilterStoredProcedure)
        {
            filter += "O.type = 'P' OR ";
        }

        if (options.Ignore.FilterView)
        {
            filter += "O.type = 'V' OR ";
        }

        if (options.Ignore.FilterTrigger)
        {
            filter += "O.type = 'TR' OR ";
        }

        if (options.Ignore.FilterFunction)
        {
            filter += "O.type IN ('IF','FN','TF') OR ";
        }

        filter = filter.Substring(0, filter.Length - 4);
        return filterQuery.Replace("{FILTER}", filter);
    }

    public void Fill(Database database, string connectionString)
    {
        ICode code;
        try
        {
            if (database.Options.Ignore.FilterStoredProcedure || database.Options.Ignore.FilterView || database.Options.Ignore.FilterFunction || database.Options.Ignore.FilterTrigger)
            {
                root.RaiseOnReading(new ProgressEventArgs("Reading Text Objects...", Constants.READING_TEXTOBJECTS));
                using var conn = new SqlConnection(connectionString);
                using var command = new SqlCommand(GetSQL(database.Options), conn);
                conn.Open();
                command.CommandTimeout = 0;
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    code = null;
                    root.RaiseOnReadingOne(reader["name"]);
                    var type = reader["Type"].ToString().Trim();
                    var name = reader["name"].ToString();
                    var definition = reader["Text"].ToString();
                    var id = (int)reader["object_id"];
                    if (type.Equals("V"))
                    {
                        code = database.Views.Find(id);
                    }

                    if (type.Equals("TR"))
                    {
                        code = (ICode)database.Find(id);
                    }

                    if (type.Equals("P"))
                    {
                        var procedure = database.Procedures.Find(id);
                        if (procedure != null)
                        {
                            ((ICode)procedure).Text = GetObjectDefinition(type, name, definition);
                        }
                    }

                    if (type.Equals("IF") || type.Equals("FN") || type.Equals("TF"))
                    {
                        code = database.Functions.Find(id);
                    }

                    if (code != null)
                    {
                        code.Text = reader["Text"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string GetObjectDefinition(string type, string name, string definition)
    {
        var rv = definition;

        var sqlDelimiters = @"(\r|\n|\s)+?";
        var options = RegexOptions.IgnoreCase | RegexOptions.Multiline;
        var re = new Regex(@"CREATE" + sqlDelimiters + @"PROC(EDURE)?" + sqlDelimiters + @"(\w+\.|\[\w+\]\.)?\[?(?<spname>\w+)\]?" + sqlDelimiters, options);
        switch (type)
        {
            case "P":
                var match = re.Match(definition);
                if (match != null && match.Success)
                {
                    // Try to replace the name saved in the definition when the object was created by the one used for the object in sys.object
                    var oldName = match.Groups["spname"].Value;
                    //if (string.IsNullOrEmpty(oldName)) System.Diagnostics.Debugger.Break();
                    if (string.Compare(oldName, name) != 0)
                    {
                        rv = rv.Replace(oldName, name);
                    }
                }
                break;
            default:
                //TODO : Add the logic used for other objects than procedures
                break;
        }

        return rv;
    }
}
