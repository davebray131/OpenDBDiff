using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateIndex
{
    private readonly Generate root;

    public GenerateIndex(Generate root) => this.root = root;

    public void Fill(Database database, string connectionString)
    {
        var indexid = 0;
        var parentId = 0;
        string type;
        ISchemaBase parent = null;
        root.RaiseOnReading(new ProgressEventArgs("Reading Index...", Constants.READING_INDEXES));
        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(IndexSQLCommand.Get(database.Info.Version, database.Info.Edition), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        Index item = null;
        while (reader.Read())
        {
            root.RaiseOnReadingOne(reader["Name"]);
            type = reader["ObjectType"].ToString().Trim();
            bool change;
            if (parentId != (int)reader["object_id"])
            {
                parentId = (int)reader["object_id"];
                parent = type.Equals("V") ? database.Views.Find(parentId) : database.Tables.Find(parentId);

                change = true;
            }
            else
            {
                change = false;
            }

            if (parent != null)
            {
                if (indexid != (int)reader["index_id"] || change)
                {
                    item = new Index(parent)
                    {
                        Name = reader["Name"].ToString(),
                        Owner = parent.Owner,
                        Type = (Index.IndexTypeEnum)(byte)reader["type"],
                        Id = (int)reader["index_id"],
                        IgnoreDupKey = (bool)reader["ignore_dup_key"],
                        IsAutoStatistics = (bool)reader["NoAutomaticRecomputation"],
                        IsDisabled = (bool)reader["is_disabled"],
                        IsPrimaryKey = (bool)reader["is_primary_key"],
                        IsUniqueKey = (bool)reader["is_unique"]
                    };
                    if (database.Options.Ignore.FilterIndexRowLock)
                    {
                        item.AllowPageLocks = (bool)reader["allow_page_locks"];
                        item.AllowRowLocks = (bool)reader["allow_row_locks"];
                    }
                    if (database.Options.Ignore.FilterIndexFillFactor)
                    {
                        item.FillFactor = (byte)reader["fill_factor"];
                        item.IsPadded = (bool)reader["is_padded"];
                    }
                    if (database.Options.Ignore.FilterTableFileGroup && (item.Type != Index.IndexTypeEnum.XML))
                    {
                        item.FileGroup = reader["FileGroup"].ToString();
                    }

                    if ((database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServer2008) && database.Options.Ignore.FilterIndexFilter)
                    {
                        item.FilterDefintion = reader["FilterDefinition"].ToString();
                    }
                    indexid = (int)reader["index_id"];
                    if (type.Equals("V"))
                    {
                        ((View)parent).Indexes.Add(item);
                    }
                    else
                    {
                        ((Table)parent).Indexes.Add(item);
                    }
                }
                var ccon = new IndexColumn(item.Parent)
                {
                    Name = reader["ColumnName"].ToString(),
                    IsIncluded = (bool)reader["is_included_column"],
                    Order = (bool)reader["is_descending_key"],
                    Id = (int)reader["column_id"],
                    KeyOrder = (byte)reader["key_ordinal"],
                    DataTypeId = (int)reader["user_type_id"]
                };
                if ((!ccon.IsIncluded) || (ccon.IsIncluded && database.Options.Ignore.FilterIndexIncludeColumns))
                {
                    item.Columns.Add(ccon);
                }
            }
        }
    }
}
