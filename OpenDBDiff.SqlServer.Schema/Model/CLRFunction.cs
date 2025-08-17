using System.Collections.Generic;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class CLRFunction(ISchemaBase parent) : CLRCode(parent, ObjectType.CLRFunction, ScriptAction.AddFunction, ScriptAction.DropFunction)
{
    public List<Parameter> Parameters { get; set; } = [];

    public Parameter ReturnType { get; private set; } = new Parameter();

    public override string ToSql()
    {
        var sql = new StringBuilder();
        sql.Append($"CREATE FUNCTION {FullName}");
        var param = "";
        Parameters.ForEach(item => param += item.ToSql() + ",");
        if (!string.IsNullOrEmpty(param))
        {
            param = param.Substring(0, param.Length - 1);
            sql.AppendLine($" ({param})");
        }
        else
        {
            sql.AppendLine("()");
        }

        sql.Append($"RETURNS {ReturnType.ToSql()} ");
        sql.AppendLine($"WITH EXECUTE AS {AssemblyExecuteAs}");
        sql.AppendLine("AS");
        sql.AppendLine($"EXTERNAL NAME [{AssemblyName}].[{AssemblyClass}].[{AssemblyMethod}]");
        sql.AppendLine("GO");
        return sql.ToString(); ;
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
