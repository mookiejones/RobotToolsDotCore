﻿using System.Windows.Input;
namespace RobotTools.UI.Editor.Bookmarks;

public interface IBookmark
{
    int LineNumber { get; }
    IImage Image { get; }
    int ZOrder { get; }
    bool CanDragDrop { get; }
    void MouseDown(MouseButtonEventArgs e);
    void MouseUp(MouseButtonEventArgs e);
    void Drop(int lineNumber);
}
