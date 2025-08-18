using System.Text;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Model;

public class AssemblyFile : SQLServerSchemaBase
{
    public AssemblyFile(ISchemaBase parent, AssemblyFile assemblyFile, ObjectStatus status)
        : base(parent, ObjectType.AssemblyFile)
    {
        Name = assemblyFile.Name;
        Content = assemblyFile.Content;
        Status = status;
    }

    public AssemblyFile(ISchemaBase parent, string name, string content)
        : base(parent, ObjectType.AssemblyFile)
    {
        Name = name;
        Content = content;
    }

    public override string FullName => "[" + Name + "]";

    public string Content { get; set; }

    public override string ToSqlAdd()
    {
        var sql = new StringBuilder();
        sql.AppendLine($"ALTER ASSEMBLY {Parent.FullName}");
        sql.AppendLine($"ADD FILE FROM {Content}");
        sql.AppendLine($"AS N'{Name}'");
        sql.AppendLine(GO);
        return sql.ToString();
    }

    public override string ToSql() => ToSqlAdd();

    public override string ToSqlDrop() =>
        $"ALTER ASSEMBLY {Parent.FullName}\r\nDROP FILE N'{Name}'\r\n{GO}";

    public override SQLScriptList ToSqlDiff(System.Collections.Generic.ICollection<ISchemaBase> schemas)
    {
        var listDiff = new SQLScriptList();

        if (Status == ObjectStatus.Drop)
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropAssemblyFile);
        }

        if (Status == ObjectStatus.Create)
        {
            listDiff.Add(ToSqlAdd(), 0, ScriptAction.AddAssemblyFile);
        }

        if (HasState(ObjectStatus.Alter))
        {
            listDiff.Add(ToSqlDrop(), 0, ScriptAction.DropAssemblyFile);
            listDiff.Add(ToSqlAdd(), 0, ScriptAction.AddAssemblyFile);
        }
        return listDiff;
    }
}
