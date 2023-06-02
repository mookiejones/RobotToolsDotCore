using System;

namespace RobotTools.UI.Editor;

public interface ITextEditorCaret
{
    int Offset { get; set; }
    int Line { get; set; }
    int Column { get; set; }
    Location Position { get; set; }
    event EventHandler PositionChanged;
}
