using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDBDiff.SqlServer.Ui
{
    partial class SqlServerConnectFront
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.PictureBox pictureBox1;
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboServer = new System.Windows.Forms.ComboBox();
            this.cboAuthentication = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.gradientPanel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).BeginInit();
            this.gradientPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = System.Drawing.Color.Transparent;
            pictureBox1.Image = global::OpenDBDiff.SqlServer.Ui.Properties.Resources.database_yellow;
            pictureBox1.Location = new System.Drawing.Point(8, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(15, 18);
            pictureBox1.TabIndex = 27;
            pictureBox1.TabStop = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(81, 107);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(233, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextChanged += new System.EventHandler(this.TxtPassword_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(49, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "User";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Host";
            // 
            // cboServer
            // 
            this.cboServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboServer.FormattingEnabled = true;
            this.cboServer.Items.AddRange(new object[] {
            "(local)"});
            this.cboServer.Location = new System.Drawing.Point(81, 27);
            this.cboServer.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cboServer.Name = "cboServer";
            this.cboServer.Size = new System.Drawing.Size(233, 21);
            this.cboServer.TabIndex = 0;
            this.cboServer.DropDown += new System.EventHandler(this.CboServer_DropDown);
            this.cboServer.SelectedIndexChanged += new System.EventHandler(this.CboServer_SelectedIndexChanged);
            this.cboServer.TextChanged += new System.EventHandler(this.CboServer_TextChanged);
            // 
            // cboAuthentication
            // 
            this.cboAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthentication.FormattingEnabled = true;
            this.cboAuthentication.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.cboAuthentication.Location = new System.Drawing.Point(81, 54);
            this.cboAuthentication.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cboAuthentication.Name = "cboAuthentication";
            this.cboAuthentication.Size = new System.Drawing.Size(233, 21);
            this.cboAuthentication.TabIndex = 1;
            this.cboAuthentication.SelectedIndexChanged += new System.EventHandler(this.CboAuthentication_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Authentication";
            // 
            // cboDatabase
            // 
            this.cboDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDatabase.FormattingEnabled = true;
            this.cboDatabase.Location = new System.Drawing.Point(81, 133);
            this.cboDatabase.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(233, 21);
            this.cboDatabase.TabIndex = 4;
            this.cboDatabase.DropDown += new System.EventHandler(this.CboDatabase_DropDown);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Database";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(81, 81);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(233, 20);
            this.txtUsername.TabIndex = 2;
            this.txtUsername.TextChanged += new System.EventHandler(this.TxtUsername_TextChanged);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(322, 133);
            this.btnTest.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(54, 20);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // lblName
            // 
            this.lblName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(29, 5);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(99, 15);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "Source Database";
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.gradientPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.gradientPanel1, 3);
            this.gradientPanel1.Controls.Add(pictureBox1);
            this.gradientPanel1.Controls.Add(this.lblName);
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(384, 24);
            this.gradientPanel1.TabIndex = 26;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gradientPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTest, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.cboDatabase, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.cboAuthentication, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtUsername, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cboServer, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 163);
            this.tableLayoutPanel1.TabIndex = 27;
            // 
            // SqlServerConnectFront
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SqlServerConnectFront";
            this.Size = new System.Drawing.Size(384, 163);
            ((System.ComponentModel.ISupportInitialize)(pictureBox1)).EndInit();
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label4;
        private Label label3;
        private Label label2;
        private ComboBox cboServer;
        private ComboBox cboAuthentication;
        private Label label5;
        private Button btnTest;
        private ComboBox cboDatabase;
        private Label label6;
        private Label lblName;
        private Panel gradientPanel1;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
