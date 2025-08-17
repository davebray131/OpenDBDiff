using System.Collections;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Rule : Code
{
    public Rule(ISchemaBase parent)
        : base(parent, ObjectType.Rule, ScriptAction.AddRule, ScriptAction.DropRule)
    {
    }

    public new Rule Clone(ISchemaBase parent)
    {
        var item = new Rule(parent)
        {
            Id = this.Id,
            Name = this.Name,
            Owner = this.Owner,
            Text = this.Text,
            Guid = this.Guid
        };
        return item;
    }

    public string ToSQLAddBind()
    {
        var sql = this.Parent.ObjectType == ObjectType.Column
            ? string.Format("EXEC sp_bindrule N'{0}', N'[{1}].[{2}]','futureonly'\r\nGO\r\n", Name, this.Parent.Parent.Name, this.Parent.Name)
            : string.Format("EXEC sp_bindrule N'{0}', N'{1}','futureonly'\r\nGO\r\n", Name, this.Parent.Name);
        return sql;
    }

    public string ToSQLAddUnBind()
    {
        return this.Parent.ObjectType == ObjectType.Column
            ? string.Format("EXEC sp_unbindrule @objname=N'[{0}].[{1}]'\r\nGO\r\n", this.Parent.Parent.Name, this.Parent.Name)
            : string.Format("EXEC sp_unbindrule @objname=N'{0}'\r\nGO\r\n", this.Parent.Name);
    }

    private SQLScriptList ToSQLUnBindAll()
    {
        var listDiff = new SQLScriptList();
        Hashtable items = [];
        var useDataTypes = ((Database)this.Parent).UserTypes.FindAll(item => { return item.Rule.FullName.Equals(this.FullName); });
        foreach (var item in useDataTypes)
        {
            foreach (var dependency in item.Dependencies)
            {
                var column = ((Database)this.Parent).Tables[dependency.Name].Columns[dependency.ColumnName];
                if ((!column.IsComputed) && (column.Status != ObjectStatus.Create))
                {
                    if (!items.ContainsKey(column.FullName))
                    {
                        listDiff.Add("EXEC sp_unbindrule '" + column.FullName + "'\r\nGO\r\n", 0, ScriptAction.UnbindRuleColumn);
                        items.Add(column.FullName, column.FullName);
                    }
                }
            }
            if (item.Rule.Status != ObjectStatus.Create)
            {
                listDiff.Add("EXEC sp_unbindrule '" + item.FullName + "'\r\nGO\r\n", 0, ScriptAction.UnbindRuleType);
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

        if (this.Status == ObjectStatus.Drop)
        {
            listDiff.AddRange(ToSQLUnBindAll());
            listDiff.Add(Drop());
        }
        if (this.Status == ObjectStatus.Create)
        {
            listDiff.Add(Create());
        }

        if (this.Status == ObjectStatus.Alter)
        {
            listDiff.AddRange(ToSQLUnBindAll());
            listDiff.AddRange(Rebuild());
        }
        return listDiff;
    }
}
