using System;
using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class Assembly : Code
{
    public Assembly(ISchemaBase parent)
        : base(parent, ObjectType.Assembly, ScriptAction.AddAssembly, ScriptAction.DropAssembly) => Files = new SchemaList<AssemblyFile, Assembly>(this);

    public override ISchemaBase Clone(ISchemaBase parent)
    {
        var item = new Assembly(parent)
        {
            Id = Id,
            Name = Name,
            Owner = Owner,
            Visible = Visible,
            Text = Text,
            PermissionSet = PermissionSet,
            CLRName = CLRName,
            Guid = Guid,
            Files = Files
        };
        DependenciesOut.ForEach(dep => item.DependenciesOut.Add(dep));
        ExtendedProperties.ForEach(ep => item.ExtendedProperties.Add(ep));
        return item;
    }

    public SchemaList<AssemblyFile, Assembly> Files { get; set; }

    public override string FullName => "[" + Name + "]";

    public string CLRName { get; set; }

    public bool Visible { get; set; }

    public string PermissionSet { get; set; }

    public override string ToSql()
    {
        var access = PermissionSet;
        if (PermissionSet.Equals("UNSAFE_ACCESS"))
        {
            access = "UNSAFE";
        }

        if (PermissionSet.Equals("SAFE_ACCESS"))
        {
            access = "SAFE";
        }

        var sql = new StringBuilder();
        sql.AppendLine($"CREATE ASSEMBLY {FullName}");
        sql.AppendLine($"AUTHORIZATION {Owner}");
        sql.AppendLine($"FROM {Text}");
        sql.AppendLine($"WITH PERMISSION_SET = {access}");
        sql.AppendLine("GO");
        sql.Append(Files.ToSql());
        sql.Append(ExtendedProperties.ToSql());
        return sql.ToString();
    }

    public override string ToSqlDrop() => $"DROP ASSEMBLY {FullName}\r\nGO\r\n";

    public override string ToSqlAdd() => ToSql();

    private string ToSQLAlter()
    {
        var access = PermissionSet;
        if (PermissionSet.Equals("UNSAFE_ACCESS"))
        {
            access = "UNSAFE";
        }
        if (PermissionSet.Equals("SAFE_ACCESS"))
        {
            access = "SAFE";
        }

        return $"ALTER ASSEMBLY {FullName} WITH PERMISSION_SET = {access}\r\nGO\r\n";
    }

    private string ToSQLAlterOwner() => $"ALTER AUTHORIZATION ON ASSEMBLY::{FullName} TO {Owner}\r\nGO\r\n";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var list = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            list.AddRange(RebuildDependencies());
            list.Add(Drop());
        }
        if (Status == ObjectStatus.Create)
        {
            list.Add(Create());
        }

        if (HasState(ObjectStatus.Rebuild))
        {
            list.AddRange(Rebuild());
        }

        if (HasState(ObjectStatus.ChangeOwner))
        {
            list.Add(ToSQLAlterOwner(), 0, ScriptAction.AlterAssembly);
        }

        if (HasState(ObjectStatus.PermissionSet))
        {
            list.Add(ToSQLAlter(), 0, ScriptAction.AlterAssembly);
        }

        if (HasState(ObjectStatus.Alter))
        {
            list.AddRange(Files.ToSqlDiff());
        }

        list.AddRange(ExtendedProperties.ToSqlDiff());
        return list;
    }

    public bool Compare(Assembly obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        if (
            !CLRName.Equals(obj.CLRName) ||
            !PermissionSet.Equals(obj.PermissionSet) ||
            !Owner.Equals(obj.Owner) ||
            !Text.Equals(obj.Text) ||
            Files.Count != obj.Files.Count)
        {
            return false;
        }

        for (var j = 0; j < Files.Count; j++)
        {
            if (!Files[j].Content.Equals(obj.Files[j].Content))
            {
                return false;
            }
        }

        return true;
    }

    public override bool IsCodeType => true;
}
