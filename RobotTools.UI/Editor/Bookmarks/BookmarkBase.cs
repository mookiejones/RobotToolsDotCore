﻿using System;
using System.Windows.Input;
using ICSharpCode.AvalonEdit.Document;
namespace RobotTools.UI.Editor.Bookmarks
{
    public class BookmarkBase : IBookmark
    {
        public static readonly IImage defaultBookmarkImage = null;
        private IEditor _document;
        private TextLocation _location;

        public BookmarkBase(TextLocation location)
        {
            Location = location;
        }

        public IEditor Document
        {
            get { return _document; }
            set
            {
                if (_document != value)
                {
                    if (Anchor != null)
                    {
                        throw new NotImplementedException();
                        //                        _location = Anchor.Location;
                        Anchor = null;
                    }
                    _document = value;
                    CreateAnchor();
                    OnDocumentChanged(EventArgs.Empty);
                }
            }
        }

        public ITextAnchor Anchor { get; private set; }

        public TextLocation Location
        {
            get
            {
                throw new NotImplementedException();
                //                return (Anchor != null) ? Anchor.Location : _location;
            }
            set
            {
                _location = value;
                CreateAnchor();
            }
        }

        public int ColumnNumber
        {
            get { return (Anchor != null) ? Anchor.Column : _location.Column; }
        }

        public virtual bool CanToggle
        {
            get { return true; }
        }

        public static IImage DefaultBookmarkImage
        {
            get { return defaultBookmarkImage; }
        }

        public int LineNumber
        {
            get { return (Anchor != null) ? Anchor.Line : _location.Line; }
        }

        public virtual int ZOrder
        {
            get { return 0; }
        }

        public virtual IImage Image
        {
            get { return defaultBookmarkImage; }
        }

        public virtual bool CanDragDrop
        {
            get { return false; }
        }

        public virtual void MouseDown(MouseButtonEventArgs e)
        {
        }

        public virtual void MouseUp(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && CanToggle)
            {
                RemoveMark();
                e.Handled = true;
            }
        }

        public virtual void Drop(int lineNumber)
        {
        }

        public event EventHandler DocumentChanged;

        private void CreateAnchor()
        {
            if (_document != null)
            {
                var num = Math.Max(1, Math.Min(_location.Line, _document.TotalNumberOfLines));
                var length = _document.GetLine(num).Length;
                var offset = _document.PositionToOffset(num, Math.Max(1, Math.Min(_location.Column, length + 1)));
                Anchor = _document.CreateAnchor(offset);
                Anchor.MovementType = AnchorMovementType.AfterInsertion;
                Anchor.Deleted += AnchorDeleted;
            }
            else
            {
                Anchor = null;
            }
        }

        private void AnchorDeleted(object sender, EventArgs e)
        {
            //            _location = Location.Empty;
            Anchor = null;
            RemoveMark();
        }

        protected virtual void RemoveMark()
        {
            if (_document != null)
            {
                var bookmarkMargin = _document.GetService(typeof(IBookmarkMargin)) as IBookmarkMargin;
                if (bookmarkMargin != null)
                {
                    bookmarkMargin.Bookmarks.Remove(this);
                }
            }
        }

        protected virtual void OnDocumentChanged(EventArgs e)
        {
            if (DocumentChanged != null)
            {
                DocumentChanged(this, e);
            }
        }

        protected virtual void Redraw()
        {
            if (_document != null)
            {
                var bookmarkMargin = _document.GetService(typeof(IBookmarkMargin)) as IBookmarkMargin;
                if (bookmarkMargin != null)
                {
                    bookmarkMargin.Redraw();
                }
            }
        }
    }
}
