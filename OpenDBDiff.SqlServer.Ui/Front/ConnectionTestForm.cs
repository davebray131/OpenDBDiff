using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace OpenDBDiff.SqlServer.Ui.Front;
public partial class ConnectionTestForm : Form
{
    private readonly string connectionString;
    private readonly CancellationTokenSource ctsSource = new();
    public ConnectionTestForm(string connectionString, string title = null)
    {
        InitializeComponent();
        this.connectionString = connectionString;

        if (!string.IsNullOrWhiteSpace(title))
        {
            label1.Text = $"... Testing {title} ...";
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _ = StartConnectionTest();
    }

    private async Task StartConnectionTest()
    {
        try
        {
            using var connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            await connection.OpenAsync(ctsSource.Token).ConfigureAwait(false);
            connection.Close();
            CloseWith(DialogResult.Yes);
        }
        catch (TaskCanceledException) { CloseWith(DialogResult.Cancel); }
        catch { CloseWith(DialogResult.No); }
    }

    private void CloseWith(DialogResult result)
    {
        DialogResult = result;
        if (InvokeRequired)
        {
            BeginInvoke(() => Close());
            return;
        }
        Close();
    }

    private void ButtonCancelTest_Click(object sender, EventArgs e) => ctsSource.Cancel();
}
