using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Compare;

internal class CompareConstraints : CompareBase<Constraint>
{
    protected override void DoUpdate<Root>(SchemaList<Constraint, Root> originFields, Constraint node)
    {
        var origin = originFields[node.FullName];
        if (!Constraint.Compare(origin, node))
        {
            var newNode = (Constraint)node.Clone(originFields.Parent);
            newNode.Status = node.IsDisabled == origin.IsDisabled ? ObjectStatus.Alter : ObjectStatus.Alter + (int)ObjectStatus.Disabled;

            originFields[node.FullName] = newNode;
        }
        else
        {
            if (node.IsDisabled != origin.IsDisabled)
            {
                var newNode = (Constraint)node.Clone(originFields.Parent);
                newNode.Status = ObjectStatus.Disabled;
                originFields[node.FullName] = newNode;
            }
        }
    }

    protected override void DoNew<Root>(SchemaList<Constraint, Root> originFields, Constraint node)
    {
        var newNode = (Constraint)node.Clone(originFields.Parent);
        newNode.Status = ObjectStatus.Create;
        originFields.Add(newNode);
    }
}
