using System.Collections.Generic;
using System.Data;
using System.Data.Sql;

namespace OpenDBDiff.SqlServer.Ui.Util
{
    internal static class SqlServerList
    {
        public static List<string> Get()
        {
            SqlDataSourceEnumerator sqlSource = SqlDataSourceEnumerator.Instance;
            DataTable dt = sqlSource.GetDataSources();

            List<string> serverList = new List<string>();
            string serverName;
            string instanceName;

            foreach (DataRow dr in dt.Rows)
            {
                serverName = dr["ServerName"].ToString();
                instanceName = dr["InstanceName"]?.ToString();

                if (string.IsNullOrEmpty(instanceName))
                    serverList.Add(serverName);
                else
                    serverList.Add(string.Format("{0}\\{1}", serverName, instanceName));
            }

            return serverList;
        }
    }
}
