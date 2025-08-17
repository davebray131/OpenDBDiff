using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Options;

public class SqlOptionScript : IOptionsContainer<bool>
{
    public SqlOptionScript()
    {
    }

    public SqlOptionScript(IOptionsContainer<bool> optionsContainer) => AlterObjectOnSchemaBinding = optionsContainer.GetOptions()["AlterObjectOnSchemaBinding"];

    public bool AlterObjectOnSchemaBinding { get; set; } = true;

    public IDictionary<string, bool> GetOptions() => new Dictionary<string, bool>() { { "AlterObjectOnSchemaBinding", AlterObjectOnSchemaBinding } };
}
