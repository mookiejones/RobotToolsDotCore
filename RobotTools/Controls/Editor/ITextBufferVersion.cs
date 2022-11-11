﻿using ICSharpCode.AvalonEdit.Document;
using System.Collections.Generic;

namespace RobotTools.Controls.Editor
{
    public interface ITextBufferVersion
    {
        bool BelongsToSameDocumentAs(ITextBufferVersion other);
        int CompareAge(ITextBufferVersion other);
        IEnumerable<TextChangeEventArgs> GetChangesTo(ITextBufferVersion other);
        int MoveOffsetTo(ITextBufferVersion other, int oldOffset, AnchorMovementType movement);
    }
}
