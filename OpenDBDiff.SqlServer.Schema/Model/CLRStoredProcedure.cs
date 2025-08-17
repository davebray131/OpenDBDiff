using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class CLRStoredProcedure(ISchemaBase parent) : CLRCode(parent, ObjectType.CLRStoredProcedure, ScriptAction.AddStoredProcedure, ScriptAction.DropStoredProcedure)
{
    public List<Parameter> Parameters { get; set; } = [];

    public override string ToSql()
    {
        var sql = $"CREATE PROCEDURE {FullName}\r\n";
        var param = "";
        Parameters.ForEach(item => param += "\t" + item.ToSql() + ",\r\n");
        if (!string.IsNullOrEmpty(param))
        {
            param = param.Substring(0, param.Length - 3) + "\r\n";
        }

        sql += param;
        sql += $"WITH EXECUTE AS {AssemblyExecuteAs}\r\n";
        sql += "AS\r\n";
        sql += $"EXTERNAL NAME [{AssemblyName}].[{AssemblyClass}].[{AssemblyMethod}]\r\n";
        sql += "GO\r\n";
        return sql;
    }

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();

        if (HasState(ObjectStatus.Drop))
        {
            list.Add(Drop());
        }

        if (HasState(ObjectStatus.Create))
        {
            list.Add(Create());
        }

        if (Status == ObjectStatus.Alter)
        {
            list.AddRange(Rebuild());
        }
        list.AddRange(ExtendedProperties.ToSqlDiff());
        return list;
    }
}
