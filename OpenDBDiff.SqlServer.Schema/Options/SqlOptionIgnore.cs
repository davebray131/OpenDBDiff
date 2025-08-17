using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Options;

public class SqlOptionIgnore : IOptionsContainer<bool>
{
    public SqlOptionIgnore(bool defaultValue)
    {
        FilterPartitionFunction = true;
        FilterPartitionScheme = true;
        FilterIndexFilter = true;
        FilterIndex = true;
        FilterConstraintPK = true;
        FilterConstraintFK = true;
        FilterConstraintUK = true;
        FilterConstraintCheck = true;
        FilterIndexFillFactor = true;
        FilterIndexIncludeColumns = true;
        FilterIndexRowLock = true;
        FilterColumnOrder = true;
        FilterColumnIdentity = true;
        FilterColumnCollation = true;
        FilterNotForReplication = true;
        FilterUsers = true;
        FilterRoles = true;
        FilterCLRFunction = true;
        FilterCLRTrigger = true;
        FilterCLRUDT = true;
        FilterCLRStoredProcedure = true;
        FilterFullText = true;
        FilterFullTextPath = false;
        FilterTableLockEscalation = true;
        FilterTableChangeTracking = true;
        FilterConstraint = defaultValue;
        FilterFunction = defaultValue;
        FilterStoredProcedure = defaultValue;
        FilterView = defaultValue;
        FilterTable = defaultValue;
        FilterTableOption = defaultValue;
        FilterUserDataType = defaultValue;
        FilterTrigger = defaultValue;
        FilterSchema = defaultValue;
        FilterXMLSchema = defaultValue;
        FilterTableFileGroup = defaultValue;
        FilterExtendedProperties = defaultValue;
        FilterDDLTriggers = defaultValue;
        FilterSynonyms = defaultValue;
        FilterRules = defaultValue;
        FilterAssemblies = defaultValue;
    }

    public SqlOptionIgnore(IOptionsContainer<bool> optionsContainer)
    {
        var options = optionsContainer.GetOptions();
        FilterPartitionFunction = options["FilterPartitionFunction"];
        FilterPartitionScheme = options["FilterPartitionScheme"];
        FilterIndexFilter = options["FilterIndexFilter"];
        FilterIndex = options["FilterIndex"];
        FilterConstraintPK = options["FilterConstraintPK"];
        FilterConstraintFK = options["FilterConstraintFK"];
        FilterConstraintUK = options["FilterConstraintUK"];
        FilterConstraintCheck = options["FilterConstraintCheck"];
        FilterIndexFillFactor = options["FilterIndexFillFactor"];
        FilterIndexIncludeColumns = options["FilterIndexIncludeColumns"];
        FilterIndexRowLock = options["FilterIndexRowLock"];
        FilterColumnOrder = options["FilterColumnOrder"];
        FilterColumnIdentity = options["FilterColumnIdentity"];
        FilterColumnCollation = options["FilterColumnCollation"];
        FilterNotForReplication = options["FilterNotForReplication"];
        FilterUsers = options["FilterUsers"];
        FilterRoles = options["FilterRoles"];
        FilterCLRFunction = options["FilterCLRFunction"];
        FilterCLRTrigger = options["FilterCLRTrigger"];
        FilterCLRUDT = options["FilterCLRUDT"];
        FilterCLRStoredProcedure = options["FilterCLRStoredProcedure"];
        FilterFullText = options["FilterFullText"];
        FilterFullTextPath = options["FilterFullTextPath"];
        FilterTableLockEscalation = options["FilterTableLockEscalation"];
        FilterTableChangeTracking = options["FilterTableChangeTracking"];
        FilterConstraint = options["FilterConstraint"];
        FilterFunction = options["FilterFunction"];
        FilterStoredProcedure = options["FilterStoredProcedure"];
        FilterView = options["FilterView"];
        FilterTable = options["FilterTable"];
        FilterTableOption = options["FilterTableOption"];
        FilterUserDataType = options["FilterUserDataType"];
        FilterTrigger = options["FilterTrigger"];
        FilterSchema = options["FilterSchema"];
        FilterXMLSchema = options["FilterXMLSchema"];
        FilterTableFileGroup = options["FilterTableFileGroup"];
        FilterExtendedProperties = options["FilterExtendedProperties"];
        FilterDDLTriggers = options["FilterDDLTriggers"];
        FilterSynonyms = options["FilterSynonyms"];
        FilterRules = options["FilterRules"];
        FilterAssemblies = options["FilterAssemblies"];

    }

