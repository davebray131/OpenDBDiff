using System;

namespace OpenDBDiff.Abstractions.Schema.Events;

public class ProgressEventArgs(string message, int progress) : EventArgs
{
    public string Message { get; set; } = message;

    public int Progress { get; set; } = progress;
}
