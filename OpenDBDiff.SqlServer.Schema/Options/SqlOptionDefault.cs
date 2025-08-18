using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Options;

public class SqlOptionDefault : IOptionsContainer<string>
{
    public SqlOptionDefault(IOptionsContainer<string> optionsContainer)
    {
        var options = optionsContainer.GetOptions();
        DefaultIntegerValue = options["defaultIntegerValue"];
        DefaultRealValue = options["defaultRealValue"];
        DefaultTextValue = options["defaultTextValue"];
        DefaultDateValue = options["defaultDateValue"];
        DefaultVariantValue = options["defaultVariantValue"];
        DefaultNTextValue = options["defaultNTextValue"];
        DefaultBlobValue = options["defaultBlobValue"];
        DefaultUniqueValue = options["defaultUniqueValue"];
        UseDefaultValueIfExists = bool.Parse(options["useDefaultValueIfExists"]);
        DefaultTime = options["defaultTime"];
        DefaultXml = options["defaultXml"];
    }

    public SqlOptionDefault()
    {
    }

    public string DefaultXml { get; set; } = string.Empty;

    public string DefaultTime { get; set; } = "00:00:00";

    public IDictionary<string, string> GetOptions()
    {
        var options = new Dictionary<string, string>
        {
            { "defaultIntegerValue", DefaultIntegerValue },
            { "defaultRealValue", DefaultRealValue },
            { "defaultTextValue", DefaultTextValue },
            { "defaultDateValue", DefaultDateValue },
            { "defaultVariantValue", DefaultVariantValue },
            { "defaultNTextValue", DefaultNTextValue },
            { "defaultBlobValue", DefaultBlobValue },
            { "defaultUniqueValue", DefaultUniqueValue },
            { "useDefaultValueIfExists", UseDefaultValueIfExists.ToString() },
            { "defaultTime", DefaultTime },
            { "defaultXml", DefaultXml }
        };
        return options;
    }
    /// <summary>
    /// Gets or sets a value indicating whether use default value if exists.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if use default value if exists; otherwise, <c>false</c>.
    /// </value>
    public bool UseDefaultValueIfExists { get; set; } = true;

    /// <summary>
    /// Gets or sets the default unique (uniqueidentifier) values.
    /// </summary>
    /// <value>The default unique value.</value>
    public string DefaultUniqueValue { get; set; } = "NEWID()";

    /// <summary>
    /// Gets or sets the default BLOB (varbinary,image, bynary) values.
    /// </summary>
    /// <value>The default BLOB value.</value>
    public string DefaultBlobValue { get; set; } = "0x";

    /// <summary>
    /// Gets or sets the default Unicode text (nvarchar,nchar,ntext) values.
    /// </summary>
    /// <value>The default N text value.</value>
    public string DefaultNTextValue { get; set; } = "N''";

    /// <summary>
    /// Gets or sets the default sql_variant values.
    /// </summary>
    /// <value>The default variant value.</value>
    public string DefaultVariantValue { get; set; } = "''";

    /// <summary>
    /// Gets or sets the default date (datetime,smalldatetime) values.
    /// </summary>
    /// <value>The default date value.</value>
    public string DefaultDateValue { get; set; } = "getdate()";

    /// <summary>
    /// Gets or sets the default text (varchar,char,text) values.
    /// </summary>
    /// <value>The default text value.</value>
    public string DefaultTextValue { get; set; } = "''";

    /// <summary>
    /// Gets or sets the default real (decimal,money,numeric,float) value.
    /// </summary>
    /// <value>The default real value.</value>
    public string DefaultRealValue { get; set; } = "0.0";

    /// <summary>
    /// Gets or sets the default integer (int, smallint, bigint, tinyint, bit) value.
    /// </summary>
    /// <value>The default integer value.</value>
    public string DefaultIntegerValue { get; set; } = "0";

}
