using System;
using System.Collections.Generic;
namespace RobotTools.UI.Editor.Bookmarks;

public interface IBookmarkMargin
{
    IList<IBookmark> Bookmarks { get; }
    event EventHandler RedrawRequested;
    void Redraw();
}
