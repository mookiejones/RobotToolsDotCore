namespace RobotTools.UI.Editor.Completion;

public abstract class CompletionContext
{
    public ITextEditor Editor { get; set; }
    private int StartOffset { get; set; }
    private int EndOffset { get; set; }

    public int Length
    {
        get { return EndOffset - StartOffset; }
    }

    public char CompletionChar { get; set; }
    public bool CompletionCharHandled { get; set; }
}
