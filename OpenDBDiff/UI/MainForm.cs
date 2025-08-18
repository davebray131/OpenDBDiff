using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using Essy.Tools.InputBox;
using OpenDBDiff.Abstractions.Schema;
using OpenDBDiff.Abstractions.Schema.Misc;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Abstractions.Ui;
using OpenDBDiff.Extensions;
using OpenDBDiff.Settings;
using OpenDBDiff.SqlServer.Ui.Front;
using ScintillaNET;

namespace OpenDBDiff.UI;

public partial class MainForm : Form
{
    private Project ActiveProject;
    private IFront LeftDatabaseSelector;
    private IFront RightDatabaseSelector;
    private IOption Options;
    private List<ISchemaBase> _selectedSchemas = [];

    private readonly List<IProjectHandler> ProjectHandlers = [];
    private IProjectHandler ProjectSelectorHandler;

    public MainForm()
    {
        InitializeComponent();

        Font = new Font("Calibri", 10);

        Text = string.Concat(nameof(OpenDBDiff), " v", Assembly.GetExecutingAssembly().GetName().Version.ToString());
    }

    private bool TestDatabase(string connectionString, string title)
    {
        var testForm = new ConnectionTestForm(connectionString, title);
        var testResult = testForm.ShowDialog();
        if (testResult != DialogResult.Yes)
        {
            if (testResult == DialogResult.No)
            {
                MessageBox.Show($"There was a problem connecting to the {title.ToLower()}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        return true;
    }

    private void StartComparison()
    {
        ProgressForm progress = null;
        string errorLocation = null;
        try
        {
            if (!string.IsNullOrEmpty(ProjectSelectorHandler.GetSourceDatabaseName()) &&
                 (!string.IsNullOrEmpty(ProjectSelectorHandler.GetDestinationDatabaseName())))
            {
                // Test connectivity to the databases first to prevent the nasty exception
                if (!TestDatabase(RightDatabaseSelector.ConnectionString, "Destination Database") || !TestDatabase(LeftDatabaseSelector.ConnectionString, "Source Database"))
                {
                    return;
                }

                Options ??= ProjectSelectorHandler.GetDefaultProjectOptions();
                var leftGenerator = ProjectSelectorHandler.SetSourceGenerator(LeftDatabaseSelector.ConnectionString, Options);
                var rightGenerator = ProjectSelectorHandler.SetDestinationGenerator(RightDatabaseSelector.ConnectionString, Options);
                var databaseComparer = ProjectSelectorHandler.GetDatabaseComparer();

                var leftPair = new KeyValuePair<string, IGenerator>(LeftDatabaseSelector.ToString(), leftGenerator);
                var rightPair = new KeyValuePair<string, IGenerator>(RightDatabaseSelector.ToString(), rightGenerator);

                // The progress form will execute the comparer to generate action scripts to migrate the right to the left
                // Hence, inside the ProgressForm and deeper, right is the origin and left is the destination
                progress = new ProgressForm(rightPair, leftPair, databaseComparer);
                progress.ShowDialog(this);
                if (progress.Error != null)
                {
                    throw new SchemaException(progress.Error.Message, progress.Error);
                }

                txtSyncScript.LexerLanguage = ProjectSelectorHandler.GetScriptLanguage();
                txtSyncScript.ReadOnly = false;
                errorLocation = "Generating Synchronized Script";
                txtSyncScript.Text = progress.Destination.ToSqlDiff(_selectedSchemas).ToSQL();
                txtSyncScript.ReadOnly = true;
                txtSyncScript.SetMarginWidth();

                // Notice again that left is destination, because we generated scripts to migrate the right database to the left.
                schemaTree.LeftDatabase = progress.Destination;
                schemaTree.RightDatabase = progress.Origin;

                schemaTree.OnSelectItem += new SchemaTreeView.SchemaHandler(SchemaTreeView1_OnSelectItem);
                SchemaTreeView1_OnSelectItem(schemaTree.SelectedNode);
                textActionReport.Text = progress.Origin.ActionMessage.Message;

                btnCopy.Enabled = true;
                btnSaveAs.Enabled = true;
                btnUpdateAll.Enabled = true;
            }
            else
            {
                MessageBox.Show(Owner, "Please select a valid connection string", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            if (errorLocation == null && progress != null)
            {
                errorLocation = string.Format("{0} (while {1})", progress.ErrorLocation, progress.ErrorMostRecentProgress ?? "initializing");
            }

            throw new SchemaException("Error " + (errorLocation ?? " Comparing Databases"), ex);
        }
    }

    private void SchemaTreeView1_OnSelectItem(string nodeFullName)
    {
        try
        {
            txtNewObject.ReadOnly = false;
            txtOldObject.ReadOnly = false;
            txtDiff.ReadOnly = false;

            txtNewObject.ClearAll();
            txtOldObject.ClearAll();
            txtDiff.ClearAll();

            if (string.IsNullOrEmpty(nodeFullName))
            {
                return;
            }

            var database = (IDatabase)schemaTree.LeftDatabase;

            ObjectStatus? status;

            status = database.Find(nodeFullName)?.Status;
            if (status.HasValue && status.Value != ObjectStatus.Drop)
            {
                txtNewObject.Text = database.Find(nodeFullName).ToSql();
                txtNewObject.SetMarginWidth();
                btnUpdate.Enabled = database.Find(nodeFullName).Status != ObjectStatus.Original;
                btnCompareTableData.Enabled = database.Find(nodeFullName).ObjectType == ObjectType.Table;
            }

            database = (IDatabase)schemaTree.RightDatabase;
            status = database.Find(nodeFullName)?.Status;
            if (status.HasValue && status.Value != ObjectStatus.Create)
            {
                txtOldObject.Text = database.Find(nodeFullName).ToSql();
                txtOldObject.SetMarginWidth();
            }
            txtNewObject.ReadOnly = true;
            txtOldObject.ReadOnly = true;

            var diff = new SideBySideDiffBuilder(new Differ()).BuildDiffModel(txtOldObject.Text, txtNewObject.Text);

            var sb = new StringBuilder();
            DiffPiece newLine, oldLine;
            var markers = new Marker[] { txtDiff.Markers[0], txtDiff.Markers[1], txtDiff.Markers[2], txtDiff.Markers[3] };
            foreach (var marker in markers)
            {
                marker.Symbol = MarkerSymbol.Background;
            }

            markers[0].SetBackColor(Color.LightGreen); // Imaginary (?)
            markers[1].SetBackColor(Color.LightCyan); // Modified
            markers[2].SetBackColor(Color.LightSalmon); // Deleted
            markers[3].SetBackColor(Color.PeachPuff); // Modified

            var indexes = new List<int>[] { [], [], [], [] };
            var index = 0;
            for (var i = 0; i < Math.Max(diff.NewText.Lines.Count, diff.OldText.Lines.Count); i++)
            {
                newLine = i < diff.NewText.Lines.Count ? diff.NewText.Lines[i] : null;
                oldLine = i < diff.OldText.Lines.Count ? diff.OldText.Lines[i] : null;
                if (oldLine.Type == ChangeType.Inserted)
                {
                    sb.AppendLine(" " + oldLine.Text);
                }
                else if (oldLine.Type == ChangeType.Deleted)
                {
                    sb.AppendLine("- " + oldLine.Text);
                    indexes[2].Add(index);
                }
                else if (oldLine.Type == ChangeType.Modified)
                {
                    sb.AppendLine("* " + newLine.Text);
                    indexes[1].Add(index++);
                    sb.AppendLine("* " + oldLine.Text);
                    indexes[3].Add(index);
                }
                else if (oldLine.Type == ChangeType.Imaginary)
                {
                    sb.AppendLine("+ " + newLine.Text);
                    indexes[0].Add(index);
                }
                else if (oldLine.Type == ChangeType.Unchanged)
                {
                    sb.AppendLine("  " + oldLine.Text);
                }
                index++;
            }
            txtDiff.Text = sb.ToString();
            txtDiff.SetMarginWidth();
            for (var i = 0; i < 4; i++)
            {
                foreach (var ind in indexes[i])
                {
                    txtDiff.Lines[ind].MarkerAdd(i);
                }
            }
        }
        finally
        {
            txtNewObject.ReadOnly = true;
            txtOldObject.ReadOnly = true;
            txtDiff.ReadOnly = true;
        }
    }

    private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Refresh script when tab is shown
        if (tabControl1.SelectedIndex != 1)
        {
            return;
        }

        if (schemaTree.LeftDatabase is IDatabase db)
        {
            _selectedSchemas = schemaTree.GetCheckedSchemas();
            txtSyncScript.ReadOnly = false;
            txtSyncScript.Text = db.ToSqlDiff(_selectedSchemas).ToSQL();
            txtSyncScript.ReadOnly = true;
            txtSyncScript.SetMarginWidth();
        }
    }

    private void BtnCompareTableData_Click(object sender, EventArgs e)
    {
        var tree = (TreeView)schemaTree.Controls.Find("treeView1", true)[0];
        var selected = (ISchemaBase)tree.SelectedNode.Tag;
        var dataCompare = new DataCompareForm(selected, LeftDatabaseSelector.ConnectionString, RightDatabaseSelector.ConnectionString);
        dataCompare.ShowDialog();
    }

    private void BtnCompare_Click(object sender, EventArgs e)
    {
        var errorLocation = "Processing Compare";
        try
        {
            Cursor = Cursors.WaitCursor;
            _selectedSchemas = schemaTree.GetCheckedSchemas();
            StartComparison();
            //schemaTreeView1.SetCheckedSchemas(_selectedSchemas);  // If you want to recall last schemas
            schemaTree.SelectAllSchemas(); // If you want to select all schemas after a compare
            errorLocation = "Saving Connections";
            Project.SaveLastConfiguration(LeftDatabaseSelector.ConnectionString, RightDatabaseSelector.ConnectionString);

            // Auto select the script tab
            tabControl1.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            HandleException(errorLocation, ex);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    private void HandleException(string errorLocation, Exception ex) => new ErrorForm(ex).ShowDialog(this);

    private void UnloadProjectHandler()
    {
        if (ProjectSelectorHandler != null)
        {
            LeftDatabasePanel.Controls.Remove((Control)LeftDatabaseSelector);
            RightDatabasePanel.Controls.Remove((Control)RightDatabaseSelector);
            ProjectSelectorHandler.Unload();
            ProjectSelectorHandler = null;
        }
    }

    private void LoadProjectHandler(IProjectHandler projectHandler)
    {
        UnloadProjectHandler();
        ProjectSelectorHandler = projectHandler;
        LeftDatabaseSelector = ProjectSelectorHandler.CreateSourceSelector();
        RightDatabaseSelector = ProjectSelectorHandler.CreateDestinationSelector();
        LeftDatabasePanel.Controls.Add(LeftDatabaseSelector.Control);
        RightDatabasePanel.Controls.Add(RightDatabaseSelector.Control);
    }

    private void LoadProjectHandler<T>() where T : IProjectHandler, new()
    {
        var handler = new T();
        LoadProjectHandler(handler);
    }

    private void OptSybase_CheckedChanged(object sender, EventArgs e)
    {
        /*if (optSybase.Checked)
        {
            this.mySqlConnectFront2 = new OpenDBDiff.Schema.Sybase.Front.AseConnectFront();
            this.mySqlConnectFront1 = new OpenDBDiff.Schema.Sybase.Front.AseConnectFront();
            this.mySqlConnectFront1.Location = new System.Drawing.Point(5, 19);
            this.mySqlConnectFront1.Name = "mySqlConnectFront1";
            this.mySqlConnectFront1.Size = new System.Drawing.Size(410, 214);
            this.mySqlConnectFront1.TabIndex = 10;
            this.mySqlConnectFront2.Location = new System.Drawing.Point(5, 19);
            this.mySqlConnectFront2.Name = "mySqlConnectFront2";
            this.mySqlConnectFront2.Size = new System.Drawing.Size(410, 214);
            this.mySqlConnectFront2.TabIndex = 10;
            this.mySqlConnectFront1.Visible = true;
            this.mySqlConnectFront2.Visible = true;
            this.groupBox3.Controls.Add((System.Windows.Forms.Control)this.mySqlConnectFront2);
            this.groupBox2.Controls.Add((System.Windows.Forms.Control)this.mySqlConnectFront1);
        }
        else
        {
            this.groupBox2.Controls.Remove((System.Windows.Forms.Control)this.mySqlConnectFront1);
            this.groupBox3.Controls.Remove((System.Windows.Forms.Control)this.mySqlConnectFront2);
        }*/
    }

    private void BtnSaveAs_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(saveFileDialog1.FileName) && !string.IsNullOrEmpty(Path.GetDirectoryName(saveFileDialog1.FileName)))
            {
                saveFileDialog1.InitialDirectory = Path.GetDirectoryName(saveFileDialog1.FileName);
                saveFileDialog1.FileName = Path.GetFileName(saveFileDialog1.FileName);
            }
            saveFileDialog1.ShowDialog(this);
            if (!string.IsNullOrEmpty(saveFileDialog1.FileName))
            {
                if (schemaTree.LeftDatabase is IDatabase db)
                {
                    using var writer = new StreamWriter(saveFileDialog1.FileName, false);
                    _selectedSchemas = schemaTree.GetCheckedSchemas();
                    writer.Write(db.ToSqlDiff(_selectedSchemas).ToSQL());
                    writer.Close();
                }
            }
        }
        catch (Exception ex)
        {
            HandleException("Save Script As", ex);
        }
    }

    private void BtnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            Clipboard.SetText(txtSyncScript.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show("An error ocurred while trying to copying the text to the clipboard");
            Trace.WriteLine("ERROR: +" + ex.Message);
        }
    }

    private void BtnUpdate_Click(object sender, EventArgs e)
    {
        var tree = (TreeView)schemaTree.Controls.Find("treeView1", true)[0];
        var dbArm = tree.Nodes[0];
        var sb = new StringBuilder();

        foreach (TreeNode node in dbArm.Nodes)
        {
            if (node.Nodes.Count != 0)
            {
                foreach (TreeNode subnode in node.Nodes)
                {
                    if (subnode.Checked)
                    {
                        //ISchemaBase selected = (ISchemaBase)tree.SelectedNode.Tag;
                        var selected = (ISchemaBase)subnode.Tag;

                        var database = (IDatabase)schemaTree.LeftDatabase;

                        if (database.Find(selected.FullName) != null)
                        {
                            switch (selected.ObjectType)
                            {
                                case ObjectType.Table:
                                    {
                                        switch (selected.Status)
                                        {
                                            case ObjectStatus.Create: sb.Append(Updater.CreateNew(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.AlterWhitespace: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            default: sb.AppendLine($"Nothing could be found to do for table '{selected.Name}'"); break;
                                        }
                                    }
                                    break;

                                case ObjectType.StoredProcedure:
                                    {
                                        switch (selected.Status)
                                        {
                                            case ObjectStatus.Create: sb.Append(Updater.CreateNew(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.AlterWhitespace: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            default: sb.AppendLine($"Nothing could be found to do for stored procedure '{selected.Name}'"); break;
                                        }
                                    }
                                    break;

                                case ObjectType.Function:
                                    {
                                        switch (selected.Status)
                                        {
                                            case ObjectStatus.Create: sb.Append(Updater.CreateNew(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.AlterWhitespace: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter | ObjectStatus.AlterBody: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            default: sb.AppendLine($"Nothing could be found to do for function '{selected.Name}'"); break;
                                        }
                                    }
                                    break;

                                case ObjectType.View:
                                    {
                                        switch (selected.Status)
                                        {
                                            case ObjectStatus.Create: sb.Append(Updater.CreateNew(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.AlterWhitespace: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            case ObjectStatus.Alter | ObjectStatus.AlterBody: sb.Append(Updater.Alter(selected, RightDatabaseSelector.ConnectionString)); break;
                                            default: sb.AppendLine($"Nothing could be found to do for view '{selected.Name}'"); break;
                                        }
                                    }
                                    break;

                                default:
                                    {
                                        switch (selected.Status)
                                        {
                                            case ObjectStatus.Create: sb.Append(Updater.AddNew(selected, RightDatabaseSelector.ConnectionString)); break;
                                            default: sb.AppendLine($"Nothing could be found to do for '{selected.Name}'"); break;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }

        _ = sb.Length == 0
            ? MessageBox.Show(this, "All successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            : MessageBox.Show(this, sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        if (Options.Comparison.ReloadComparisonOnUpdate)
        {
            StartComparison();
        }

        btnUpdate.Enabled = false;
    }

    private void BtnUpdateAll_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("Are you sure you want to update all?", "Confirm update", MessageBoxButtons.OKCancel) == DialogResult.OK)
        {
            var tree = (TreeView)schemaTree.Controls.Find("treeView1", true)[0];
            var database = tree.Nodes[0];
            var sb = new StringBuilder();
            foreach (TreeNode tn in database.Nodes)
            {
                foreach (TreeNode inner in tn.Nodes)
                {
                    if (inner.Tag != null)
                    {
                        var item = (ISchemaBase)inner.Tag;
                        switch (item.Status)
                        {
                            case ObjectStatus.Create: _ = sb.Append(Updater.CreateNew(item, RightDatabaseSelector.ConnectionString)); break;
                            case ObjectStatus.Alter: _ = sb.Append(Updater.Alter(item, RightDatabaseSelector.ConnectionString)); break;
                        }
                    }
                }
            }

            _ = sb.Length == 0
                ? MessageBox.Show(this, "Update successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                : MessageBox.Show(this, sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            StartComparison();
        }
    }

    private void BtnOptions_Click(object sender, EventArgs e)
    {
        Options ??= ProjectSelectorHandler.GetDefaultProjectOptions();
        var form = new OptionForm(ProjectSelectorHandler, Options);
        form.OptionSaved += new OptionControl.OptionEventHandler((option) => Options = option);
        form.ShowDialog(this);
    }

    private void LoadProjectHandlers()
    {
        ProjectHandlers.Clear();
        toolProjectTypes.Items.Clear();

        ProjectHandlers.Add(new SqlServer.Ui.SQLServerProjectHandler());
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        LoadProjectHandlers();
        foreach (var projectHandler in ProjectHandlers)
        {
            toolProjectTypes.Items.Add(projectHandler);
        }

        if (toolProjectTypes.SelectedItem == null && toolProjectTypes.Items.Count > 0)
        {
            toolProjectTypes.SelectedIndex = 0;
        }

        var LastConfiguration = Project.GetLastConfiguration();
        if (LastConfiguration != null)
        {
            if (LeftDatabaseSelector != null)
            {
                LeftDatabaseSelector.ConnectionString = LastConfiguration.ConnectionStringSource;
            }

            if (RightDatabaseSelector != null)
            {
                RightDatabaseSelector.ConnectionString = LastConfiguration.ConnectionStringDestination;
            }
        }

        txtNewObject.LexerLanguage = "mssql";
        txtNewObject.ReadOnly = false;
        txtOldObject.LexerLanguage = "mssql";
        txtOldObject.ReadOnly = false;
        txtDiff.LexerLanguage = "mssql";
        txtDiff.ReadOnly = false;
        txtDiff.Margins[0].Width = 20;

        var scintillaControls = new Scintilla[] { txtNewObject, txtOldObject, txtDiff, txtSyncScript };
        foreach (var scintilla in scintillaControls)
        {
            scintilla.InitializeScintillaControls();
        }
        txtSyncScript.Text = "";
        txtSyncScript.SetMarginWidth();
    }

    private void BtnSaveProject_Click(object sender, EventArgs e)
    {
        try
        {
            if (ActiveProject == null)
            {
                ActiveProject = new Project
                {
                    ConnectionStringSource = ProjectSelectorHandler.GetSourceConnectionString(),
                    ConnectionStringDestination = ProjectSelectorHandler.GetDestinationConnectionString(),
                    ProjectName = string.Format(
                        "[{0}].[{1}] - [{2}].[{3}]",
                        ProjectSelectorHandler.GetSourceServerName(),
                        ProjectSelectorHandler.GetSourceDatabaseName(),
                        ProjectSelectorHandler.GetDestinationServerName(),
                        ProjectSelectorHandler.GetDestinationDatabaseName()
                    ),
                    Options = Options ?? ProjectSelectorHandler.GetDefaultProjectOptions(),
                    Type = Project.ProjectType.SQLServer
                };

                var newProjectName = InputBox.ShowInputBox("Enter the project name.", ActiveProject.ProjectName.Trim(), false)?.Trim();

                if (string.IsNullOrWhiteSpace(newProjectName))
                {
                    return;
                }

                ActiveProject.ProjectName = newProjectName;
            }
            Project.Upsert(ActiveProject);
        }
        catch (Exception ex)
        {
            HandleException("Saving Project", ex);
        }
    }

    private void BtnProject_Click(object sender, EventArgs e)
    {
        try
        {
            var projects = Project.GetAll();
            if (projects.Any())
            {
                var form = new ListProjectsForm(projects);
                form.OnSelect += new ListProjectHandler(Form_OnSelect);
                form.OnDelete += new ListProjectHandler(Form_OnDelete);
                form.OnRename += new ListProjectHandler(Form_OnRename);
                form.ShowDialog(this);
            }
            else
            {
                MessageBox.Show(this, "There are currently no saved projects.", "Projects", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            HandleException("Opening Project", ex);
        }
    }

    private void Form_OnRename(Project itemSelected)
    {
        try
        {
            Project.Upsert(itemSelected);
        }
        catch (Exception ex)
        {
            HandleException("Renaming Project", ex);
        }
    }

    private void Form_OnDelete(Project itemSelected)
    {
        try
        {
            Project.Delete(itemSelected.Id);
            if ((ActiveProject?.Id).HasValue && ActiveProject.Id == itemSelected.Id)
            {
                ActiveProject = null;
                LeftDatabaseSelector.ConnectionString = "";
                RightDatabaseSelector.ConnectionString = "";
            }
        }
        catch (Exception ex)
        {
            HandleException("Deleting Project", ex);
        }
    }

    private void Form_OnSelect(Project itemSelected)
    {
        try
        {
            if (itemSelected != null)
            {
                ActiveProject = itemSelected;
                LeftDatabaseSelector.ConnectionString = itemSelected.ConnectionStringSource;
                RightDatabaseSelector.ConnectionString = itemSelected.ConnectionStringDestination;
            }
        }
        catch (Exception ex)
        {
            HandleException("Selecting Project", ex);
        }
    }

    private void BtnNewProject_Click(object sender, EventArgs e)
    {
        LeftDatabaseSelector.ConnectionString = string.Empty;
        RightDatabaseSelector.ConnectionString = string.Empty;
        ActiveProject = null;
    }

    private void ToolProjectTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        UnloadProjectHandler();
        if (toolProjectTypes.SelectedItem != null)
        {
            var handler = toolProjectTypes.SelectedItem as IProjectHandler;
            LoadProjectHandler(handler);
        }
    }

    private void SwapButton_Click(object sender, EventArgs e)
    {
        var temp = RightDatabaseSelector.Clone() as IFront;
        RightDatabaseSelector.SetSettingsFrom(LeftDatabaseSelector);
        LeftDatabaseSelector.SetSettingsFrom(temp);
    }
}
