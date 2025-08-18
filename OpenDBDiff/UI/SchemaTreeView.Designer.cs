using System.ComponentModel;
using System.Windows.Forms;

namespace OpenDBDiff.UI
{
    partial class SchemaTreeView
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
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.chkOld = new System.Windows.Forms.CheckBox();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.chkDifferent = new System.Windows.Forms.CheckBox();
            this.chkShowUnchangedItems = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeView1.CheckBoxes = true;
            this.tableLayoutPanel1.SetColumnSpan(this.treeView1, 2);
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 51);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(245, 59);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            // 
            // chkOld
            // 
            this.chkOld.AutoSize = true;
            this.chkOld.Checked = true;
            this.chkOld.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOld.Location = new System.Drawing.Point(128, 26);
            this.chkOld.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.chkOld.Name = "chkOld";
            this.chkOld.Size = new System.Drawing.Size(117, 17);
            this.chkOld.TabIndex = 1;
            this.chkOld.Text = "Show missing items";
            this.chkOld.UseVisualStyleBackColor = true;
            this.chkOld.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.Checked = true;
            this.chkNew.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNew.Location = new System.Drawing.Point(3, 26);
            this.chkNew.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(103, 17);
            this.chkNew.TabIndex = 2;
            this.chkNew.Text = "Show new items";
            this.chkNew.UseVisualStyleBackColor = true;
            this.chkNew.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // chkDifferent
            // 
            this.chkDifferent.AutoSize = true;
            this.chkDifferent.Checked = true;
            this.chkDifferent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDifferent.Location = new System.Drawing.Point(128, 3);
            this.chkDifferent.Name = "chkDifferent";
            this.chkDifferent.Size = new System.Drawing.Size(120, 17);
            this.chkDifferent.TabIndex = 3;
            this.chkDifferent.Text = "Show changed items";
            this.chkDifferent.UseVisualStyleBackColor = true;
            this.chkDifferent.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // chkShowUnchangedItems
            // 
            this.chkShowUnchangedItems.AutoSize = true;
            this.chkShowUnchangedItems.Checked = true;
            this.chkShowUnchangedItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowUnchangedItems.Location = new System.Drawing.Point(3, 3);
            this.chkShowUnchangedItems.Name = "chkShowUnchangedItems";
            this.chkShowUnchangedItems.Size = new System.Drawing.Size(119, 17);
            this.chkShowUnchangedItems.TabIndex = 4;
            this.chkShowUnchangedItems.Text = "Show unchanged items";
            this.chkShowUnchangedItems.UseVisualStyleBackColor = true;
            this.chkShowUnchangedItems.CheckedChanged += new System.EventHandler(this.FilterCheckbox_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.chkShowUnchangedItems, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.treeView1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkOld, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkNew, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkDifferent, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(251, 113);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // SchemaTreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SchemaTreeView";
            this.Size = new System.Drawing.Size(251, 113);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView treeView1;
        private CheckBox chkOld;
        private CheckBox chkNew;
        private CheckBox chkDifferent;
        private CheckBox chkShowUnchangedItems;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
