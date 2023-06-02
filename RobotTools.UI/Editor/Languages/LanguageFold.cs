using System;
using ICSharpCode.AvalonEdit.Folding;

namespace RobotTools.UI.Editor.Languages;

public class LanguageFold:NewFolding
{
    #region Properties
    public string StartFold { get; private set; }
    internal string EndFold { get; set; }
    public string Text { get; private set; }
    public int Start { get; private set; }
    public int End { get; private set; }
    public string Title { get; private set; }

    #endregion

    public LanguageFold(int start, int end, string text, string startfold, string endfold, bool closed) : base(start, end)
    {
        Name = String.Format("{0}æ{1}", startfold, endfold);
        StartFold = startfold;
        EndFold = endfold;
        DefaultClosed = closed;
        Start = start;
        End = end;
        Text = text;
        var title = text;

        var p = title.IndexOf("\r\n", StringComparison.Ordinal);
        var n = title.IndexOf('%');

        if (n > -1)
            title = title.Substring(0, n);
        else if (p > -1)
            title = title.Substring(0, p);

        Title = title;

    }
}
