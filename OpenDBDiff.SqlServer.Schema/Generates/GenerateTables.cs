using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Errors;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;
using Constraint = OpenDBDiff.SqlServer.Schema.Model.Constraint;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateTables(Generate root)
{
    private int colIDIndex = -1;
    private int colNameIndex = -1;
    private int colFormulaIndex = -1;
    private int colIsPersistedIndex = -1;
    private int colIsComputedIndex = -1;
    private int colNullableIndex = -1;
    private int colXmlSchemaIndex = -1;
    private int colIs_xml_documentIndex = -1;
    private int colPrecisionIndex = -1;
    private int colScaleIndex = -1;
    private int colDataUserTypeIdIndex = -1;
    private int colIsUserDefinedTypeIndex = -1;
    private int colSizeIndex = -1;
    private int colHasIndexIndex = -1;
    private int colHasComputedFormulaIndex = -1;
    private int colIsRowGuidIndex = -1;
    private int colTypeIndex = -1;
    private int colOwnerType = -1;
    private int colis_sparseIndex = -1;
    private int colIs_FileStream = -1;
    private int colDefaultIdIndex = -1;
    private int colDefaultNameIndex = -1;
    private int colDefaultDefinitionIndex = -1;
    private int colrule_object_idIndex = -1;
    private int colIsIdentityReplIndex = -1;
    private int colCollationIndex = -1;
    private int colIsIdentityIndex = -1;
    private int colIdentSeedIndex = -1;
    private int colIdentIncrementIndex = -1;
    private int TableIdIndex = -1;
    private int TableNameIndex = -1;
    private int TableOwnerIndex = -1;
    private int TableHasChangeTracking = -1;
    private int TableHasChangeTrackingTrackColumn = -1;
    private int TableLockEscalation = -1;
    private int Text_In_Row_limitIndex = -1;
    private int HasClusteredIndexIndex = -1;
    private int large_value_types_out_of_rowIndex = -1;
    private int HasVarDecimalIndex = -1;
    private int FileGroupIndex = -1;
    private int FileGroupTextIndex = -1;
    private int FileGroupStreamIndex = -1;

    private void InitTableIndex(Database database, IDataRecord reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException(nameof(reader));
        }

        if (TableIdIndex == -1)
        {
            TableIdIndex = reader.GetOrdinal("TableId");
            TableNameIndex = reader.GetOrdinal("TableName");
            TableOwnerIndex = reader.GetOrdinal("TableOwner");
            Text_In_Row_limitIndex = reader.GetOrdinal("Text_In_Row_limit");
            HasClusteredIndexIndex = reader.GetOrdinal("HasClusteredIndex");
            large_value_types_out_of_rowIndex = reader.GetOrdinal("large_value_types_out_of_row");
            HasVarDecimalIndex = reader.GetOrdinal("HasVarDecimal");
            FileGroupIndex = reader.GetOrdinal("FileGroup");
            FileGroupTextIndex = reader.GetOrdinal("FileGroupText");
            if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
            {
                FileGroupStreamIndex = reader.GetOrdinal("FileGroupStream");
                TableHasChangeTracking = reader.GetOrdinal("HasChangeTracking");
                TableHasChangeTrackingTrackColumn = reader.GetOrdinal("HasChangeTrackingTrackColumn");
                TableLockEscalation = reader.GetOrdinal("lock_escalation_desc");
            }
        }
    }

    private void InitColIndex(Database database, IDataRecord reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException(nameof(reader));
        }
        if (colNameIndex == -1)
        {
            colIDIndex = reader.GetOrdinal("ID");
            colNameIndex = reader.GetOrdinal("Name");
            colFormulaIndex = reader.GetOrdinal("Formula");
            colIsPersistedIndex = reader.GetOrdinal("FormulaPersisted");
            colIsComputedIndex = reader.GetOrdinal("IsComputed");
            colNullableIndex = reader.GetOrdinal("IsNullable");
            colOwnerType = reader.GetOrdinal("OwnerType");
            colXmlSchemaIndex = reader.GetOrdinal("XmlSchema");
            colIs_xml_documentIndex = reader.GetOrdinal("Is_xml_document");
            colPrecisionIndex = reader.GetOrdinal("Precision");
            colScaleIndex = reader.GetOrdinal("Scale");
            colDataUserTypeIdIndex = reader.GetOrdinal("user_type_id");
            colIsUserDefinedTypeIndex = reader.GetOrdinal("is_user_defined");
            colSizeIndex = reader.GetOrdinal("Size");
            colHasIndexIndex = reader.GetOrdinal("HasIndex");
            colHasComputedFormulaIndex = reader.GetOrdinal("HasComputedFormula");
            colIsRowGuidIndex = reader.GetOrdinal("IsRowGuid");
            colTypeIndex = reader.GetOrdinal("Type");
            colDefaultIdIndex = reader.GetOrdinal("DefaultId");
            colDefaultNameIndex = reader.GetOrdinal("DefaultName");
            colDefaultDefinitionIndex = reader.GetOrdinal("DefaultDefinition");
            colrule_object_idIndex = reader.GetOrdinal("rule_object_id");
            colIsIdentityReplIndex = reader.GetOrdinal("IsIdentityRepl");
            colCollationIndex = reader.GetOrdinal("Collation");
            colIsIdentityIndex = reader.GetOrdinal("IsIdentity");
            colIdentSeedIndex = reader.GetOrdinal("IdentSeed");
            colIdentIncrementIndex = reader.GetOrdinal("IdentIncrement");
            if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
            {
                colis_sparseIndex = reader.GetOrdinal("is_sparse");
                colIs_FileStream = reader.GetOrdinal("is_filestream");
            }
        }
    }

    private void FillColumn<T>(ITable<T> table, SqlDataReader reader) where T : ISchemaBase
    {
        var database = (Database)table.Parent;

        InitColIndex(database, reader);
        var col = new Column((ISchemaBase)table)
        {
            Id = reader.GetInt32(colIDIndex)
        };
        if (database.Options.Ignore.FilterColumnOrder)
        {
            col.Position = table.Columns.Count + 1;
        }

        if (database.Options.Ignore.FilterColumnCollation)
        {
            col.Collation = reader.GetString(colCollationIndex);
        }

        if (database.Options.Ignore.FilterColumnIdentity)
        {
            col.IsIdentity = (bool)reader[colIsIdentityIndex];
            if (col.IsIdentity || col.IsIdentityForReplication)
            {
                col.IdentitySeed = !reader.IsDBNull(colIdentSeedIndex) ? (long)(decimal)reader[colIdentSeedIndex] : 1;

                col.IdentityIncrement = !reader.IsDBNull(colIdentIncrementIndex) ? (int)(decimal)reader[colIdentIncrementIndex] : 1;
            }
            if (database.Options.Ignore.FilterNotForReplication)
            {
                col.IsIdentityForReplication = (int)reader[colIsIdentityReplIndex] == 1;
            }
        }
        col.Name = reader.GetString(colNameIndex);
        col.Owner = table.Owner;
        col.ComputedFormula = reader.GetString(colFormulaIndex);
        col.IsPersisted = reader.GetBoolean(colIsPersistedIndex);
        col.IsComputed = reader.GetBoolean(colIsComputedIndex);
        col.IsNullable = reader.GetBoolean(colNullableIndex);
        col.XmlSchema = reader[colXmlSchemaIndex].ToString();
        col.IsXmlDocument = reader.GetBoolean(colIs_xml_documentIndex);
        col.Precision = reader.GetByte(colPrecisionIndex);
        col.Scale = reader.GetByte(colScaleIndex);
        col.DataUserTypeId = reader.GetInt32(colDataUserTypeIdIndex);
        col.IsUserDefinedType = reader.GetBoolean(colIsUserDefinedTypeIndex);
        if (!string.IsNullOrEmpty(reader[colSizeIndex].ToString()))
        {
            col.Size = reader.GetInt16(colSizeIndex);
        }

        col.HasIndexDependencies = reader.GetInt32(colHasIndexIndex) == 1;
        col.HasComputedDependencies = reader.GetInt32(colHasComputedFormulaIndex) == 1;
        col.IsRowGuid = reader.GetBoolean(colIsRowGuidIndex);
        col.Type = col.IsUserDefinedType
            ? $"[{reader.GetString(colOwnerType)}].[{reader.GetString(colTypeIndex)}]"
            : reader.GetString(colTypeIndex);

        if (((Database)table.Parent).Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
        {
            col.IsSparse = reader.GetBoolean(colis_sparseIndex);
            col.IsFileStream = reader.GetBoolean(colIs_FileStream);
        }
        if ((int)reader[colDefaultIdIndex] != 0)
        {
            col.DefaultConstraint = new ColumnConstraint(col)
            {
                Id = reader.GetInt32(colDefaultIdIndex),
                Owner = table.Owner,
                Name = reader.GetString(colDefaultNameIndex),
                Type = Constraint.ConstraintType.Default,
                Definition = reader.GetString(colDefaultDefinitionIndex)
            };
        }
        if (reader.GetInt32(colrule_object_idIndex) != 0)
        {
            col.Rule = ((Database)table.Parent).Rules.Find(reader.GetInt32(colrule_object_idIndex));
        }

        table.Columns.Add(col);
    }

    public void Fill(Database database, string connectionString, List<MessageLog> messages)
    {
        try
        {
            root.RaiseOnReading(new ProgressEventArgs("Reading tables...", Constants.READING_TABLES));
            FillTables(database, connectionString);
            if (database.Tables.Any() || database.TablesTypes.Any())
            {
                if (database.Options.Ignore.FilterConstraint)
                {
                    new GenerateConstraint(root).Fill(database, connectionString);
                }
            }
        }
        catch (Exception ex)
        {
            messages.Add(new MessageLog(ex.Message, ex.StackTrace, MessageLog.LogType.Error));
        }
    }

    private void FillTables(Database database, string connectionString)
    {
        int textInRow;
        bool largeValues;
        bool varDecimal;
        var lastObjectId = 0;
        var isTable = true;
        ISchemaBase item = null;

        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(TableSQLCommand.GetTableDetail(database.Info.Version, database.Info.Edition), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            InitTableIndex(database, reader);
            root.RaiseOnReadingOne(reader[TableNameIndex]);
            if (lastObjectId != reader.GetInt32(TableIdIndex))
            {
                lastObjectId = reader.GetInt32(TableIdIndex);
                isTable = reader["ObjectType"].ToString().Trim().Equals("U");
                if (isTable)
                {
                    item = new Table(database)
                    {
                        Id = reader.GetInt32(TableIdIndex),
                        Name = reader.GetString(TableNameIndex),
                        Owner = reader.GetString(TableOwnerIndex)
                    };
                    ((Table)item).HasClusteredIndex = reader.GetInt32(HasClusteredIndexIndex) == 1;
                    textInRow = reader.GetInt32(Text_In_Row_limitIndex);
                    largeValues = reader.GetBoolean(large_value_types_out_of_rowIndex);
                    varDecimal = reader.GetInt32(HasVarDecimalIndex) == 1;
                    if (database.Options.Ignore.FilterTableFileGroup)
                    {
                        ((Table)item).FileGroup = reader.GetString(FileGroupIndex);
                        ((Table)item).FileGroupText = reader.GetString(FileGroupTextIndex);
                        if (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008)
                        {
                            if (database.Options.Ignore.FilterTableChangeTracking)
                            {
                                ((Table)item).FileGroupStream = reader.GetString(FileGroupStreamIndex);
                                ((Table)item).HasChangeTracking = reader.GetInt32(TableHasChangeTracking) == 1;
                                ((Table)item).HasChangeTrackingTrackColumn = ((int)reader[TableHasChangeTrackingTrackColumn]) == 1;
                            }
                        }
                    }
                    if (database.Options.Ignore.FilterTableOption)
                    {
                        if (textInRow > 0)
                        {
                            ((Table)item).Options.Add(new TableOption("TextInRow", textInRow.ToString(CultureInfo.InvariantCulture), item));
                        }

                        if (largeValues)
                        {
                            ((Table)item).Options.Add(new TableOption("LargeValues", "1", item));
                        }

                        if (varDecimal)
                        {
                            ((Table)item).Options.Add(new TableOption("VarDecimal", "1", item));
                        }
                    }
                    if (database.Options.Ignore.FilterTableLockEscalation && (database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008))
                    {
                        ((Table)item).Options.Add(new TableOption("LockEscalation", (string)reader[TableLockEscalation], item));
                    }
                    else
                    {
                        ((Table)item).Options.Add(new TableOption("LockEscalation", "TABLE", item));
                    }

                    database.Tables.Add((Table)item);
                }
                else
                {
                    item = new TableType(database)
                    {
                        Id = (int)reader[TableIdIndex],
                        Name = reader.GetString(TableNameIndex),
                        Owner = reader.GetString(TableOwnerIndex)
                    };
                    database.TablesTypes.Add((TableType)item);
                }
            }
            if (isTable)
            {
                if (database.Options.Ignore.FilterTable)
                {
                    FillColumn((ITable<Table>)item, reader);
                }
            }
            else
            {
                if (database.Options.Ignore.FilterUserDataType)
                {
                    FillColumn((ITable<TableType>)item, reader);
                }
            }
        }
        //tables.ToSQL();
    }
}
