using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class PartitionFunction : SQLServerSchemaBase
{
    private const int IS_STRING = 0;
    private const int IS_UNIQUE = 1;
    private const int IS_DATE = 2;
    private const int IS_NUMERIC = 3;

    public PartitionFunction(ISchemaBase parent)
        : base(parent, ObjectType.PartitionFunction) => Values = [];

    public new PartitionFunction Clone(ISchemaBase parent)
    {
        var item = new PartitionFunction(parent)
        {
            Id = this.Id,
            IsBoundaryRight = this.IsBoundaryRight,
            Name = this.Name,
            Precision = this.Precision,
            Scale = this.Scale,
            Size = this.Size,
            Type = this.Type
        };
        this.Values.ForEach(value => { item.Values.Add(value); });
        return item;
    }

    public List<string> Values { get; set; }

    public PartitionFunction Old { get; set; }

    public int Precision { get; set; }

    public int Scale { get; set; }

    public int Size { get; set; }

    public bool IsBoundaryRight { get; set; }

    public string Type { get; set; }

    private int ValueItem(string typeName)
    {
        return typeName.Equals("nchar") || typeName.Equals("nvarchar") || typeName.Equals("varchar") || typeName.Equals("char")
            ? IS_STRING
            : typeName.Equals("uniqueidentifier")
            ? IS_UNIQUE
            : typeName.Equals("datetime") || typeName.Equals("smalldatetime") || typeName.Equals("datetime2") || typeName.Equals("time") || typeName.Equals("datetimeoffset")
            ? IS_DATE
            : typeName.Equals("numeric") || typeName.Equals("decimal") || typeName.Equals("float") || typeName.Equals("real") || typeName.Equals("money") || typeName.Equals("smallmoney")
            ? IS_NUMERIC
            : IS_NUMERIC;
    }

    public override string ToSql()
    {
        var sqltype = Type;

        if (Type.Equals("binary") || Type.Equals("varbinary") || Type.Equals("varchar") || Type.Equals("char") || Type.Equals("nchar") || Type.Equals("nvarchar"))
        {
            if (Type.Equals("nchar") || Type.Equals("nvarchar"))
            {
                sqltype += " (" + (Size / 2).ToString(CultureInfo.InvariantCulture) + ")";
            }
            else
            {
                sqltype += " (" + Size.ToString(CultureInfo.InvariantCulture) + ")";
            }
        }
        if (Type.Equals("numeric") || Type.Equals("decimal"))
        {
            sqltype += " (" + Precision.ToString(CultureInfo.InvariantCulture) + "," + Scale.ToString(CultureInfo.InvariantCulture) + ")";
        }

        if (((Database)Parent).Info.Version >= DatabaseInfo.SQLServerVersion.SQLServer2008)
        {
            if (Type.Equals("datetime2") || Type.Equals("datetimeoffset") || Type.Equals("time"))
            {
                sqltype += "(" + Scale.ToString(CultureInfo.InvariantCulture) + ")";
            }
        }

        var sql = "CREATE PARTITION FUNCTION [" + Name + "](" + sqltype + ") AS RANGE\r\n ";
        if (IsBoundaryRight)
        {
            sql += "RIGHT";
        }
        else
        {
            sql += "LEFT";
        }

        sql += " FOR VALUES (";

        var sqlvalues = "";
        var valueType = ValueItem(Type);

        if (valueType == IS_STRING)
        {
            Values.ForEach(item => { sqlvalues += "N'" + item + "',"; });
        }
        else
            if (valueType == IS_DATE)
        {
            Values.ForEach(item => { sqlvalues += "'" + DateTime.Parse(item).ToString("yyyyMMdd HH:mm:ss.fff") + "',"; });
        }
        else
                if (valueType == IS_UNIQUE)
        {
            Values.ForEach(item => { sqlvalues += "'{" + item + "}',"; });
        }
        else
                    if (valueType == IS_NUMERIC)
        {
            Values.ForEach(item => { sqlvalues += item.Replace(",", ".") + ","; });
        }
        else
        {
            Values.ForEach(item => { sqlvalues += item + ","; });
        }

        sql += sqlvalues.Substring(0, sqlvalues.Length - 1) + ")";

        return sql + "\r\nGO\r\n";
    }

    public override string ToSqlDrop() => $"DROP PARTITION FUNCTION [{Name}]\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    public string ToSqlAlter()
    {
        var sqlFinal = new StringBuilder();
        var sql = "ALTER PARTITION FUNCTION [" + Name + "]()\r\n";
        string sqlMerge;
        string sqlSplit;
        var items = Old.Values.Except<string>(this.Values);
        var valueType = ValueItem(Type);
        foreach (var item in items)
        {
            sqlMerge = "MERGE RANGE (";
            if (valueType == IS_STRING)
            {
                sqlMerge += "N'" + item + "'";
            }
            else
                if (valueType == IS_DATE)
            {
                sqlMerge += "'" + DateTime.Parse(item).ToString("yyyyMMdd HH:mm:ss.fff") + "'";
            }
            else
                    if (valueType == IS_UNIQUE)
            {
                sqlMerge += "'{" + item + "}'";
            }
            else
                        if (valueType == IS_NUMERIC)
            {
                sqlMerge += item.Replace(",", ".");
            }
            else
            {
                sqlMerge += item;
            }

            _ = sqlFinal.Append(sql + sqlMerge + ")\r\nGO\r\n");
        }
        var items2 = this.Values.Except<string>(this.Old.Values);
        foreach (var item in items2)
        {
            sqlSplit = "SPLIT RANGE (";
            if (valueType == IS_STRING)
            {
                sqlSplit += "N'" + item + "'";
            }
            else
                if (valueType == IS_DATE)
            {
                sqlSplit += "'" + DateTime.Parse(item).ToString("yyyyMMdd HH:mm:ss.fff") + "'";
            }
            else
                    if (valueType == IS_UNIQUE)
            {
                sqlSplit += "'{" + item + "}'";
            }
            else
                        if (valueType == IS_NUMERIC)
            {
                sqlSplit += item.Replace(",", ".");
            }
            else
            {
                sqlSplit += item;
            }

            _ = sqlFinal.Append(sql + sqlSplit + ")\r\nGO\r\n");
        }
        return sqlFinal.ToString();
    }

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropPartitionFunction);
        }
        if (this.Status == ObjectStatus.Rebuild)
        {
            listDiff.Add(ToSqlDrop() + ToSqlAdd(), 0, ScriptAction.AlterPartitionFunction);
        }
        if (this.Status == ObjectStatus.Alter)
        {
            listDiff.Add(ToSqlAlter(), 0, ScriptAction.AlterPartitionFunction);
        }

        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSqlAdd(), 0, ScriptAction.AddPartitionFunction);
        }
        return listDiff;
    }

    public static bool Compare(PartitionFunction origin, PartitionFunction destination)
    {
        if (destination == null)
        {
            throw new ArgumentNullException("destination");
        }

        return origin == null
            ? throw new ArgumentNullException("origin")
            : origin.Type.Equals(destination.Type) && origin.Size == destination.Size && origin.Precision == destination.Precision && origin.Scale == destination.Scale && origin.IsBoundaryRight == destination.IsBoundaryRight;
    }

    public static bool CompareValues(PartitionFunction origin, PartitionFunction destination)
    {
        return destination == null
            ? throw new ArgumentNullException("destination")
            : origin == null
            ? throw new ArgumentNullException("origin")
            : origin.Values.Count == destination.Values.Count && origin.Values.Except(destination.Values).ToList().Count == 0;
    }
}
