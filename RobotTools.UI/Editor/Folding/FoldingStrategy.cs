using System.Collections.Generic;
using System.Windows.Forms;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using RobotTools.UI.Editor.Languages.Robot;

namespace RobotTools.UI.Editor.Folding
{
    public  class FoldingStrategy 
    {
        public void UpdateFoldings(FoldingManager manager, TextDocument document)
        {
            int firstErrorOffset;
            var newFoldings = CreateNewFoldings(document, out firstErrorOffset);
            manager.UpdateFoldings(newFoldings, firstErrorOffset);
        }


        private bool FoldFunctions
        {
            get;
            set;
        }

        private bool FoldsClosedByDefault
        {
            get;
            set;
        }
        public FoldingStrategy()
        {
            FoldFunctions = true;
        }
        public FoldingStrategy(bool foldFunctions)
        {
            FoldFunctions = foldFunctions;
        }


        protected virtual IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            var list = new List<NewFolding>();
            firstErrorOffset = -1;
            var stack = new Stack<DocumentLine>();
            var stack2 = new Stack<DocumentLine>();
            var flag = false;
            foreach (var current in document.Lines)
            {
                var input = document.GetText(current).ToLower().TrimEnd(new[]
                {
                    ' ',
                    '\t'
                }).TrimStart(new[]
                {
                    ' ',
                    '\t'
                });
                if (FoldFunctions && KrlRegularExpressions.DefLineRegex.IsMatch(input))
                {
                    stack.Push(current);
                    flag = true;
                }
                if (FoldFunctions && KrlRegularExpressions.EndDefLineRegex.IsMatch(input) && stack.Count > 0)
                {
                    var endOffset = current.EndOffset;
                    var documentLine = stack.Pop();
                    var name = document.GetText(documentLine).TrimStart(new[]
                    {
                        ' ',
                        '\t'
                    }).TrimEnd(new[]
                    {
                        ' ',
                        '\t'
                    });
                    list.Add(new NewFolding(documentLine.Offset, endOffset)
                    {
                        Name = name
                    });
                    flag = false;
                }
                if (KrlRegularExpressions.FoldStartLineRegex.IsMatch(input) && (flag | !FoldFunctions))
                {
                    stack2.Push(current);
                }
                if (KrlRegularExpressions.FoldEndLineRegex.IsMatch(input) && stack2.Count > 0 && (flag | !FoldFunctions))
                {
                    var endOffset = current.EndOffset;
                    var documentLine2 = stack2.Pop();
                    var text = document.GetText(documentLine2).TrimStart(new[]
                    {
                        ' ',
                        '\t'
                    }).TrimEnd(new[]
                    {
                        ' ',
                        '\t'
                    }).ToUpper();
                    text = text.Replace(";FOLD", string.Empty).TrimStart(new[]
                    {
                        ' ',
                        '\t'
                    });
                    var num = text.IndexOf(';');
                    if (num > 0)
                    {
                        text = text.Remove(num);
                    }
                    list.Add(new NewFolding(documentLine2.Offset, endOffset)
                    {
                        Name = text,
                        DefaultClosed = FoldsClosedByDefault
                    });
                }
         

            }
            list.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
            return list;
        }
    }
}


