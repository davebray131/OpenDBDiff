using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Compare;

internal class CompareUserDataTypes : CompareBase<UserDataType>
{
    protected override void DoNew<Root>(SchemaList<UserDataType, Root> originFields, UserDataType node)
    {
        var newNode = (UserDataType)node.Clone(originFields.Parent);
        newNode.Status = ObjectStatus.Create;
        var HasAssembly = originFields.Exists(item => item.AssemblyFullName.Equals(node.AssemblyFullName) && item.IsAssembly);
        if (HasAssembly)
        {
            newNode.Status += (int)ObjectStatus.DropOlder;
        }

        originFields.Add(newNode);
    }

    protected override void DoUpdate<Root>(SchemaList<UserDataType, Root> originFields, UserDataType node)
    {
        if (!node.Compare(originFields[node.FullName]))
        {
            var newNode = (UserDataType)node.Clone(originFields.Parent);
            newNode.Dependencies.AddRange(originFields[node.FullName].Dependencies);

            if (!UserDataType.CompareDefault(node, originFields[node.FullName]))
            {
                newNode.Default.Status = !string.IsNullOrEmpty(node.Default.Name) ? ObjectStatus.Create : ObjectStatus.Drop;

                newNode.Status = ObjectStatus.Alter;
            }
            else
            {
                if (!UserDataType.CompareRule(node, originFields[node.FullName]))
                {
                    newNode.Rule.Status = !string.IsNullOrEmpty(node.Rule.Name) ? ObjectStatus.Create : ObjectStatus.Drop;

                    newNode.Status = ObjectStatus.Alter;
                }
                else
                {
                    newNode.Status = ObjectStatus.Rebuild;
                }
            }
            originFields[node.FullName] = newNode;
        }
    }
}
