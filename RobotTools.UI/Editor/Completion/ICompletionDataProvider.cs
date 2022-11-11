using ICSharpCode.AvalonEdit.CodeCompletion;
using System;
using System.Collections.Generic;

namespace RobotTools.UI.Editor.Completion
{
    public interface ICompletionDataProvider : IDisposable
    {
        IEnumerable<ICompletionData> ProvideData(CompletionContextInfo context);
    }
}
