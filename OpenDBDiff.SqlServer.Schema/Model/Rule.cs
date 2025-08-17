using System.Collections;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Rule(ISchemaBase parent) : Code(parent, ObjectType.Rule, ScriptAction.AddRule, ScriptAction.DropRule)
{
    public new Rule Clone(ISchemaBase parent) => new(parent)
    {
        Id = Id,
        Name = Name,
        Owner = Owner,
        Text = Text,
        Guid = Guid
    };

    public string ToSQLAddBind()
    {
        var sql = Parent.ObjectType == ObjectType.Column
            ? $"EXEC sp_bindrule N'{Name}', N'[{Parent.Parent.Name}].[{Parent.Name}]','futureonly'\r\nGO\r\n"
            : $"EXEC sp_bindrule N'{Name}', N'{Parent.Name}','futureonly'\r\nGO\r\n";
        return sql;
    }

    public string ToSQLAddUnBind() => Parent.ObjectType == ObjectType.Column
            ? $"EXEC sp_unbindrule @objname=N'[{Parent.Parent.Name}].[{Parent.Name}]'\r\nGO\r\n"
            : $"EXEC sp_unbindrule @objname=N'{Parent.Name}'\r\nGO\r\n";

    private SQLScriptList ToSQLUnBindAll()
    {
        var listDiff = new SQLScriptList();
        Hashtable items = [];
        var useDataTypes = ((Database)Parent).UserTypes.FindAll(item => { return item.Rule.FullName.Equals(FullName); });
        foreach (var item in useDataTypes)
        {
            foreach (var dependency in item.Dependencies)
            {
                var column = ((Database)Parent).Tables[dependency.Name].Columns[dependency.ColumnName];
                if ((!column.IsComputed) && (column.Status != ObjectStatus.Create))
                {
                    if (!items.ContainsKey(column.FullName))
                    {
                        listDiff.Add($"EXEC sp_unbindrule '{column.FullName}'\r\nGO\r\n", 0, ScriptAction.UnbindRuleColumn);
                        items.Add(column.FullName, column.FullName);
                    }
                }
            }
            if (item.Rule.Status != ObjectStatus.Create)
            {
                listDiff.Add($"EXEC sp_unbindrule '{item.FullName}'\r\nGO\r\n", 0, ScriptAction.UnbindRuleType);
            }
        }
        return listDiff;
    }

    /// <summary>
    /// Devuelve el schema de diferencias del Schema en formato SQL.
    /// </summary>
    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.AddRange(ToSQLUnBindAll());
            listDiff.Add(Drop());
        }
        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(Create());
        }

        if (Status == ObjectStatus.Alter)
        {
            listDiff.AddRange(ToSQLUnBindAll());
            listDiff.AddRange(Rebuild());
        }
        return listDiff;
    }
}
