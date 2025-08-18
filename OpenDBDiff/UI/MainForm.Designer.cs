using System.ComponentModel;
using System.Windows.Forms;
using ScintillaNET;

namespace OpenDBDiff.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSchema = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabNewObject = new System.Windows.Forms.TabPage();
            this.txtNewObject = new ScintillaNET.Scintilla();
            this.tabOldObject = new System.Windows.Forms.TabPage();
            this.txtOldObject = new ScintillaNET.Scintilla();
            this.tabDiff = new System.Windows.Forms.TabPage();
            this.txtDiff = new ScintillaNET.Scintilla();
            this.tabScript = new System.Windows.Forms.TabPage();
            this.txtSyncScript = new ScintillaNET.Scintilla();
            this.tabAction = new System.Windows.Forms.TabPage();
            this.textActionReport = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.LeftDatabasePanel = new System.Windows.Forms.Panel();
            this.RightDatabasePanel = new System.Windows.Forms.Panel();
            this.SwapButton = new System.Windows.Forms.Button();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.btnSaveProject = new System.Windows.Forms.Button();
            this.btnProject = new System.Windows.Forms.Button();
            this.toolMenu = new System.Windows.Forms.ToolStrip();
            this.toolOpenProject = new System.Windows.Forms.ToolStripButton();
            this.toolNewProject = new System.Windows.Forms.ToolStripButton();
            this.toolSaveProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolLblProjectType = new System.Windows.Forms.ToolStripLabel();
            this.toolProjectTypes = new System.Windows.Forms.ToolStripComboBox();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCompareTableData = new System.Windows.Forms.Button();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.schemaTree = new OpenDBDiff.UI.SchemaTreeView();
            this.tabControl1.SuspendLayout();
            this.tabSchema.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabNewObject.SuspendLayout();
            this.tabOldObject.SuspendLayout();
            this.tabDiff.SuspendLayout();
            this.tabScript.SuspendLayout();
            this.tabAction.SuspendLayout();
            this.toolMenu.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSchema);
            this.tabControl1.Controls.Add(this.tabScript);
            this.tabControl1.Controls.Add(this.tabAction);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 555);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabSchema
            // 
            this.tabSchema.Controls.Add(this.tableLayoutPanel5);
            this.tabSchema.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSchema.Location = new System.Drawing.Point(4, 32);
            this.tabSchema.Name = "tabSchema";
            this.tabSchema.Padding = new System.Windows.Forms.Padding(3);
            this.tabSchema.Size = new System.Drawing.Size(910, 519);
            this.tabSchema.TabIndex = 1;
            this.tabSchema.Text = "   Schema   ";
            this.tabSchema.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.schemaTree, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(904, 513);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tabControl2, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(303, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(598, 507);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(598, 40);
            this.groupBox2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(345, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Drop object";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Alter old object";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Create new object";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Red;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(310, 9);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(32, 20);
            this.panel5.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Blue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(160, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(32, 20);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Lime;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(10, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(32, 20);
            this.panel3.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabNewObject);
            this.tabControl2.Controls.Add(this.tabOldObject);
            this.tabControl2.Controls.Add(this.tabDiff);
            this.tabControl2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 45);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(598, 462);
            this.tabControl2.TabIndex = 3;
            // 
            // tabNewObject
            // 
            this.tabNewObject.Controls.Add(this.txtNewObject);
            this.tabNewObject.Location = new System.Drawing.Point(4, 28);
            this.tabNewObject.Name = "tabNewObject";
            this.tabNewObject.Size = new System.Drawing.Size(590, 430);
            this.tabNewObject.TabIndex = 0;
            this.tabNewObject.Text = "   New object   ";
            this.tabNewObject.UseVisualStyleBackColor = true;
            // 
            // txtNewObject
            // 
            this.txtNewObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNewObject.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewObject.Location = new System.Drawing.Point(0, 0);
            this.txtNewObject.Name = "txtNewObject";
            this.txtNewObject.Size = new System.Drawing.Size(590, 430);
            this.txtNewObject.TabIndex = 0;
            // 
            // tabOldObject
            // 
            this.tabOldObject.Controls.Add(this.txtOldObject);
            this.tabOldObject.Location = new System.Drawing.Point(4, 28);
            this.tabOldObject.Name = "tabOldObject";
            this.tabOldObject.Size = new System.Drawing.Size(590, 430);
            this.tabOldObject.TabIndex = 1;
            this.tabOldObject.Text = "   Old object   ";
            this.tabOldObject.UseVisualStyleBackColor = true;
            // 
            // txtOldObject
            // 
            this.txtOldObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOldObject.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldObject.Location = new System.Drawing.Point(0, 0);
            this.txtOldObject.Name = "txtOldObject";
            this.txtOldObject.Size = new System.Drawing.Size(590, 430);
            this.txtOldObject.TabIndex = 0;
            // 
            // tabDiff
            // 
            this.tabDiff.Controls.Add(this.txtDiff);
            this.tabDiff.Location = new System.Drawing.Point(4, 28);
            this.tabDiff.Name = "tabDiff";
            this.tabDiff.Size = new System.Drawing.Size(590, 430);
            this.tabDiff.TabIndex = 2;
            this.tabDiff.Text = "   Diff   ";
            this.tabDiff.UseVisualStyleBackColor = true;
            // 
            // txtDiff
            // 
            this.txtDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiff.Location = new System.Drawing.Point(0, 0);
            this.txtDiff.Name = "txtDiff";
            this.txtDiff.Size = new System.Drawing.Size(590, 430);
            this.txtDiff.TabIndex = 0;
            // 
            // tabScript
            // 
            this.tabScript.Controls.Add(this.txtSyncScript);
            this.tabScript.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabScript.Location = new System.Drawing.Point(4, 32);
            this.tabScript.Name = "tabScript";
            this.tabScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabScript.Size = new System.Drawing.Size(910, 519);
            this.tabScript.TabIndex = 0;
            this.tabScript.Text = "   Synchronized script   ";
            this.tabScript.UseVisualStyleBackColor = true;
            // 
            // txtSyncScript
            // 
            this.txtSyncScript.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSyncScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSyncScript.Location = new System.Drawing.Point(3, 3);
            this.txtSyncScript.Margin = new System.Windows.Forms.Padding(0);
            this.txtSyncScript.Name = "txtSyncScript";
            this.txtSyncScript.ReadOnly = true;
            this.txtSyncScript.Size = new System.Drawing.Size(904, 513);
            this.txtSyncScript.TabIndex = 0;
            // 
            // tabAction
            // 
            this.tabAction.Controls.Add(this.textActionReport);
            this.tabAction.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAction.Location = new System.Drawing.Point(4, 32);
            this.tabAction.Name = "tabAction";
            this.tabAction.Padding = new System.Windows.Forms.Padding(3);
            this.tabAction.Size = new System.Drawing.Size(910, 519);
            this.tabAction.TabIndex = 2;
            this.tabAction.Text = "   Action report   ";
            this.tabAction.UseVisualStyleBackColor = true;
            // 
            // textActionReport
            // 
            this.textActionReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textActionReport.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textActionReport.Location = new System.Drawing.Point(3, 3);
            this.textActionReport.Multiline = true;
            this.textActionReport.Name = "textActionReport";
            this.textActionReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textActionReport.Size = new System.Drawing.Size(904, 513);
            this.textActionReport.TabIndex = 0;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "sql";
            this.saveFileDialog1.Filter = "SQL File|*.sql";
            // 
            // LeftDatabasePanel
            // 
            this.LeftDatabasePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LeftDatabasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftDatabasePanel.Location = new System.Drawing.Point(136, 3);
            this.LeftDatabasePanel.Name = "LeftDatabasePanel";
            this.LeftDatabasePanel.Size = new System.Drawing.Size(401, 169);
            this.LeftDatabasePanel.TabIndex = 10;
            // 
            // RightDatabasePanel
            // 
            this.RightDatabasePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RightDatabasePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RightDatabasePanel.Location = new System.Drawing.Point(583, 3);
            this.RightDatabasePanel.Name = "RightDatabasePanel";
            this.RightDatabasePanel.Size = new System.Drawing.Size(401, 169);
            this.RightDatabasePanel.TabIndex = 11;
            // 
            // SwapButton
            // 
            this.SwapButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwapButton.Image = ((System.Drawing.Image)(resources.GetObject("SwapButton.Image")));
            this.SwapButton.Location = new System.Drawing.Point(543, 3);
            this.SwapButton.Name = "SwapButton";
            this.SwapButton.Size = new System.Drawing.Size(34, 169);
            this.SwapButton.TabIndex = 12;
            this.toolTip1.SetToolTip(this.SwapButton, "Swap source and destination");
            this.SwapButton.UseVisualStyleBackColor = true;
            this.SwapButton.Click += new System.EventHandler(this.SwapButton_Click);
            // 
            // btnNewProject
            // 
            this.btnNewProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewProject.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNewProject.Image = ((System.Drawing.Image)(resources.GetObject("btnNewProject.Image")));
            this.btnNewProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewProject.Location = new System.Drawing.Point(3, 42);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnNewProject.Size = new System.Drawing.Size(121, 33);
            this.btnNewProject.TabIndex = 15;
            this.btnNewProject.Text = "New project";
            this.btnNewProject.UseVisualStyleBackColor = false;
            this.btnNewProject.Click += new System.EventHandler(this.BtnNewProject_Click);
            // 
            // btnSaveProject
            // 
            this.btnSaveProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveProject.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveProject.Image")));
            this.btnSaveProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveProject.Location = new System.Drawing.Point(3, 81);
            this.btnSaveProject.Name = "btnSaveProject";
            this.btnSaveProject.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnSaveProject.Size = new System.Drawing.Size(121, 33);
            this.btnSaveProject.TabIndex = 13;
            this.btnSaveProject.Text = "Save project";
            this.btnSaveProject.UseVisualStyleBackColor = false;
            this.btnSaveProject.Click += new System.EventHandler(this.BtnSaveProject_Click);
            // 
            // btnProject
            // 
            this.btnProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnProject.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnProject.Image = ((System.Drawing.Image)(resources.GetObject("btnProject.Image")));
            this.btnProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProject.Location = new System.Drawing.Point(3, 3);
            this.btnProject.Name = "btnProject";
            this.btnProject.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnProject.Size = new System.Drawing.Size(121, 33);
            this.btnProject.TabIndex = 12;
            this.btnProject.Text = "Open project";
            this.btnProject.UseVisualStyleBackColor = false;
            this.btnProject.Click += new System.EventHandler(this.BtnProject_Click);
            // 
            // toolMenu
            // 
            this.toolMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolOpenProject,
            this.toolNewProject,
            this.toolSaveProject,
            this.toolStripSeparator1,
            this.toolLblProjectType,
            this.toolProjectTypes});
            this.toolMenu.Location = new System.Drawing.Point(0, 0);
            this.toolMenu.Name = "toolMenu";
            this.toolMenu.Size = new System.Drawing.Size(940, 25);
            this.toolMenu.TabIndex = 16;
            this.toolMenu.Visible = false;
            // 
            // toolOpenProject
            // 
            this.toolOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("toolOpenProject.Image")));
            this.toolOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOpenProject.Name = "toolOpenProject";
            this.toolOpenProject.Size = new System.Drawing.Size(96, 22);
            this.toolOpenProject.Text = "&Open Project";
            // 
            // toolNewProject
            // 
            this.toolNewProject.Image = ((System.Drawing.Image)(resources.GetObject("toolNewProject.Image")));
            this.toolNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewProject.Name = "toolNewProject";
            this.toolNewProject.Size = new System.Drawing.Size(91, 22);
            this.toolNewProject.Text = "&New Project";
            // 
            // toolSaveProject
            // 
            this.toolSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("toolSaveProject.Image")));
            this.toolSaveProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolSaveProject.Name = "toolSaveProject";
            this.toolSaveProject.Size = new System.Drawing.Size(91, 22);
            this.toolSaveProject.Text = "&Save Project";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // toolLblProjectType
            // 
            this.toolLblProjectType.Name = "toolLblProjectType";
            this.toolLblProjectType.Size = new System.Drawing.Size(75, 22);
            this.toolLblProjectType.Text = "Project Type:";
            // 
            // toolProjectTypes
            // 
            this.toolProjectTypes.AutoSize = false;
            this.toolProjectTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolProjectTypes.Items.AddRange(new object[] {
            "SQL Sever 2005",
            "MySQL 5.0 or Higher",
            "Sybase 12.5"});
            this.toolProjectTypes.Name = "toolProjectTypes";
            this.toolProjectTypes.Size = new System.Drawing.Size(200, 23);
            this.toolProjectTypes.SelectedIndexChanged += new System.EventHandler(this.ToolProjectTypes_SelectedIndexChanged);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCompare.Image = ((System.Drawing.Image)(resources.GetObject("btnCompare.Image")));
            this.btnCompare.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCompare.Location = new System.Drawing.Point(5, 28);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(0, 28, 0, 4);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Padding = new System.Windows.Forms.Padding(4);
            this.btnCompare.Size = new System.Drawing.Size(95, 65);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Compare";
            this.btnCompare.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOptions.Image")));
            this.btnOptions.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOptions.Location = new System.Drawing.Point(5, 101);
            this.btnOptions.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Padding = new System.Windows.Forms.Padding(4);
            this.btnOptions.Size = new System.Drawing.Size(95, 65);
            this.btnOptions.TabIndex = 5;
            this.btnOptions.Text = "Options";
            this.btnOptions.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.BtnOptions_Click);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveAs.Enabled = false;
            this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveAs.Location = new System.Drawing.Point(5, 174);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Padding = new System.Windows.Forms.Padding(4);
            this.btnSaveAs.Size = new System.Drawing.Size(95, 65);
            this.btnSaveAs.TabIndex = 6;
            this.btnSaveAs.Text = "Save as";
            this.btnSaveAs.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.BtnSaveAs_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCopy.Enabled = false;
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCopy.Location = new System.Drawing.Point(5, 247);
            this.btnCopy.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Padding = new System.Windows.Forms.Padding(4);
            this.btnCopy.Size = new System.Drawing.Size(95, 65);
            this.btnCopy.TabIndex = 7;
            this.btnCopy.Text = "Copy script";
            this.btnCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpdate.Location = new System.Drawing.Point(5, 320);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Size = new System.Drawing.Size(95, 65);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnCompareTableData
            // 
            this.btnCompareTableData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCompareTableData.Enabled = false;
            this.btnCompareTableData.Image = ((System.Drawing.Image)(resources.GetObject("btnCompareTableData.Image")));
            this.btnCompareTableData.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCompareTableData.Location = new System.Drawing.Point(5, 393);
            this.btnCompareTableData.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnCompareTableData.Name = "btnCompareTableData";
            this.btnCompareTableData.Padding = new System.Windows.Forms.Padding(4);
            this.btnCompareTableData.Size = new System.Drawing.Size(95, 65);
            this.btnCompareTableData.TabIndex = 9;
            this.btnCompareTableData.Text = "Compare";
            this.btnCompareTableData.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompareTableData.UseVisualStyleBackColor = true;
            this.btnCompareTableData.Click += new System.EventHandler(this.BtnCompareTableData_Click);
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpdateAll.Enabled = false;
            this.btnUpdateAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateAll.Image")));
            this.btnUpdateAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpdateAll.Location = new System.Drawing.Point(5, 466);
            this.btnUpdateAll.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Padding = new System.Windows.Forms.Padding(4);
            this.btnUpdateAll.Size = new System.Drawing.Size(95, 65);
            this.btnUpdateAll.TabIndex = 10;
            this.btnUpdateAll.Text = "Update all";
            this.btnUpdateAll.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.BtnUpdateAll_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.RightDatabasePanel, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.LeftDatabasePanel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.SwapButton, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1029, 175);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnProject, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnSaveProject, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnNewProject, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(127, 169);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 181);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1029, 561);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btnUpdateAll, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.btnCompareTableData, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.btnUpdate, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.btnCopy, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.btnSaveAs, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.btnOptions, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnCompare, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(924, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(105, 561);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // schemaTree
            // 
            this.schemaTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schemaTree.LeftDatabase = null;
            this.schemaTree.Location = new System.Drawing.Point(0, 0);
            this.schemaTree.Margin = new System.Windows.Forms.Padding(0);
            this.schemaTree.Name = "schemaTree";
            this.schemaTree.RightDatabase = null;
            this.schemaTree.ShowChangedItems = true;
            this.schemaTree.ShowMissingItems = true;
            this.schemaTree.ShowNewItems = true;
            this.schemaTree.ShowUnchangedItems = true;
            this.schemaTree.Size = new System.Drawing.Size(300, 513);
            this.schemaTree.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnCompare;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 744);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.toolMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "OpenDBDiff";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSchema.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabNewObject.ResumeLayout(false);
            this.tabOldObject.ResumeLayout(false);
            this.tabDiff.ResumeLayout(false);
            this.tabScript.ResumeLayout(false);
            this.tabAction.ResumeLayout(false);
            this.tabAction.PerformLayout();
            this.toolMenu.ResumeLayout(false);
            this.toolMenu.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TabControl tabControl1;
        private TabPage tabScript;
        private Button btnCompare;
        private Button btnSaveAs;
        private SaveFileDialog saveFileDialog1;
        private Button btnCopy;
        private Button btnUpdate;
        private Button btnUpdateAll;
        private Button btnOptions;
        private TabPage tabSchema;
        private SchemaTreeView schemaTree;
        private TabPage tabAction;
        private TextBox textActionReport;
        private Scintilla txtSyncScript;
        private Panel LeftDatabasePanel;
        private Panel RightDatabasePanel;
        private Panel groupBox2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Label label1;
        private Label label3;
        private Label label2;
        private TabControl tabControl2;
        private TabPage tabNewObject;
        private TabPage tabOldObject;
        private Scintilla txtNewObject;
        private Scintilla txtOldObject;
        private Button btnSaveProject;
        private Button btnProject;
        private Button btnNewProject;
        private Button btnCompareTableData;
        private TabPage tabDiff;
        private Scintilla txtDiff;
        private ToolStrip toolMenu;
        private ToolStripButton toolOpenProject;
        private ToolStripButton toolNewProject;
        private ToolStripButton toolSaveProject;
        private ToolStripComboBox toolProjectTypes;
        private ToolStripLabel toolLblProjectType;
        private ToolStripSeparator toolStripSeparator1;
        private Button SwapButton;
        private ToolTip toolTip1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
    }
}
