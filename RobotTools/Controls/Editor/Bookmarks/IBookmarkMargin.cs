using System;
using System.Collections.Generic;
namespace RobotTools.Controls.Editor.Bookmarks
{
    public interface IBookmarkMargin
    {
        IList<IBookmark> Bookmarks { get; }
        event EventHandler RedrawRequested;
        void Redraw();
    }
}
