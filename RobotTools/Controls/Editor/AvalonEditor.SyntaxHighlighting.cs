using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.IO;

namespace RobotTools.Controls.Editor
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
                    var opt = TextOptions.EnableHyperlinks;
                    SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".src");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
