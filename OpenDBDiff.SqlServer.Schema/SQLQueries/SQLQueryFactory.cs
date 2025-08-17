using System;
using System.Collections.Generic;

namespace OpenDBDiff.SqlServer.Schema.SQLQueries;

public static class SQLQueryFactory
{
    private static readonly Dictionary<string, string> queries = [];

    public static string Get(string queryFullName, Model.DatabaseInfo.SQLServerVersion version) => Get($"{queryFullName}.{version}");

    public static string Get(string queryClass)
    {
        var ns = typeof(SQLQueryFactory).Namespace;
        var qualifiedQueryClass = string.Concat(ns, '.', queryClass);

        if (queries.ContainsKey(qualifiedQueryClass))
        {
            return queries[qualifiedQueryClass];
        }
        else
        {
            var query = FetchQuery(qualifiedQueryClass);
            queries.Add(qualifiedQueryClass, query);
            return query;
        }
    }

    private static string FetchQuery(string queryFullName)
    {
        var resourceName = queryFullName + ".sql";
        using var stream = typeof(SQLQueryFactory).Assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("The Query " + queryFullName + " cannot be found");
        using var reader = new System.IO.StreamReader(stream);
        return reader.ReadToEnd();
    }
}
