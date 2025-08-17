using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Compare;

internal class CompareStoredProcedures : CompareBase<StoredProcedure>
{
    protected override void DoUpdate<Root>(SchemaList<StoredProcedure, Root> originFields, StoredProcedure node)
    {
        if (!node.Compare(originFields[node.FullName]))
        {
            var newNode = node; //.Clone(originFields.Parent);

            newNode.Status = node.CompareExceptWhitespace(originFields[node.FullName]) ? ObjectStatus.AlterWhitespace : ObjectStatus.Alter;

            originFields[node.FullName] = newNode;
        }
    }
}
