﻿namespace RobotTools.UI.Editor.Bracket
{
    public sealed class BracketSearchResult
    {
        public BracketSearchResult(int openingBracketOffset, int openingBracketLength, int closingBracketOffset,
            int closingBracketLength)
        {
            OpeningBracketOffset = openingBracketOffset;
            OpeningBracketLength = openingBracketLength;
            ClosingBracketOffset = closingBracketOffset;
            ClosingBracketLength = closingBracketLength;
        }

        public int OpeningBracketOffset { get; private set; }
        public int OpeningBracketLength { get; private set; }
        public int ClosingBracketOffset { get; private set; }
        public int ClosingBracketLength { get; private set; }
    }
}
