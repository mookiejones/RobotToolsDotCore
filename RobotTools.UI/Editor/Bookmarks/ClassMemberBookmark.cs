﻿using System.Windows.Input;
namespace RobotTools.UI.Editor.Bookmarks
{
    public sealed class ClassMemberBookmark : IBookmark
    {
        public ClassMemberBookmark(int lineNumber, IImage image)
        {
            Image = image;
            LineNumber = lineNumber;
        }

        public int LineNumber { get; private set; }
        public IImage Image { get; private set; }

        public int ZOrder
        {
            get { return -10; }
        }

        public bool CanDragDrop
        {
            get { return false; }
        }

        public void MouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
            }
        }

        public void MouseUp(MouseButtonEventArgs e)
        {
        }

        public void Drop(int lineNumber)
        {
        }
    }
}
