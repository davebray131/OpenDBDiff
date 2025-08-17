using OpenDBDiff.Abstractions.Schema;

namespace OpenDBDiff.SqlServer.Schema.Generates.Util;

internal static class ConvertType
{
    public static ObjectType GetObjectType(string type) => type.Trim().Equals("V")
            ? ObjectType.View
            : type.Trim().Equals("U")
            ? ObjectType.Table
            : type.Trim().Equals("FN")
            ? ObjectType.Function
            : type.Trim().Equals("TF")
            ? ObjectType.Function
            : type.Trim().Equals("IF")
            ? ObjectType.Function
            : type.Trim().Equals("P") ? ObjectType.StoredProcedure : type.Trim().Equals("TR") ? ObjectType.Trigger : ObjectType.None;
}
