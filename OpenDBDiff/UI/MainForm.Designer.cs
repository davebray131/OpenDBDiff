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
            this.lblMessage = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtNewObject = new ScintillaNET.Scintilla();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.txtOldObject = new ScintillaNET.Scintilla();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtDiff = new ScintillaNET.Scintilla();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.schemaTreeView1 = new OpenDBDiff.UI.SchemaTreeView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSyncScript = new ScintillaNET.Scintilla();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.toolMenu.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(6, 528);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(918, 555);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(910, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Schema";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(350, 50);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(676, 474);
            this.tabControl2.TabIndex = 3;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.txtNewObject);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(668, 448);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "New object";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // txtNewObject
            // 
            this.txtNewObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNewObject.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewObject.Location = new System.Drawing.Point(3, 3);
            this.txtNewObject.Name = "txtNewObject";
            this.txtNewObject.Size = new System.Drawing.Size(662, 442);
            this.txtNewObject.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.txtOldObject);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(668, 448);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Old object";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // txtOldObject
            // 
            this.txtOldObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOldObject.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOldObject.Location = new System.Drawing.Point(3, 3);
            this.txtOldObject.Name = "txtOldObject";
            this.txtOldObject.Size = new System.Drawing.Size(662, 442);
            this.txtOldObject.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txtDiff);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(668, 448);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Diff";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // txtDiff
            // 
            this.txtDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiff.Location = new System.Drawing.Point(0, 0);
            this.txtDiff.Name = "txtDiff";
            this.txtDiff.Size = new System.Drawing.Size(668, 448);
            this.txtDiff.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Location = new System.Drawing.Point(350, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(676, 40);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(345, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Drop object";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Alter old object";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Create new object";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Red;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Location = new System.Drawing.Point(310, 12);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(32, 20);
            this.panel5.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Blue;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(160, 12);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(32, 20);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Lime;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(10, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(32, 20);
            this.panel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.schemaTreeView1);
            this.groupBox1.Location = new System.Drawing.Point(6, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(335, 517);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // schemaTreeView1
            // 
            this.schemaTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.schemaTreeView1.LeftDatabase = null;
            this.schemaTreeView1.Location = new System.Drawing.Point(7, 10);
            this.schemaTreeView1.Name = "schemaTreeView1";
            this.schemaTreeView1.RightDatabase = null;
            this.schemaTreeView1.ShowChangedItems = true;
            this.schemaTreeView1.ShowMissingItems = true;
            this.schemaTreeView1.ShowNewItems = true;
            this.schemaTreeView1.ShowUnchangedItems = true;
            this.schemaTreeView1.Size = new System.Drawing.Size(322, 501);
            this.schemaTreeView1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.lblMessage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(910, 529);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Synchronized script";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.txtSyncScript);
            this.panel1.Location = new System.Drawing.Point(9, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(895, 607);
            this.panel1.TabIndex = 6;
            // 
            // txtSyncScript
            // 
            this.txtSyncScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSyncScript.Location = new System.Drawing.Point(0, 0);
            this.txtSyncScript.Name = "txtSyncScript";
            this.txtSyncScript.ReadOnly = true;
            this.txtSyncScript.Size = new System.Drawing.Size(891, 603);
            this.txtSyncScript.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(910, 529);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Action report";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(9, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(895, 607);
            this.textBox1.TabIndex = 0;
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
            this.SwapButton.Image = global::OpenDBDiff.Properties.Resources.arrow_ew;
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
            this.btnNewProject.Image = global::OpenDBDiff.Properties.Resources.new_window;
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
            this.btnSaveProject.Image = global::OpenDBDiff.Properties.Resources.diskette;
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
            this.btnProject.Image = global::OpenDBDiff.Properties.Resources.folder;
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
            this.toolOpenProject.Image = global::OpenDBDiff.Properties.Resources.folder;
            this.toolOpenProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolOpenProject.Name = "toolOpenProject";
            this.toolOpenProject.Size = new System.Drawing.Size(96, 22);
            this.toolOpenProject.Text = "&Open Project";
            // 
            // toolNewProject
            // 
            this.toolNewProject.Image = global::OpenDBDiff.Properties.Resources.new_window;
            this.toolNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolNewProject.Name = "toolNewProject";
            this.toolNewProject.Size = new System.Drawing.Size(91, 22);
            this.toolNewProject.Text = "&New Project";
            // 
            // toolSaveProject
            // 
            this.toolSaveProject.Image = global::OpenDBDiff.Properties.Resources.diskette;
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
            this.btnCompare.Image = global::OpenDBDiff.Properties.Resources.compare;
            this.btnCompare.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCompare.Location = new System.Drawing.Point(5, 22);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(0, 22, 0, 4);
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
            this.btnOptions.Image = global::OpenDBDiff.Properties.Resources.setting_tools;
            this.btnOptions.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnOptions.Location = new System.Drawing.Point(5, 95);
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
            this.btnSaveAs.Image = global::OpenDBDiff.Properties.Resources.save_as;
            this.btnSaveAs.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSaveAs.Location = new System.Drawing.Point(5, 168);
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
            this.btnCopy.Image = global::OpenDBDiff.Properties.Resources.clipboard_invoice;
            this.btnCopy.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCopy.Location = new System.Drawing.Point(5, 241);
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
            this.btnUpdate.Image = global::OpenDBDiff.Properties.Resources.refresh_all;
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpdate.Location = new System.Drawing.Point(5, 314);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Padding = new System.Windows.Forms.Padding(4);
            this.btnUpdate.Size = new System.Drawing.Size(95, 65);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update selected";
            this.btnUpdate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnCompareTableData
            // 
            this.btnCompareTableData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCompareTableData.Enabled = false;
            this.btnCompareTableData.Image = global::OpenDBDiff.Properties.Resources.table_analysis;
            this.btnCompareTableData.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCompareTableData.Location = new System.Drawing.Point(5, 387);
            this.btnCompareTableData.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.btnCompareTableData.Name = "btnCompareTableData";
            this.btnCompareTableData.Padding = new System.Windows.Forms.Padding(4);
            this.btnCompareTableData.Size = new System.Drawing.Size(95, 65);
            this.btnCompareTableData.TabIndex = 9;
            this.btnCompareTableData.Text = "Compare data";
            this.btnCompareTableData.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCompareTableData.UseVisualStyleBackColor = true;
            this.btnCompareTableData.Click += new System.EventHandler(this.BtnCompareTableData_Click);
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpdateAll.Enabled = false;
            this.btnUpdateAll.Image = global::OpenDBDiff.Properties.Resources.database_refresh;
            this.btnUpdateAll.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUpdateAll.Location = new System.Drawing.Point(5, 460);
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
            this.tabPage2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
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

        private Label lblMessage;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnCompare;
        private Button btnSaveAs;
        private SaveFileDialog saveFileDialog1;
        private Button btnCopy;
        private Button btnUpdate;
        private Button btnUpdateAll;
        private Button btnOptions;
        private TabPage tabPage2;
        private SchemaTreeView schemaTreeView1;
        private GroupBox groupBox1;
        private TabPage tabPage3;
        private TextBox textBox1;
        private Panel panel1;
        private Scintilla txtSyncScript;
        private Panel LeftDatabasePanel;
        private Panel RightDatabasePanel;
        private GroupBox groupBox2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Label label1;
        private Label label3;
        private Label label2;
        private TabControl tabControl2;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private Scintilla txtNewObject;
        private Scintilla txtOldObject;
        private Button btnSaveProject;
        private Button btnProject;
        private Button btnNewProject;
        private Button btnCompareTableData;
        private TabPage tabPage6;
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
    }
}
