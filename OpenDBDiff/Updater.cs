using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff;

/// <summary>
/// Handles the SQL update queries
/// </summary>
static class Updater
{
    public static string CreateNew(ISchemaBase target, string connectionString)
    {
        var script = target.ToSql();
        script = script.Replace("GO", "");
        //script = script.Replace("\r", "");
        //script = script.Replace("\t", "");
        //script = script.Replace("\n", "");
        var result = string.Empty;
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(script, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                result = e.Message + "\n";
            }
        }
        return result;
    }

    public static string AddNew(ISchemaBase target, string connectionString)
    {
        var result = string.Empty;
        var script = target.ToSqlAdd();
        script = script.Replace("GO", "");
        //script = script.Replace("\r", "");
        //script = script.Replace("\t", "");
        //script = script.Replace("\n", "");

        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand(script, connection);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                result = e.Message + "\n";
            }
        }
        return result;
    }

    public static DataTable GetData(ISchemaBase selected, string connectionString)
    {
        var data = new DataTable();
        try
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand
            {
                Connection = connection,
                CommandText = "SELECT * FROM " + selected.FullName
            };

            connection.Open();
            var reader = command.ExecuteReader(CommandBehavior.KeyInfo);
            data.Load(reader);
            //data.Load(command.ExecuteReader());

            connection.Close();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        return data;
    }

    public static string Alter(ISchemaBase target, string connectionString)
    {
        var db = target.RootParent;
        using var connection = new SqlConnection(connectionString);
        if (db != null && DialogResult.Yes != MessageBox.Show(string.Format("Alter {0} {1} in {2}..{3}?\n(WARNING: No automatic backup is made!)",
                target.ObjectType,
                target.Name,
                connection.DataSource,
                connection.Database), "ALTER Destination?", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2))
        {
            return "Cancelled.";
        }

        var sb = new StringBuilder();
        var SqlDiff = target.ToSqlDiff([]);
        string[] splitOn = ["GO"];
        var tempList = SqlDiff.ToSQL().Split(splitOn, StringSplitOptions.RemoveEmptyEntries);
        var scripts = new List<string>(tempList);

        foreach (var sql in scripts)
        {
            var script = sql;
            //script = script.Replace("\r", "");
            //script = script.Replace("\t", "");
            //script = script.Replace("\n", " ");
            if (target.ObjectType == ObjectType.StoredProcedure)
            {
                script = sql.Replace("CREATE PROCEDURE", "ALTER PROCEDURE");
            }
            var command = new SqlCommand(script, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                sb.AppendLine($"{target.Name}: {e.Message}");
                connection.Close();
            }
        }
        return sb.ToString();
    }

    public static string Rebuild(ISchemaBase target, string connectionString)
    {
        var SqlDiff = target.ToSqlDiff([]);
        string[] splitOn = ["GO"];
        var tempList = SqlDiff.ToSQL().Split(splitOn, StringSplitOptions.RemoveEmptyEntries);
        var scripts = new List<string>(tempList);
        var result = string.Empty;
        var script = scripts[0];
        if (target.ObjectType == ObjectType.Table)
        {
            script = script.Replace("CREATE TABLE", "ALTER TABLE");
        }
        MessageBox.Show(script);
        return result;
    }

    public static bool CommitTable(DataTable table, string tableFullName, string ConnectionString)
    {
        using var connection = new SqlConnection(ConnectionString);
        var command = new SqlCommand("SELECT * FROM " + tableFullName, connection);
        var da = new SqlDataAdapter(command);
        try
        {
            using var builder = new SqlCommandBuilder(da);
            connection.Open();
            da.Update(table);
            connection.Close();
            return true;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return false;
        }
    }
}
