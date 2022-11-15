using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.IO;

namespace RobotTools.UI.Editor
{
    public partial class AvalonEditor
    {
        public void SetHighlighting()
        {
            try
            {
                if (Filename != null)
                {
                    SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(Path.GetExtension(Filename));
                }
                else
                {
                    var opt = Options.EnableHyperlinks;
                    SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".src");
                }
            }
            catch (Exception )
            {

            }
        }
    }
}
