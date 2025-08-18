using System;
using System.Drawing;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Abstractions.Ui;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Ui;

public class SQLServerProjectHandler : IProjectHandler
{
    private SqlServerConnectFront DestinationControl;
    private SqlServerConnectFront SourceControl;

    private SqlOption Option;

    public IFront CreateDestinationSelector()
    {
        DestinationControl = new SqlServerConnectFront
        {
            ServerName = "(local)",
            UseWindowsAuthentication = true,
            UserName = "sa",
            Password = "",
            DatabaseName = "",
            Location = new Point(1, 1),
            Name = "DestinationControl",
            Dock = DockStyle.Fill,
            TabIndex = 10,
            Text = "DESTINATION DATABASE"
        };

        return DestinationControl;
    }

    public IFront CreateSourceSelector()
    {
        SourceControl = new SqlServerConnectFront
        {
            ServerName = "(local)",
            UseWindowsAuthentication = true,
            UserName = "sa",
            Password = "",
            DatabaseName = "",

            Location = new Point(1, 1),
            Name = "SourceControl",
            TabIndex = 10,
            Text = "SOURCE DATABASE",
            Dock = DockStyle.Fill,
        };

        return SourceControl;
    }

    public IDatabaseComparer GetDatabaseComparer() => new SQLServerComparer();

    public IGenerator SetDestinationGenerator(string connectionString, IOption options) => new SQLServerGenerator(connectionString, options);

    public IGenerator SetSourceGenerator(string connectionString, IOption options) => new SQLServerGenerator(connectionString, options);

    public string GetDestinationConnectionString() => DestinationControl.ConnectionString;

    public string GetDestinationDatabaseName() => DestinationControl.DatabaseName;

    public string GetDestinationServerName() => DestinationControl.ServerName;

    public string GetSourceConnectionString() => SourceControl.ConnectionString;

    public string GetSourceDatabaseName() => SourceControl.DatabaseName;

    public string GetSourceServerName() => SourceControl.ServerName;

    public IOption GetDefaultProjectOptions() => Option ??= new SqlOption();
    public void SetProjectOptions(IOption option)
    {
        if (option == null)
        {
            throw new ArgumentNullException(nameof(option));
        }
        else if (option is not SqlOption)
        {
            throw new NotSupportedException($"This project handler only supports {nameof(SqlOption)} options. {option.GetType().Name} not supported");
        }
        Option = option as SqlOption;
    }

    public OptionControl CreateOptionControl() => new SqlOptionsFront();

    public string GetScriptLanguage() => "mssql";

    public void Unload()
    {
        SourceControl.Dispose();
        DestinationControl.Dispose();
        SourceControl = null;
        DestinationControl = null;
    }

    public override string ToString() => "SQLServer 2005 or higher";
}
