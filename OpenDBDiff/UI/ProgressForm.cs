using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenDBDiff.Abstractions.Schema.Events;
using OpenDBDiff.Abstractions.Schema.Model;
using OpenDBDiff.Abstractions.Ui;

namespace OpenDBDiff.UI;

public partial class ProgressForm : Form
{
    private readonly IGenerator OriginGenerator;
    private readonly IGenerator DestinationGenerator;
    private bool IsProcessing = false;
    private IDatabase originClone = null;
    private readonly IDatabaseComparer Comparer;

    // TODO: thread-safe error reporting

    public ProgressForm(KeyValuePair<string, IGenerator> originDatabase, KeyValuePair<string, IGenerator> destinationDatabase, IDatabaseComparer comparer)
    {
        InitializeComponent();

        Origin = null;
        Destination = null;
        originProgressControl.Maximum = originDatabase.Value.GetMaxValue();
        originProgressControl.DatabaseName = originDatabase.Key;
        OriginGenerator = originDatabase.Value;

        destinationProgressControl.Maximum = destinationDatabase.Value.GetMaxValue();
        destinationProgressControl.DatabaseName = destinationDatabase.Key;
        DestinationGenerator = destinationDatabase.Value;

        Comparer = comparer;
    }

    public IDatabase Origin { get; private set; }

    public IDatabase Destination { get; private set; }

    public string ErrorLocation { get; private set; }

    public string ErrorMostRecentProgress { get; private set; }

    public Exception Error { get; private set; }

    private void BtnOK_Click(object sender, EventArgs e)
    {
        Cursor = Cursors.WaitCursor;
        Close();
        Cursor = Cursors.Default;
    }

    private void ProgressForm_Activated(object sender, EventArgs e)
    {
        var handler = new ProgressEventHandler.ProgressHandler(GenData2_OnProgress);
        try
        {
            if (!IsProcessing)
            {
                Refresh();
                IsProcessing = false;
                OriginGenerator.OnProgress += new ProgressEventHandler.ProgressHandler(GenData1_OnProgress);
                DestinationGenerator.OnProgress += handler;

                ErrorLocation = "Loading " + destinationProgressControl.DatabaseName;
                Origin = OriginGenerator.Process();
                originProgressControl.Message = "Complete";
                originProgressControl.Value = OriginGenerator.GetMaxValue();

                ErrorLocation = "Loading " + originProgressControl.DatabaseName;
                Destination = DestinationGenerator.Process();

                originClone = (IDatabase)Origin.Clone(null);

                ErrorLocation = "Comparing Databases";
                Destination = Comparer.Compare(Origin, Destination);
                Origin = originClone;

                destinationProgressControl.Message = "Complete";
                destinationProgressControl.Value = DestinationGenerator.GetMaxValue();
            }
        }
        catch (Exception err)
        {
            Error = err;
        }
        finally
        {
            OriginGenerator.OnProgress -= handler;
            DestinationGenerator.OnProgress -= handler;
            Close();
        }
    }

    private void GenData2_OnProgress(ProgressEventArgs e)
    {
        if (e.Progress > -1 && destinationProgressControl.Value != e.Progress)
        {
            destinationProgressControl.Value = e.Progress;
        }

        if (string.Compare(destinationProgressControl.Message, e.Message) != 0)
        {
            destinationProgressControl.Message = e.Message;
        }

        ErrorMostRecentProgress = e.Message;
    }

    private void GenData1_OnProgress(ProgressEventArgs e)
    {
        if (e.Progress > -1 && originProgressControl.Value != e.Progress)
        {
            originProgressControl.Value = e.Progress;
        }

        if (string.Compare(originProgressControl.Message, e.Message) != 0)
        {
            originProgressControl.Message = e.Message;
        }

        ErrorMostRecentProgress = e.Message;
    }
}
