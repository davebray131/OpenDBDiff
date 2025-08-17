using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Abstractions.Ui;
using OpenDBDiff.SqlServer.Schema.Generates;
using OpenDBDiff.SqlServer.Schema.Options;

namespace OpenDBDiff.SqlServer.Ui;

public class SQLServerGenerator : IGenerator
{
    private readonly Generate generate;

    public event ProgressEventHandler.ProgressHandler OnProgress;

    public SQLServerGenerator(string connectionString, IOption option)
    {
        generate = new Generate()
        {
            ConnectionString = connectionString,
            Options = new SqlOption(option)
        };
        generate.OnProgress += new ProgressEventHandler.ProgressHandler(args =>
        {
            OnProgress?.Invoke(args);
        });
    }

    public int GetMaxValue() => Generate.MaxValue;

    public IDatabase Process() => generate.Process();
}
