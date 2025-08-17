using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema.Model;

namespace OpenDBDiff.UI;

public partial class DataCompareForm : Form
{
    public DataCompareForm(ISchemaBase Selected, string SrcConnectionString, string DestConnectionString)
    {
        InitializeComponent();
        this.selected = Selected;
        this.srcConnectionString = SrcConnectionString;
        this.destConnectionString = DestConnectionString;

        DoCompare();
    }

    private void DoCompare()
    {
        var srcTable = Updater.GetData(selected, srcConnectionString);
        var destTable = Updater.GetData(selected, destConnectionString);

        srcDgv.MultiSelect = false;
        srcDgv.ReadOnly = true;
        srcDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        srcDgv.RowHeadersVisible = false;
        srcDgv.DataSource = srcTable;
        srcDgv.Rows[0].Cells[0].Style.ForeColor = Color.Blue;

        destDgv.MultiSelect = false;
        destDgv.ReadOnly = true;
        destDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        destDgv.RowHeadersVisible = false;
        destDgv.DataSource = destTable;
        destDgv.CellFormatting += new DataGridViewCellFormattingEventHandler(DestDgv_CellFormatting);
    }

    private void DestDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var table = (DataTable)destDgv.DataSource;
        if (e.RowIndex < table.Rows.Count)
        {
            if (table.Rows[e.RowIndex].RowState == DataRowState.Added)
            {
                e.CellStyle.ForeColor = Color.Green;
            }
            else if (table.Rows[e.RowIndex].RowState == DataRowState.Modified)
            {
                e.CellStyle.ForeColor = Color.Blue;
            }
        }
    }

    private void BtnCommitChanges_Click(object sender, EventArgs e)
    {
        var destination = (DataTable)destDgv.DataSource;
        var edits = destination.GetChanges();
        if (Updater.CommitTable(edits, selected.FullName, destConnectionString))
        {
            destination.AcceptChanges();
            DoCompare();
            btnCommitChanges.Enabled = false;
        }
    }

    private void BtnUpdateRow_Click(object sender, EventArgs e)
    {
        var source = (DataTable)srcDgv.DataSource;
        var destination = (DataTable)destDgv.DataSource;

        var sourceItems = source.Rows[srcDgv.CurrentRow.Index].ItemArray;

        for (var i = 0; i < destination.Columns.Count; i++)
        {
            if (destination.Columns[i].Unique)
            {
                if (destination.Rows.Find(sourceItems[i]) == null && destination.Columns[i].AutoIncrement)
                {
                    sourceItems[i] = null;
                }
            }
        }

        destination.BeginLoadData();
        _ = destination.LoadDataRow(sourceItems, false);
        destination.EndLoadData();
        btnCommitChanges.Enabled = true;
    }

    private void BtnMerge_Click(object sender, EventArgs e)
    {
        var source = (DataTable)srcDgv.DataSource;
        var destination = (DataTable)destDgv.DataSource;

        destination.Merge(source, true);
        foreach (DataRow dr in destination.Rows)
        {
            if (dr.RowState == DataRowState.Unchanged)
            {
                dr.SetAdded();
            }
        }
        btnCommitChanges.Enabled = true;
    }

    private void BtnRowToRow_Click(object sender, EventArgs e)
    {
        var source = (DataTable)srcDgv.DataSource;
        var destination = (DataTable)destDgv.DataSource;

        var sourceRow = source.Rows[srcDgv.CurrentRow.Index];
        var destinationRow = destination.Rows[destDgv.CurrentRow.Index];

        for (var i = 0; i < destination.Columns.Count; i++)
        {
            if (!destination.Columns[i].Unique)
            {
                destinationRow[i] = sourceRow[i];
            }
        }
        btnCommitChanges.Enabled = true;
    }
    private readonly ISchemaBase selected;
    private readonly string srcConnectionString;
    private readonly string destConnectionString;
}
