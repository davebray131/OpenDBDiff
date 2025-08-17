namespace OpenDBDiff.Abstractions.Schema.Errors;

public class MessageLog(string description, string fullDescription, MessageLog.LogType type)
{
    public enum LogType
    {
        Information = 0,
        Warning = 1,
        Error = 2
    }

    public LogType Type { get; private set; } = type;

    public string FullDescription { get; private set; } = fullDescription;

    public string Description { get; private set; } = description;
}
