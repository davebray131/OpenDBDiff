using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using OpenDBDiff.Abstractions.Ui;
using OpenDBDiff.SqlServer.Ui.Util;

namespace OpenDBDiff.SqlServer.Ui
{
    public partial class SqlServerConnectFront : UserControl, IFront
    {
        private bool isDatabaseFilled = false;
        private bool isServerFilled = false;

        private delegate void clearCombo();

        private delegate void addCombo(string item);

        public SqlServerConnectFront()
        {
            InitializeComponent();
            cboAuthentication.SelectedIndex = 1;
        }

        private void CboAuthentication_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = cboAuthentication.SelectedIndex == 1;
            txtPassword.Enabled = cboAuthentication.SelectedIndex == 1;
            isDatabaseFilled = false;
            ClearDatabase();
        }

        public string ErrorConnection { get; private set; }

        public bool UseWindowsAuthentication
        {
            get => cboAuthentication.SelectedIndex == 0; set => cboAuthentication.SelectedIndex = value ? 0 : 1;
        }

        public string DatabaseName
        {
            get => cboDatabase.Text; set => cboDatabase.Text = value;
        }

        public string UserName
        {
            get => txtUsername.Text; set => txtUsername.Text = value;
        }

        public string Password
        {
            get => txtPassword.Text; set => txtPassword.Text = value;
        }

        public override string Text
        {
            get => lblName.Text; set => lblName.Text = value;
        }

        public string ServerName
        {
            get => cboServer.Text; set => cboServer.Text = value;
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection())
                {
                    connection.ConnectionString = this.ConnectionString;
                    connection.Open();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorConnection = ex.Message;
                return false;
            }
        }

        private string BuildConnectionString(string server, string database)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server.Trim()
            };

            // if database is ommitted the connection will be established to the default database for the user
            if (!string.IsNullOrEmpty(database))
            {
                builder.InitialCatalog = database.Trim();
            }

            builder.IntegratedSecurity = true;
            builder.TrustServerCertificate = true;
            return builder.ConnectionString;
        }

        private string BuildConnectionString(string server, string database, string username, string password)
        {
            var builder = new SqlConnectionStringBuilder(BuildConnectionString(server, database))
            {
                IntegratedSecurity = false,
                UserID = username,
                Password = password
            };
            return builder.ConnectionString;
        }

        public string ConnectionStringToDefaultDatabase => cboAuthentication.SelectedIndex == 1
                    ? BuildConnectionString(cboServer.Text, null, txtUsername.Text, txtPassword.Text)
                    : BuildConnectionString(cboServer.Text, null);

        public string ConnectionStringToMasterDatabase => cboAuthentication.SelectedIndex == 1
                    ? BuildConnectionString(cboServer.Text, "master", txtUsername.Text, txtPassword.Text)
                    : BuildConnectionString(cboServer.Text, "master");

        public string ConnectionString
        {
            get => cboAuthentication.SelectedIndex == 1
                    ? BuildConnectionString(cboServer.Text, cboDatabase.Text, txtUsername.Text, txtPassword.Text)
                    : BuildConnectionString(cboServer.Text, cboDatabase.Text);
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var builder = new SqlConnectionStringBuilder(value);

                    ServerName = builder.DataSource;
                    UseWindowsAuthentication = builder.IntegratedSecurity;
                    if (UseWindowsAuthentication)
                    {
                        UserName = "";
                        Password = "";
                    }
                    else
                    {
                        UserName = builder.UserID;
                        Password = builder.Password;
                    }
                    DatabaseName = builder.InitialCatalog;
                }
                else
                {
                    cboAuthentication.SelectedIndex = 1;
                    UserName = "";
                    Password = "";
                    ServerName = "(local)";
                    DatabaseName = "";
                }
            }
        }

        public Control Control => this;

        private void BtnTest_Click(object sender, EventArgs e)
        {
            _ = TestConnection()
                ? MessageBox.Show(this, "Test successful!", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information)
                : MessageBox.Show(this, "Test failed!\r\n" + ErrorConnection, "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AddComboItem(string item)
        {
            if (!InvokeRequired)
            {
                _ = cboDatabase.Items.Add(item);
            }
            else
            {
                var add = new addCombo(AddComboItem);
                _ = Invoke(add, new string[] { item });
            }
        }

        private void ClearDatabase()
        {
            if (!InvokeRequired)
            {
                cboDatabase.Items.Clear();
            }
            else
            {
                var clear = new clearCombo(ClearDatabase);
                _ = Invoke(clear);
            }
        }

        private void FillDatabase()
        {
            if (!isDatabaseFilled)
            {
                var connectionString = ConnectionStringToDefaultDatabase;
                ClearDatabase();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var command = new SqlCommand("SELECT name,database_id FROM sys.databases ORDER BY Name", conn))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AddComboItem(reader["Name"].ToString());
                            }
                            isDatabaseFilled = true;
                        }
                    }
                }
            }
        }

        private void CboServer_SelectedIndexChanged(object sender, EventArgs e) => isDatabaseFilled = false;

        private void TxtUsername_TextChanged(object sender, EventArgs e)
        {
            isDatabaseFilled = false;
            ClearDatabase();
        }

        private void TxtPassword_TextChanged(object sender, EventArgs e)
        {
            isDatabaseFilled = false;
            ClearDatabase();
        }

        private void CboServer_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (!isServerFilled)
                {
                    this.Cursor = Cursors.WaitCursor;
                    SqlServerList.Get().ForEach(item => cboServer.Items.Add(item));
                    isServerFilled = true;
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(this, ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CboDatabase_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FillDatabase();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                cboDatabase.Items.Clear();
                _ = MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CboServer_TextChanged(object sender, EventArgs e) => isDatabaseFilled = false;

        public override string ToString() => string.Format($"Server: {ServerName}, Database: {DatabaseName}");

        public object Clone()
        {
            var clone = new SqlServerConnectFront
            {
                ServerName = this.ServerName,
                UseWindowsAuthentication = this.UseWindowsAuthentication,
                UserName = this.UserName,
                Password = this.Password,
                DatabaseName = this.DatabaseName,
                Location = new System.Drawing.Point(1, 1),
                Name = "SourceControl",
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            return clone;
        }

        public void SetSettingsFrom(IFront other)
        {
            if (other is SqlServerConnectFront sql)
            {
                this.ServerName = sql.ServerName;
                this.DatabaseName = sql.DatabaseName;
                this.UseWindowsAuthentication = sql.UseWindowsAuthentication;
                this.UserName = sql.UserName;
                this.Password = sql.Password;
            }
        }
    }
}
