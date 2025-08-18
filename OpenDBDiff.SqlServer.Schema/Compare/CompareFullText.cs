using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Compare;

internal class CompareFullText : CompareBase<FullText>
{
    protected override void DoUpdate<Root>(SchemaList<FullText, Root> originFields, FullText node)
    {
        if (!node.Compare(originFields[node.FullName]))
        {
            var newNode = node; //.Clone(originFields.Parent);
            if (node.IsDefault != originFields[node.FullName].IsDefault)
            {
                newNode.Status |= ObjectStatus.Disabled;
            }

            if (!node.Owner.Equals(originFields[node.FullName].Owner))
            {
                newNode.Status |= ObjectStatus.ChangeOwner;
            }

            if (node.IsAccentSensity != originFields[node.FullName].IsAccentSensity)
            {
                newNode.Status |= ObjectStatus.Alter;
            }

            originFields[node.FullName] = newNode;
        }
    }
}