    public bool FilterTableChangeTracking { get; set; }

    public bool FilterTableLockEscalation { get; set; }

    public bool FilterFullTextPath { get; set; }

    public bool FilterFullText { get; set; }

    public bool FilterCLRStoredProcedure { get; set; }

    public bool FilterCLRUDT { get; set; }

    public bool FilterCLRTrigger { get; set; }

    public bool FilterCLRFunction { get; set; }

    public bool FilterRoles { get; set; }

    public bool FilterUsers { get; set; }

    public bool FilterNotForReplication { get; set; }

    public bool FilterColumnCollation { get; set; }

    public bool FilterColumnIdentity { get; set; }

    public bool FilterColumnOrder { get; set; }

    public bool FilterIndexRowLock { get; set; }

    public bool FilterIndexIncludeColumns { get; set; }

    public bool FilterIndexFillFactor { get; set; }

    public bool FilterAssemblies { get; set; }

    public bool FilterRules { get; set; }

    public bool FilterSynonyms { get; set; }

    public bool FilterDDLTriggers { get; set; }

    public bool FilterExtendedProperties { get; set; }

    public bool FilterTableFileGroup { get; set; }

    public bool FilterFunction { get; set; }

    public bool FilterStoredProcedure { get; set; }

    public bool FilterView { get; set; }

    public bool FilterTable { get; set; }

    public bool FilterTableOption { get; set; }

    public bool FilterUserDataType { get; set; }

    public bool FilterTrigger { get; set; }

    public bool FilterXMLSchema { get; set; }

    public bool FilterSchema { get; set; }

    public bool FilterConstraint { get; set; }

    public bool FilterConstraintCheck { get; set; }

    public bool FilterConstraintUK { get; set; }

    public bool FilterConstraintFK { get; set; }

    public bool FilterConstraintPK { get; set; }

    public bool FilterIndex { get; set; }

    public bool FilterIndexFilter { get; set; }

    public bool FilterPartitionScheme { get; set; }

    public bool FilterPartitionFunction { get; set; }

    public IDictionary<string, bool> GetOptions()
    {

        var options = new Dictionary<string, bool>
        {
            { "FilterPartitionFunction", FilterPartitionFunction },
            { "FilterPartitionScheme", FilterPartitionScheme },
            { "FilterIndexFilter", FilterIndexFilter },
            { "FilterIndex", FilterIndex },
            { "FilterConstraintPK", FilterConstraintPK },
            { "FilterConstraintFK", FilterConstraintFK },
            { "FilterConstraintUK", FilterConstraintUK },
            { "FilterConstraintCheck", FilterConstraintCheck },
            { "FilterIndexFillFactor", FilterIndexFillFactor },
            { "FilterIndexIncludeColumns", FilterIndexIncludeColumns },
            { "FilterIndexRowLock", FilterIndexRowLock },
            { "FilterColumnOrder", FilterColumnOrder },
            { "FilterColumnIdentity", FilterColumnIdentity },
            { "FilterColumnCollation", FilterColumnCollation },
            { "FilterNotForReplication", FilterNotForReplication },
            { "FilterUsers", FilterUsers },
            { "FilterRoles", FilterRoles },
            { "FilterCLRFunction", FilterCLRFunction },
            { "FilterCLRTrigger", FilterCLRTrigger },
            { "FilterCLRUDT", FilterCLRUDT },
            { "FilterCLRStoredProcedure", FilterCLRStoredProcedure },
            { "FilterFullText", FilterFullText },
            { "FilterFullTextPath", FilterFullTextPath },
            { "FilterTableLockEscalation", FilterTableLockEscalation },
            { "FilterTableChangeTracking", FilterTableChangeTracking },
            { "FilterConstraint", FilterConstraint },
            { "FilterFunction", FilterFunction },
            { "FilterStoredProcedure", FilterStoredProcedure },
            { "FilterView", FilterView },
            { "FilterTable", FilterTable },
            { "FilterTableOption", FilterTableOption },
            { "FilterUserDataType", FilterUserDataType },
            { "FilterTrigger", FilterTrigger },
            { "FilterSchema", FilterSchema },
            { "FilterXMLSchema", FilterXMLSchema },
            { "FilterTableFileGroup", FilterTableFileGroup },
            { "FilterExtendedProperties", FilterExtendedProperties },
            { "FilterDDLTriggers", FilterDDLTriggers },
            { "FilterSynonyms", FilterSynonyms },
            { "FilterRules", FilterRules },
            { "FilterAssemblies", FilterAssemblies }
        };
        return options;
    }
}
