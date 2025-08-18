using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Attributes;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Table : SQLServerSchemaBase, IComparable<Table>, ITable<Table>
{
    private int dependenciesCount;
    private List<ISchemaBase> dependencies;
    private bool? hasFileStream;

    public Table(ISchemaBase parent)
        : base(parent, ObjectType.Table)
    {
        dependenciesCount = -1;
        Columns = new Columns<Table>(this);
        Constraints = new SchemaList<Constraint, Table>(this, ((Database)parent).AllObjects);
        Options = new SchemaList<TableOption, Table>(this);
        Triggers = new SchemaList<Trigger, Table>(this, ((Database)parent).AllObjects);
        CLRTriggers = new SchemaList<CLRTrigger, Table>(this, ((Database)parent).AllObjects);
        Indexes = new SchemaList<Index, Table>(this, ((Database)parent).AllObjects);
        Partitions = new SchemaList<TablePartition, Table>(this, ((Database)parent).AllObjects);
        FullTextIndex = new SchemaList<FullTextIndex, Table>(this);
    }

    public string CompressType { get; set; }

    public string FileGroupText { get; set; }

    public bool HasChangeDataCapture { get; set; }

    public bool HasChangeTrackingTrackColumn { get; set; }

    public bool HasChangeTracking { get; set; }

    public string FileGroupStream { get; set; }

    public bool HasClusteredIndex { get; set; }

    public string FileGroup { get; set; }

    public Table OriginalTable { get; set; }

    [SchemaNode("Constraints")]
    public SchemaList<Constraint, Table> Constraints { get; private set; }

    [SchemaNode("Indexes", "Index")]
    public SchemaList<Index, Table> Indexes { get; private set; }

    [SchemaNode("CLR Triggers")]
    public SchemaList<CLRTrigger, Table> CLRTriggers { get; private set; }

    [SchemaNode("Triggers")]
    public SchemaList<Trigger, Table> Triggers { get; private set; }

    public SchemaList<FullTextIndex, Table> FullTextIndex { get; private set; }

    public SchemaList<TablePartition, Table> Partitions { get; set; }

    public SchemaList<TableOption, Table> Options { get; set; }

    public bool HasIdentityColumn => Columns.Any(r => r.IsIdentity);

    public bool HasFileStream
    {
        get
        {
            hasFileStream ??= Columns.Any(r => r.IsFileStream);
            return hasFileStream.Value;
        }
    }

    public bool HasBlobColumn => Columns.Any(r => r.IsBLOB);

    public override int DependenciesCount
    {
        get
        {
            if (dependenciesCount == -1)
            {
                dependenciesCount = ((Database)Parent).Dependencies.DependenciesCount(Id, ObjectType.Constraint);
            }

            return dependenciesCount;
        }
    }

    #region IComparable<Table> Members

    public int CompareTo(Table other) => other == null
            ? throw new ArgumentNullException(nameof(other))
            : Status == other.Status ? DependenciesCount.CompareTo(other.DependenciesCount) : other.Status.CompareTo(Status);

    #endregion IComparable<Table> Members

    #region ITable<Table> Members


    [SchemaNode("Columns", "Column")]
    public Columns<Table> Columns { get; set; }

    #endregion ITable<Table> Members

    public override ISchemaBase Clone(ISchemaBase objectParent)
    {
        var table = new Table(objectParent)
        {
            Owner = Owner,
            Name = Name,
            Id = Id,
            Guid = Guid,
            Status = Status,
            FileGroup = FileGroup,
            FileGroupText = FileGroupText,
            FileGroupStream = FileGroupStream,
            HasClusteredIndex = HasClusteredIndex,
            HasChangeTracking = HasChangeTracking,
            HasChangeTrackingTrackColumn = HasChangeTrackingTrackColumn,
            HasChangeDataCapture = HasChangeDataCapture,
            dependenciesCount = DependenciesCount
        };
        table.Columns = Columns.Clone(table);
        table.Options = Options.Clone(table);
        table.CompressType = CompressType;
        table.Triggers = Triggers.Clone(table);
        table.Indexes = Indexes.Clone(table);
        table.Partitions = Partitions.Clone(table);
        table.Constraints = Constraints.Clone(table);
        return table;
    }

    public override string ToSql() => ToSql(true);

    public string ToSql(bool showFK)
    {
        List<Constraint.ConstraintType> constraintTypes = [Constraint.ConstraintType.PrimaryKey, Constraint.ConstraintType.Unique];
        if (showFK)
        {
            constraintTypes.Add(Constraint.ConstraintType.ForeignKey);
        }
        Database database = null;
        ISchemaBase current = this;
        while (database == null && current.Parent != null)
        {
            database = current.Parent as Database;
            current = current.Parent;
        }

        if (database == null)
        {
            return string.Empty;
        }

        var isAzure10 = database.Info.Version == DatabaseInfo.SQLServerVersion.SQLServerAzure10;

        var sql = new StringBuilder();

        if (Columns.Any())
        {
            sql.AppendLine($"CREATE TABLE {FullName}");
            sql.AppendLine("(");
            sql.Append(Columns.ToSql());
            if (Constraints.Any(r => constraintTypes.Contains(r.Type)))
            {
                List<string> keys = [];
                foreach (var itemType in constraintTypes)
                {
                    foreach (var item in Constraints.Where(c => !c.HasState(ObjectStatus.Drop) && c.Type == itemType))
                    {
                        keys.Add($"\t{item.ToSql()}");
                    }
                }
                sql.AppendLine(",");
                sql.AppendLine(string.Join(",", keys));
            }
            else
            {
                sql.AppendLine();
                if (!string.IsNullOrEmpty(CompressType))
                {
                    sql.AppendLine($"WITH (DATA_COMPRESSION = {CompressType})");
                }
            }
            sql.Append(")");

            if (!isAzure10)
            {
                if (!string.IsNullOrEmpty(FileGroup))
                {
                    sql.Append($" ON [{FileGroup}]");
                }

                if (!string.IsNullOrEmpty(FileGroupText))
                {
                    if (HasBlobColumn)
                    {
                        sql.Append($" TEXTIMAGE_ON [{FileGroupText}]");
                    }
                }
                if ((!string.IsNullOrEmpty(FileGroupStream)) && HasFileStream)
                {
                    sql.Append($" FILESTREAM_ON [{FileGroupStream}]");
                }
            }
            sql.AppendLine();
            sql.AppendLine("GO");

            foreach (var item in Constraints.Where(r => r.Type == Constraint.ConstraintType.Check))
            {
                sql.AppendLine(item.ToSqlAdd());
            }

            if (HasChangeTracking)
            {
                sql.Append(ToSqlChangeTracking());
            }

            sql.Append(Indexes.ToSql());
            sql.Append(FullTextIndex.ToSql());
            sql.Append(Options.ToSql());
            sql.Append(Triggers.ToSql());
        }
        return sql.ToString();
    }

    private string ToSqlChangeTracking()
    {
        var sql = new StringBuilder();
        if (HasChangeTracking)
        {
            sql.Append($"ALTER TABLE {FullName} ENABLE CHANGE_TRACKING");
            if (HasChangeTrackingTrackColumn)
            {
                sql.Append(" WITH(TRACK_COLUMNS_UPDATED = ON)");
            }
        }
        else
        {
            sql.Append($"ALTER TABLE {FullName} DISABLE CHANGE_TRACKING");
        }

        return sql.Append("\r\nGO\r\n").ToString();
    }

    public override string ToSqlAdd() => ToSql();

    public override string ToSqlDrop() => $"DROP TABLE {FullName}\r\nGO\r\n";

    /*
            private SQLScriptList BuildSQLFileGroup()
            {
                var listDiff = new SQLScriptList();

                bool found = false;
                Index clustered = Indexes.Find(item => item.Type == Index.IndexTypeEnum.Clustered);
                if (clustered == null)
                {
                    foreach (Constraint cons in Constraints)
                    {
                        if (cons.Index.Type == Index.IndexTypeEnum.Clustered)
                        {
                            listDiff.Add(cons.ToSqlDrop(FileGroup), dependenciesCount, ScripActionType.DropConstraint);
                            listDiff.Add(cons.ToSqlAdd(), dependenciesCount, ScripActionType.AddConstraint);
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        Status = ObjectStatusType.RebuildStatus;
                        listDiff = ToSqlDiff();
                    }
                }
                else
                {
                    listDiff.Add(clustered.ToSqlDrop(FileGroup), dependenciesCount, ScripActionType.DropIndex);
                    listDiff.Add(clustered.ToSqlAdd(), dependenciesCount, ScripActionType.AddIndex);
                }
                return listDiff;
            }
    */

    public override SQLScriptList ToSqlDiff(ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status != ObjectStatus.Original)
        {
            if (((Database)Parent).Options.Ignore.FilterTable)
            {
                RootParent.ActionMessage.Add(this);
            }
        }

        if (Status == ObjectStatus.Drop)
        {
            if (((Database)Parent).Options.Ignore.FilterTable)
            {
                listDiff.Add(ToSqlDrop(), dependenciesCount, ScriptAction.DropTable);
                listDiff.AddRange(ToSQLDropFKBelow());
            }
        }
        if (Status == ObjectStatus.Create)
        {
            var sql = new StringBuilder();
            Constraints.ForEach(item =>
            {
                if (item.Type == Constraint.ConstraintType.ForeignKey)
                {
                    sql.AppendLine(item.ToSqlAdd());
                }
            });
            listDiff.Add(ToSql(false), dependenciesCount, ScriptAction.AddTable);
            listDiff.Add(sql.ToString(), dependenciesCount, ScriptAction.AddConstraintFK);
        }
        if (HasState(ObjectStatus.RebuildDependencies))
        {
            GenerateDependencies();
            listDiff.AddRange(ToSQLDropDependencies());
            listDiff.AddRange(Columns.ToSqlDiff(schemas));
            listDiff.AddRange(ToSQLCreateDependencies());
            listDiff.AddRange(Constraints.ToSqlDiff());
            listDiff.AddRange(Indexes.ToSqlDiff());
            listDiff.AddRange(Options.ToSqlDiff());
            listDiff.AddRange(Triggers.ToSqlDiff());
            listDiff.AddRange(CLRTriggers.ToSqlDiff());
            listDiff.AddRange(FullTextIndex.ToSqlDiff());
        }
        if (HasState(ObjectStatus.Alter))
        {
            listDiff.AddRange(Columns.ToSqlDiff(schemas));
            listDiff.AddRange(Constraints.ToSqlDiff());
            listDiff.AddRange(Indexes.ToSqlDiff());
            listDiff.AddRange(Options.ToSqlDiff());
            listDiff.AddRange(Triggers.ToSqlDiff());
            listDiff.AddRange(CLRTriggers.ToSqlDiff());
            listDiff.AddRange(FullTextIndex.ToSqlDiff());
        }
        if (HasState(ObjectStatus.Rebuild))
        {
            GenerateDependencies();
            listDiff.AddRange(ToSQLRebuild());
            listDiff.AddRange(Columns.ToSqlDiff());
            listDiff.AddRange(Constraints.ToSqlDiff());
            listDiff.AddRange(Indexes.ToSqlDiff());
            listDiff.AddRange(Options.ToSqlDiff());
            //Como recrea la tabla, solo pone los nuevos triggers, por eso va ToSQL y no ToSQLDiff
            listDiff.Add(Triggers.ToSql(), dependenciesCount, ScriptAction.AddTrigger);
            listDiff.Add(CLRTriggers.ToSql(), dependenciesCount, ScriptAction.AddTrigger);
            listDiff.AddRange(FullTextIndex.ToSqlDiff());
        }
        if (HasState(ObjectStatus.Disabled))
        {
            listDiff.Add(ToSqlChangeTracking(), 0, ScriptAction.AlterTableChangeTracking);
        }
        return listDiff;
    }

    private string ToSQLTableRebuild()
    {
        var sql = new StringBuilder();
        var tempTable = $"Temp{Name}";
        var IsIdentityNew = false;

        var columnNamesStringBuilder = new StringBuilder();
        var valuesStringBuilder = new StringBuilder();

        foreach (var column in Columns)
        {
            if (column.Status != ObjectStatus.Drop &&
                !(column.Status == ObjectStatus.Create && column.IsNullable) &&
                !column.IsComputed && !column.Type.ToLower().Equals("timestamp"))
            {
                /*Si la nueva columna a agregar es XML, no se inserta ese campo y debe ir a la coleccion de Warnings*/
                /*Si la nueva columna a agregar es Identity, tampoco se debe insertar explicitamente*/
                if (
                    !(column.Status == ObjectStatus.Create &&
                      (column.Type.ToLower().Equals("xml") || column.IsIdentity)))
                {
                    columnNamesStringBuilder.Append("[");
                    columnNamesStringBuilder.Append(column.Name);
                    columnNamesStringBuilder.Append("],");

                    if (column.HasToForceValue)
                    {
                        if (column.HasState(ObjectStatus.Update))
                        {
                            valuesStringBuilder.Append("ISNULL([");
                            valuesStringBuilder.Append(column.Name);
                            valuesStringBuilder.Append("],");
                            valuesStringBuilder.Append(column.DefaultForceValue);
                            valuesStringBuilder.Append("),");
                        }
                        else
                        {
                            valuesStringBuilder.Append(column.DefaultForceValue);
                            valuesStringBuilder.Append(",");
                        }
                    }
                    else
                    {
                        valuesStringBuilder.Append("[");
                        valuesStringBuilder.Append(column.Name);
                        valuesStringBuilder.Append("],");
                    }
                }
                else
                {
                    if (column.IsIdentity)
                    {
                        IsIdentityNew = true;
                    }
                }
            }
        }

        if (columnNamesStringBuilder.Length > 0)
        {
            var listColumns = columnNamesStringBuilder.ToString(0, columnNamesStringBuilder.Length - 1);
            var listValues = valuesStringBuilder.ToString(0, valuesStringBuilder.Length - 1);
            sql.AppendLine(ToSQLTemp(tempTable));
            if (HasIdentityColumn && !IsIdentityNew)
            {
                sql.AppendLine($"SET IDENTITY_INSERT [{Owner}].[{tempTable}] ON");
            }

            sql.AppendLine($"INSERT INTO [{Owner}].[{tempTable}] ({listColumns}) SELECT {listValues} FROM {FullName}");
            if (HasIdentityColumn && !IsIdentityNew)
            {
                sql.AppendLine($"SET IDENTITY_INSERT [{Owner}].[{tempTable}] OFF\r\nGO\r\n");
            }

            sql.AppendLine($"DROP TABLE {FullName}\r\nGO");

            if (HasFileStream)
            {
                Constraints.ForEach(item =>
                {
                    if (item.Type == Constraint.ConstraintType.Unique &&
                        item.Status != ObjectStatus.Drop)
                    {
                        sql.AppendLine($"EXEC sp_rename N'[{Owner}].[Temp_XX_{item.Name}]',N'{item.Name}', 'OBJECT'\r\nGO");
                    }
                });
            }
            sql.AppendLine($"EXEC sp_rename N'[{Owner}].[{tempTable}]',N'{Name}', 'OBJECT'\r\nGO\r\n");
            sql.Append(OriginalTable.Options.ToSql());
        }
        else
        {
            return string.Empty;
        }

        return sql.ToString();
    }

    private SQLScriptList ToSQLRebuild()
    {
        var listDiff = new SQLScriptList();
        listDiff.AddRange(ToSQLDropDependencies());
        listDiff.Add(ToSQLTableRebuild(), dependenciesCount, ScriptAction.RebuildTable);
        listDiff.AddRange(ToSQLCreateDependencies());
        return listDiff;
    }

    private string ToSQLTemp(string TableName)
    {
        var sql = new StringBuilder();

        // Drop constraints first, to avoid duplicate constraints created in temp table
        foreach (var column in Columns.Where(c => !string.IsNullOrWhiteSpace(c.DefaultConstraint?.Name)))
        {
            sql.Append($"ALTER TABLE {FullName} DROP CONSTRAINT [{column.DefaultConstraint.Name}]\r\n");
        }

        if (sql.Length > 0)
        {
            sql.AppendLine();
        }

        sql.AppendLine($"CREATE TABLE [{Owner}].[{TableName}]");
        sql.AppendLine("(");

        Columns.Sort();

        for (var index = 0; index < Columns.Count; index++)
        {
            if (Columns[index].Status != ObjectStatus.Drop)
            {
                sql.Append("\t" + Columns[index].ToSql(true));
                if (index != Columns.Count - 1)
                {
                    sql.Append(",");
                }

                sql.AppendLine();
            }
        }
        if (HasFileStream)
        {
            sql.Remove(sql.Length - 2, 2);
            //sql = new StringBuilder(sql.ToString(0, sql.Length - 2));
            sql.AppendLine(",");
            Constraints.ForEach(item =>
            {
                if (item.Type == Constraint.ConstraintType.Unique &&
                    item.Status != ObjectStatus.Drop)
                {
                    item.Name = $"Temp_XX_{item.Name}";
                    sql.AppendLine($"\t{item.ToSql()},");
                    item.SetWasInsertInDiffList(ScriptAction.AddConstraint);
                    item.Name = item.Name.Substring(8, item.Name.Length - 8);
                }
            });
            sql.Remove(sql.Length - 3, 3);
            sql.AppendLine();
            //sql = new StringBuilder(sql.ToString(0, sql.Length - 3)).AppendLine();
        }
        else
        {
            sql.AppendLine();
            if (!string.IsNullOrEmpty(CompressType))
            {
                sql.AppendLine($"WITH (DATA_COMPRESSION = {CompressType})");
            }
        }
        sql.Append(")");

        if (!string.IsNullOrEmpty(FileGroup))
        {
            sql.Append($" ON [{FileGroup}]");
        }

        if (!string.IsNullOrEmpty(FileGroupText) && HasBlobColumn)
        {
            sql.Append($" TEXTIMAGE_ON [{FileGroupText}]");
        }

        if (!string.IsNullOrEmpty(FileGroupStream) && HasFileStream)
        {
            sql.Append($" FILESTREAM_ON [{FileGroupStream}]");
        }

        sql.AppendLine();
        sql.AppendLine("GO");

        return sql.ToString();
    }

    private void GenerateDependencies()
    {
        List<ISchemaBase> myDependencies;
        /*Si el estado es AlterRebuildDependeciesStatus, busca las dependencias solamente en las columnas que fueron modificadas*/
        if (Status == ObjectStatus.RebuildDependencies)
        {
            myDependencies = [];
            for (var ic = 0; ic < Columns.Count; ic++)
            {
                if ((Columns[ic].Status == ObjectStatus.RebuildDependencies) ||
                    (Columns[ic].Status == ObjectStatus.Alter))
                {
                    myDependencies.AddRange(((Database)Parent).Dependencies.Find(Id, 0, Columns[ic].DataUserTypeId));
                }
            }
            /*Si no encuentra ninguna, toma todas las de la tabla*/
            if (myDependencies.Count == 0)
            {
                myDependencies.AddRange(((Database)Parent).Dependencies.Find(Id));
            }
        }
        else
        {
            myDependencies = ((Database)Parent).Dependencies.Find(Id);
        }

        dependencies = [];
        for (var j = 0; j < myDependencies.Count; j++)
        {
            ISchemaBase item = null;
            if (myDependencies[j].ObjectType == ObjectType.Index)
            {
                item = Indexes[myDependencies[j].FullName];
            }

            if (myDependencies[j].ObjectType == ObjectType.Constraint)
            {
                item =
                    ((Database)Parent).Tables[myDependencies[j].Parent.FullName].Constraints[
                        myDependencies[j].FullName];
            }

            if (myDependencies[j].ObjectType == ObjectType.Default)
            {
                item = Columns[myDependencies[j].FullName].DefaultConstraint;
            }

            if (myDependencies[j].ObjectType == ObjectType.View)
            {
                item = ((Database)Parent).Views[myDependencies[j].FullName];
            }

            if (myDependencies[j].ObjectType == ObjectType.Function)
            {
                item = ((Database)Parent).Functions[myDependencies[j].FullName];
            }

            if (item != null)
            {
                dependencies.Add(item);
            }
        }
    }

    /// <summary>
    /// Genera una lista de FK que deben ser eliminadas previamente a la eliminacion de la tablas.
    /// Esto pasa porque para poder eliminar una tabla, hay que eliminar antes todas las constraints asociadas.
    /// </summary>
    private SQLScriptList ToSQLDropFKBelow()
    {
        var listDiff = new SQLScriptList();
        Constraints.ForEach(constraint =>
        {
            if ((constraint.Type == Constraint.ConstraintType.ForeignKey) &&
                (((Table)constraint.Parent).DependenciesCount <= DependenciesCount))
            {
                /*Si la FK pertenece a la misma tabla, no se debe explicitar el DROP CONSTRAINT antes de hacer el DROP TABLE*/
                if (constraint.Parent.Id != constraint.RelationalTableId)
                {
                    listDiff.Add(constraint.Drop());
                }
            }
        });
        return listDiff;
    }

    /// <summary>
    /// Genera una lista de script de DROPS de todas los constraints dependientes de la tabla.
    /// Se usa cuando se quiere reconstruir una tabla y todos sus objectos dependientes.
    /// </summary>
    private SQLScriptList ToSQLDropDependencies()
    {
        var addDependency = true;
        var listDiff = new SQLScriptList();
        //Se buscan todas las table constraints.
        for (var index = 0; index < dependencies.Count; index++)
        {
            if ((dependencies[index].Status == ObjectStatus.Original) ||
                (dependencies[index].Status == ObjectStatus.Drop))
            {
                addDependency = true;
                if (dependencies[index].ObjectType == ObjectType.Constraint)
                {
                    if ((((Constraint)dependencies[index]).Type == Constraint.ConstraintType.Unique) &&
                        (HasFileStream || OriginalTable.HasFileStream))
                    {
                        addDependency = false;
                    }

                    if ((((Constraint)dependencies[index]).Type != Constraint.ConstraintType.ForeignKey) &&
                        (dependencies[index].Status == ObjectStatus.Drop))
                    {
                        addDependency = false;
                    }
                }
                if (addDependency)
                {
                    listDiff.Add(dependencies[index].Drop());
                }
            }
        }
        //Se buscan todas las columns constraints.
        Columns.ForEach(column =>
                            {
                                if (column.DefaultConstraint != null)
                                {
                                    if (((column.DefaultConstraint.Status == ObjectStatus.Original) ||
                                         (column.DefaultConstraint.Status == ObjectStatus.Drop) ||
                                         (column.DefaultConstraint.Status == ObjectStatus.Alter)) &&
                                        (column.Status != ObjectStatus.Create))
                                    {
                                        listDiff.Add(column.DefaultConstraint.Drop());
                                    }
                                }
                            });
        return listDiff;
    }

    private SQLScriptList ToSQLCreateDependencies()
    {
        bool addDependency;
        var listDiff = new SQLScriptList();

        //Las constraints de deben recorrer en el orden inverso.
        for (var index = dependencies.Count - 1; index >= 0; index--)
        {
            if ((dependencies[index].Status == ObjectStatus.Original) &&
                (dependencies[index].Parent.Status != ObjectStatus.Drop))
            {
                addDependency = true;
                if (dependencies[index].ObjectType == ObjectType.Constraint)
                {
                    if ((((Constraint)dependencies[index]).Type == Constraint.ConstraintType.Unique) &&
                        HasFileStream)
                    {
                        addDependency = false;
                    }
                }
                if (addDependency)
                {
                    listDiff.Add(dependencies[index].Create());
                }
            }
        }
        //Se buscan todas las columns constraints.
        for (var index = Columns.Count - 1; index >= 0; index--)
        {
            if (Columns[index].DefaultConstraint != null)
            {
                if (Columns[index].DefaultConstraint.CanCreate &&
                    (Columns.Parent.Status != ObjectStatus.Rebuild))
                {
                    listDiff.Add(Columns[index].DefaultConstraint.Create());
                }
            }
        }
        return listDiff;
    }

    /// <summary>
    /// Compara dos tablas y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool CompareFileGroup(Table origin, Table destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (!string.IsNullOrEmpty(destination.FileGroup) && (!string.IsNullOrEmpty(origin.FileGroup)))
        {
            if (!destination.FileGroup.Equals(origin.FileGroup))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Compara dos tablas y devuelve true si son iguales, caso contrario, devuelve false.
    /// </summary>
    public static bool CompareFileGroupText(Table origin, Table destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (origin == null)
        {
            throw new ArgumentNullException(nameof(origin));
        }

        if (!string.IsNullOrEmpty(destination.FileGroupText) && (!string.IsNullOrEmpty(origin.FileGroupText)))
        {
            if (!destination.FileGroupText.Equals(origin.FileGroupText))
            {
                return false;
            }
        }

        return true;
    }
}
