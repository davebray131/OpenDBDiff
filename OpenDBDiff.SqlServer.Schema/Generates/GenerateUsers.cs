using Microsoft.Data.SqlClient;
using OpenDBDiff.SqlServer.Schema.Generates.SQLCommands;
using OpenDBDiff.SqlServer.Schema.Model;

namespace OpenDBDiff.SqlServer.Schema.Generates;

public class GenerateUsers(Generate root)
{
    private readonly Generate root = root;

    public void Fill(Database database, string connectioString)
    {
        string type;
        if (database.Options.Ignore.FilterUsers || database.Options.Ignore.FilterRoles)
        {
            using var conn = new SqlConnection(connectioString);
            using var command = new SqlCommand(UserSQLCommand.Get(database.Info.Version, database.Info.Edition), conn);
            conn.Open();
            command.CommandTimeout = 0;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                type = reader["type"].ToString();
                if (database.Options.Ignore.FilterUsers && (type.Equals("S") || type.Equals("U")))
                {
                    var item = new User(database)
                    {
                        Id = (int)reader["principal_id"],
                        Name = reader["name"].ToString(),
                        Login = reader["Login"].ToString(),
                        Owner = reader["default_schema_name"].ToString()
                    };
                    database.Users.Add(item);
                }
                if (database.Options.Ignore.FilterRoles && (type.Equals("A") || type.Equals("R")))
                {
                    var item = new Role(database)
                    {
                        Id = (int)reader["principal_id"],
                        Name = reader["name"].ToString(),
                        Owner = reader["default_schema_name"].ToString(),
                        Password = string.Empty,
                        IsSystem = (bool)reader["is_fixed_role"],
                        Type = type.Equals("A") ? Role.RoleTypeEnum.ApplicationRole : Role.RoleTypeEnum.DatabaseRole
                    };

                    database.Roles.Add(item);
                }
            }
        }
    }
}
