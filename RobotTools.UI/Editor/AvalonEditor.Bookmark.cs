using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using RobotTools.UI.Editor.Bookmarks;
using RobotTools.UI.Editor.Languages;
using RobotTools.UI.Editor.Languages.Robot;
using RobotTools.UI.Utilities;

namespace RobotTools.UI.Editor
{
    partial class AvalonEditor
    {
        private static BitmapImage _imgMethod;
        private static BitmapImage _imgStruct;
        private static BitmapImage _imgEnum;
        private static BitmapImage _imgSignal;
        private static BitmapImage _imgXyz;
        private static BitmapImage _imgField;

        private readonly ObservableCollection<IVariable> _variables = new ObservableCollection<IVariable>();


        private void CreateImages()
        {
            if (_imgMethod == null)
                _imgMethod = ImageHelper.LoadBitmap(Global.ImgMethod);

            if (_imgStruct == null)
                _imgStruct = ImageHelper.LoadBitmap(Global.ImgStruct);

            if (_imgEnum == null)
                _imgEnum = ImageHelper.LoadBitmap(Global.ImgEnum);

            if (_imgSignal == null)
                _imgSignal = ImageHelper.LoadBitmap(Global.ImgSignal);

            if (_imgXyz == null)
                _imgXyz = ImageHelper.LoadBitmap(Global.ImgXyz);
        }

        /// <summary>
        /// Add Bookmark
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <param name="img"></param>
        private void AddBookMark(int lineNumber, BitmapImage img)
        {
            var image = new BookmarkImage(img);
            _iconBarManager.Bookmarks.Add(new ClassMemberBookmark(lineNumber, image));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchstring"></param>
        /// <param name="img"></param>
        private void FindMatches(Regex matchstring, BitmapImage img)
        {
            if (!string.IsNullOrEmpty(matchstring.ToString()))
            {
                // Was To lower invariant
                var match = matchstring.Match(Text);

                while (match.Success)
                {
                    _variables.Add(new Variable
                    {
                        Declaration = match.Groups[0].ToString(),
                        Offset = match.Index,
                        Type = match.Groups[1].ToString(),
                        Name = match.Groups[2].ToString(),
                        Value = match.Groups[3].ToString(),
                        Path = Filename,
                        Icon = img
                    });
                    var lineByOffset = Document.GetLineByOffset(match.Index);
                    AddBookMark(lineByOffset.LineNumber, img);
                    match = match.NextMatch();
                }
                if (FileLanguage is KukaLanguage)
                {
                    match =
                        matchstring.Match(Text);
                    while (match.Success)
                    {
                        _variables.Add(new Variable
                        {
                            Declaration = match.Groups[0].ToString(),
                            Offset = match.Index,
                            Type = match.Groups[1].ToString(),
                            Name = match.Groups[2].ToString(),
                            Value = match.Groups[3].ToString(),
                            Path = Filename,
                            Icon = img
                        });
                        match = match.NextMatch();
                    }
                }
            }
        }



        private void FindBookmarkMembers()
        {
            CreateImages();

            if (FileLanguage != null)
            {
                _iconBarManager.Bookmarks.Clear();
                _variables.Clear();
                FindMatches(FileLanguage.MethodRegex, _imgMethod);
                FindMatches(FileLanguage.StructRegex, _imgStruct);
                FindMatches(FileLanguage.FieldRegex, _imgField);
                FindMatches(FileLanguage.SignalRegex, _imgSignal);
                FindMatches(FileLanguage.EnumRegex, _imgEnum);
                FindMatches(FileLanguage.XYZRegex, _imgXyz);
            }
        }
    }
}
