using System;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Abstractions.Ui;

namespace OpenDBDiff.UI;

public partial class OptionForm : Form
{
    private readonly IProjectHandler projectSelectorHandler;
    private readonly IOption SqlFilter;

    public event OptionControl.OptionEventHandler OptionSaved;

    public OptionForm(IProjectHandler projectSelectorHandler, IOption filter)
    {
        this.projectSelectorHandler = projectSelectorHandler;
        sqlOptionsFront1 = projectSelectorHandler.CreateOptionControl();
        sqlOptionsFront1.OptionSaved += SqlOptionsFront1_OptionSaved;
        SqlFilter = filter;
        sqlOptionsFront1.Load(filter);

        InitializeComponent();

        SuspendLayout();

        sqlOptionsFront1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
        | System.Windows.Forms.AnchorStyles.Left
        | System.Windows.Forms.AnchorStyles.Right;
        sqlOptionsFront1.Location = new System.Drawing.Point(3, 3);
        sqlOptionsFront1.Name = "sqlOptionsFront1";
        sqlOptionsFront1.Size = new System.Drawing.Size(586, 440);
        sqlOptionsFront1.TabIndex = 0;
        Controls.Add(sqlOptionsFront1);

        ResumeLayout();
    }

    private void SqlOptionsFront1_OptionSaved(IOption option) => OptionSaved?.Invoke(option);

    private void BtnApply_Click(object sender, EventArgs e)
    {
        sqlOptionsFront1.Save();
        Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e) => Close();
}
