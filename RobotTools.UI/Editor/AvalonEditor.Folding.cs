
using System.ComponentModel;
using System.IO;
using System.Linq;
using ICSharpCode.AvalonEdit.Folding;
using RobotTools.UI.Editor.Folding;

namespace RobotTools.UI.Editor;

public partial class AvalonEditor
{
    #region Members
    private object _foldingStrategy;
    private FoldingManager _foldingManager;

    #endregion
    [Localizable(false)]
    private void UpdateFolds()
    {
        var editorOptions = EditorOptions.Instance;
        var flag = editorOptions != null && editorOptions.EnableFolding;
        if (SyntaxHighlighting == null)
        {
            _foldingStrategy = null;
        }
        if (File.Exists(Filename))
        {
            if (Path.GetExtension(Filename) == ".xml" || Path.GetExtension(Filename) == ".cfg")
            {
                _foldingStrategy = new XmlFoldingStrategy();
            }
            else
            {
                {
                    _foldingStrategy = new[] { ".sub", ".src", ".dat" }.Any(o=>o==Extension)
                        ? new KukaFoldingStrategy()
                        :new FoldingStrategy();

                }
                if (_foldingStrategy != null && flag)
                {
                    if (_foldingManager == null)
                    {
                        _foldingManager = FoldingManager.Install(TextArea);
                    }

                    var xmlStrategy = _foldingStrategy as XmlFoldingStrategy;
                    if (xmlStrategy != null)
                    {
                        xmlStrategy.UpdateFoldings(_foldingManager, Document);
                    }
                    else
                    {
                        ((FoldingStrategy)_foldingStrategy).UpdateFoldings(_foldingManager, Document);
                    }

                    RegisterFoldTitles();
                }
                else
                {
                    if (_foldingManager != null)
                    {
                        FoldingManager.Uninstall(_foldingManager);
                        _foldingManager = null;
                    }
                }
            }
        }
        ToggleAllFolds();
    }



    private void ToggleFolds()
    {
        if (_foldingManager == null) return;
        // Look for folding on this line: 
        var folding =
            _foldingManager.GetNextFolding(TextArea.Document.GetOffset(TextArea.Caret.Line,
                                                                       TextArea.Caret.Column));
        if (folding == null || Document.GetLineByOffset(folding.StartOffset).LineNumber != TextArea.Caret.Line)
        {
            // no folding found on current line: find innermost folding containing the caret
            folding = _foldingManager.GetFoldingsContaining(TextArea.Caret.Offset).LastOrDefault();
        }
        if (folding != null)
        {
            folding.IsFolded = !folding.IsFolded;
        }
    }

    private void ToggleAllFolds()
    {
        if (_foldingManager == null) return;
        foreach (var fm in _foldingManager.AllFoldings)
            fm.IsFolded = !fm.IsFolded;
    }

    void ChangeFoldStatus(bool isFolded)
    {
        foreach (var fm in _foldingManager.AllFoldings)
            fm.IsFolded = isFolded;
    }

    private void RegisterFoldTitles()
    {
        if ( !(Path.GetExtension(Filename) == ".xml"))
        {
            foreach (var current in _foldingManager.AllFoldings)
            {
                current.Title = FileLanguage.FoldTitle(current, Document);
            }
        }
    }
}
