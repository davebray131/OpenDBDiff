using System.Collections.Generic;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.Abstractions.Schema;

public class SqlAction(ISchemaBase item)
{
    public void Add(ISchemaBase item) => Childs.Add(new SqlAction(item));

    public SqlAction this[string name]
    {
        get
        {
            for (var j = 0; j < Childs.Count; j++)
            {
                if (Childs[j].Name.Equals(name))
                {
                    return Childs[j];
                }
            }
            return null;
        }
    }

    public string Name { get; private set; } = (item.ObjectType == ObjectType.Column) || (item.ObjectType == ObjectType.Index) || (item.ObjectType == ObjectType.Constraint)
            ? item.Name
            : item.FullName;

    public ObjectType Type { get; set; } = item.ObjectType;

    public ObjectStatus Action { get; set; } = item.Status;

    public List<SqlAction> Childs { get; private set; } = [];

    private string GetTypeName()
    {
        if (Type == ObjectType.Table)
        {
            return "TABLE";
        }

        if (Type == ObjectType.Column)
        {
            return "COLUMN";
        }

        if (Type == ObjectType.Constraint)
        {
            return "CONSTRAINT";
        }

        if (Type == ObjectType.Index)
        {
            return "INDEX";
        }

        if (Type == ObjectType.View)
        {
            return "VIEW";
        }

        if (Type == ObjectType.StoredProcedure)
        {
            return "STORED PROCEDURE";
        }

        if (Type == ObjectType.Synonym)
        {
            return "SYNONYM";
        }

        return Type == ObjectType.Function
            ? "FUNCTION"
            : Type == ObjectType.Assembly ? "ASSEMBLY" : Type == ObjectType.Trigger ? "TRIGGER" : "";
    }

    private bool IsRoot => (Type != ObjectType.Function) && (Type != ObjectType.StoredProcedure) && (Type != ObjectType.View) && (Type != ObjectType.Table) && (Type != ObjectType.Database);

    public string Message
    {
        get
        {
            var message = "";
            if (Action == ObjectStatus.Drop)
            {
                message = "DROP " + GetTypeName() + " " + Name + "\r\n";
            }

            if (Action == ObjectStatus.Create)
            {
                message = "ADD " + GetTypeName() + " " + Name + "\r\n";
            }

            if ((Action == ObjectStatus.Alter) || (Action == ObjectStatus.Rebuild) || (Action == ObjectStatus.RebuildDependencies))
            {
                message = "MODIFY " + GetTypeName() + " " + Name + "\r\n";
            }

            Childs.ForEach(item =>
                {
                    if (item.IsRoot)
                    {
                        message += "    ";
                    }

                    message += item.Message;
                });
            return message;
        }
    }
}
