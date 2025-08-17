using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Generates.Util;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateConstraint(Generate root)
{

    #region Check Functions...
    public void FillCheck(Database database, string connectionString)
    {
        var parentId = 0;
        ISchemaBase table = null;

        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(ConstraintSQLCommand.GetCheck(database.Info.Version), conn);
        root.RaiseOnReading(new ProgressEventArgs("Reading constraint...", Constants.READING_CONSTRAINTS));
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        Constraint item = null;
        while (reader.Read())
        {
            root.RaiseOnReadingOne(reader["Name"]);
            if (parentId != (int)reader["parent_object_id"])
            {
                parentId = (int)reader["parent_object_id"];
                table = reader["ObjectType"].ToString().Trim().Equals("U") ? database.Tables.Find(parentId) : database.TablesTypes.Find(parentId);
            }
            if (table != null)
            {
                item = new Constraint(table)
                {
                    Id = (int)reader["id"],
                    Name = reader["Name"].ToString(),
                    Type = Constraint.ConstraintType.Check,
                    Definition = reader["Definition"].ToString(),
                    WithNoCheck = (bool)reader["WithCheck"],
                    IsDisabled = (bool)reader["is_disabled"],
                    Owner = reader["Owner"].ToString()
                };
                if (database.Options.Ignore.FilterNotForReplication)
                {
                    item.NotForReplication = (bool)reader["is_not_for_replication"];
                }

                if (reader["ObjectType"].ToString().Trim().Equals("U"))
                {
                    ((Table)table).Constraints.Add(item);
                }
                else
                {
                    ((TableType)table).Constraints.Add(item);
                }
            }
        }
    }

    #endregion

    #region ForeignKey Functions...

    private static string GetSQLForeignKey() => SQLQueries.SQLQueryFactory.Get("GetForeignKeys");

    private static void FillForeignKey(Database database, string connectionString)
    {
        var lastid = 0;
        var parentId = 0;
        Table table = null;

        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(GetSQLForeignKey(), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        Constraint con = null;
        while (reader.Read())
        {
            if (parentId != (int)reader["parent_object_id"])
            {
                parentId = (int)reader["parent_object_id"];
                table = database.Tables.Find(parentId);
            }

            if (table != null)
            {
                if (lastid != (int)reader["object_id"])
                {
                    con = new Constraint(table)
                    {
                        Id = (int)reader["object_id"],
                        Name = reader["Name"].ToString(),
                        Type = Constraint.ConstraintType.ForeignKey,
                        WithNoCheck = (bool)reader["is_not_trusted"],
                        RelationalTableFullName = "[" + reader["ReferenceOwner"].ToString() + "].[" + reader["TableRelationalName"].ToString() + "]",
                        RelationalTableId = (int)reader["TableRelationalId"],
                        Owner = reader["Owner"].ToString(),
                        IsDisabled = (bool)reader["is_disabled"],
                        OnDeleteCascade = (byte)reader["delete_referential_action"],
                        OnUpdateCascade = (byte)reader["update_referential_action"]
                    };
                    if (database.Options.Ignore.FilterNotForReplication)
                    {
                        con.NotForReplication = (bool)reader["is_not_for_replication"];
                    }

                    lastid = (int)reader["object_id"];
                    table.Constraints.Add(con);
                }
                var ccon = new ConstraintColumn(con)
                {
                    Name = reader["ColumnName"].ToString(),
                    ColumnRelationalName = reader["ColumnRelationalName"].ToString(),
                    ColumnRelationalId = (int)reader["ColumnRelationalId"],
                    Id = (int)reader["ColumnId"],
                    KeyOrder = con.Columns.Count,
                    ColumnRelationalDataTypeId = (int)reader["user_type_id"]
                };
                //table.DependenciesCount++;
                con.Columns.Add(ccon);
            }
        }
    }
    #endregion

    #region UniqueKey Functions...
    private static void FillUniqueKey(Database database, string connectionString)
    {
        var lastId = 0;
        var parentId = 0;
        ISchemaBase table = null;

        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(ConstraintSQLCommand.GetUniqueKey(database.Info.Version, database.Info.Edition), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        Constraint con = null;
        while (reader.Read())
        {
            bool change;
            if (parentId != (int)reader["ID"])
            {
                parentId = (int)reader["ID"];
                table = reader["ObjectType"].ToString().Trim().Equals("U") ? database.Tables.Find(parentId) : database.TablesTypes.Find(parentId);

                change = true;
            }
            else
            {
                change = false;
            }

            if (table != null)
            {
                if ((lastId != (int)reader["Index_id"]) || change)
                {
                    con = new Constraint(table, database.Options.Ignore.FilterIndex)
                    {
                        Name = reader["Name"].ToString(),
                        Owner = (string)reader["Owner"],
                        Id = (int)reader["Index_id"],
                        Type = Constraint.ConstraintType.Unique
                    };

                    if (database.Options.Ignore.FilterIndex)
                    {
                        con.Index.Id = (int)reader["Index_id"];
                        if (database.Options.Ignore.FilterIndexRowLock)
                        {
                            con.Index.AllowPageLocks = (bool)reader["allow_page_locks"];
                            con.Index.AllowRowLocks = (bool)reader["allow_row_locks"];
                        }

                        if (database.Options.Ignore.FilterIndexFillFactor)
                        {
                            con.Index.FillFactor = (byte)reader["fill_factor"];
                            con.Index.IsPadded = (bool)reader["is_padded"];
                        }
                        con.Index.IgnoreDupKey = (bool)reader["ignore_dup_key"];
                        con.Index.IsAutoStatistics = (bool)reader["ignore_dup_key"];
                        con.Index.IsDisabled = (bool)reader["is_disabled"];
                        con.Index.IsPrimaryKey = false;
                        con.Index.IsUniqueKey = true;
                        con.Index.Type = (Index.IndexTypeEnum)(byte)reader["type"];
                        con.Index.Name = con.Name;
                        if (database.Options.Ignore.FilterTableFileGroup)
                        {
                            con.Index.FileGroup = reader["FileGroup"].ToString();
                        }
                    }

                    lastId = (int)reader["Index_id"];
                    if (reader["ObjectType"].ToString().Trim().Equals("U"))
                    {
                        ((Table)table).Constraints.Add(con);
                    }
                    else
                    {
                        ((TableType)table).Constraints.Add(con);
                    }
                }
                var ccon = new ConstraintColumn(con)
                {
                    Name = reader["ColumnName"].ToString(),
                    IsIncluded = (bool)reader["is_included_column"],
                    Order = (bool)reader["is_descending_key"],
                    Id = (int)reader["column_id"],
                    DataTypeId = (int)reader["user_type_id"]
                };
                con.Columns.Add(ccon);
            }
        }
    }
    #endregion

    #region PrimaryKey Functions...
    private static void FillPrimaryKey(Database database, string connectionString)
    {
        var lastId = 0;
        var parentId = 0;
        ISchemaBase table = null;

        using var conn = new SqlConnection(connectionString);
        using var command = new SqlCommand(ConstraintSQLCommand.GetPrimaryKey(database.Info.Version, null), conn);
        conn.Open();
        command.CommandTimeout = 0;
        using var reader = command.ExecuteReader();
        Constraint con = null;
        while (reader.Read())
        {
            bool change;
            if (parentId != (int)reader["ID"])
            {
                parentId = (int)reader["ID"];
                table = reader["ObjectType"].ToString().Trim().Equals("U") ? database.Tables.Find(parentId) : database.TablesTypes.Find(parentId);

                change = true;
            }
            else
            {
                change = false;
            }

            if (table != null)
            {
                if ((lastId != (int)reader["Index_id"]) || change)
                {
                    con = new Constraint(table, database.Options.Ignore.FilterIndex)
                    {
                        Id = (int)reader["Index_id"],
                        Name = (string)reader["Name"],
                        Owner = (string)reader["Owner"],
                        Type = Constraint.ConstraintType.PrimaryKey
                    };

                    if (database.Options.Ignore.FilterIndex)
                    {
                        con.Index.Id = (int)reader["Index_id"];

                        if (database.Options.Ignore.FilterIndexRowLock)
                        {
                            con.Index.AllowPageLocks = (bool)reader["allow_page_locks"];
                            con.Index.AllowRowLocks = (bool)reader["allow_row_locks"];
                        }

                        if (database.Options.Ignore.FilterIndexFillFactor)
                        {
                            con.Index.FillFactor = (byte)reader["fill_factor"];
                            con.Index.IsPadded = (bool)reader["is_padded"];
                        }

                        con.Index.IgnoreDupKey = (bool)reader["ignore_dup_key"];
                        con.Index.IsAutoStatistics = (bool)reader["ignore_dup_key"];
                        con.Index.IsDisabled = (bool)reader["is_disabled"];
                        con.Index.IsPrimaryKey = true;
                        con.Index.IsUniqueKey = false;
                        con.Index.Type = (Index.IndexTypeEnum)(byte)reader["type"];
                        con.Index.Name = con.Name;
                        if (database.Options.Ignore.FilterTableFileGroup)
                        {
                            con.Index.FileGroup = reader["FileGroup"].ToString();
                        }
                    }

                    lastId = (int)reader["Index_id"];
                    if (reader["ObjectType"].ToString().Trim().Equals("U"))
                    {
                        ((Table)table).Constraints.Add(con);
                    }
                    else
                    {
                        ((TableType)table).Constraints.Add(con);
                    }
                }
                var ccon = new ConstraintColumn(con)
                {
                    Name = (string)reader["ColumnName"],
                    IsIncluded = (bool)reader["is_included_column"],
                    Order = (bool)reader["is_descending_key"],
                    KeyOrder = (byte)reader["key_ordinal"],
                    Id = (int)reader["column_id"],
                    DataTypeId = (int)reader["user_type_id"]
                };
                con.Columns.Add(ccon);
            }
        }
    }
    #endregion

    public void Fill(Database database, string connectionString)
    {
        if (database.Options.Ignore.FilterConstraintPK)
        {
            FillPrimaryKey(database, connectionString);
        }

        if (database.Options.Ignore.FilterConstraintFK)
        {
            FillForeignKey(database, connectionString);
        }

        if (database.Options.Ignore.FilterConstraintUK)
        {
            FillUniqueKey(database, connectionString);
        }

        if (database.Options.Ignore.FilterConstraintCheck)
        {
            FillCheck(database, connectionString);
        }
    }
}
