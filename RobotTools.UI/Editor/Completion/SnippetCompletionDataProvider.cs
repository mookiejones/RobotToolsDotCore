using ICSharpCode.AvalonEdit.CodeCompletion;
using RobotTools.UI.Editor.Snippets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace RobotTools.UI.Editor.Completion
{
    public sealed class SnippetCompletionDataProvider : ICompletionDataProvider, IDisposable
    {
        public IEnumerable<ICompletionData> ProvideData(CompletionContextInfo context)
        {


            if (context != null && context.Path != null && context.CompletionType != CompletionType.CompletionKey && context.CompletionType != CompletionType.ScopeChange)
            {
                var extension = Path.GetExtension(context.Path);
                if (extension != null && (extension.Equals(".src", StringComparison.InvariantCultureIgnoreCase) || extension.Equals(".dat", StringComparison.InvariantCultureIgnoreCase) || extension.Equals(".sub", StringComparison.InvariantCultureIgnoreCase)))
                {
                    if (context.StringBeforeCaret.Contains(";"))
                    {
                        goto IL_170;
                    }
                    var num = context.StringBeforeCaret.Count(c => c == '"');
                    if (num % 2 == 1)
                    {
                        goto IL_170;
                    }
                }
                foreach (var current in SnippetManager.GetCompletionDataForExtension(extension))
                {
                    yield return current;
                }
            }
        IL_170:
            yield break;
        }
        public void Dispose()
        {
        }


    }
}
