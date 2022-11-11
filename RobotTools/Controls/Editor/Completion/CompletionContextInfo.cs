﻿using System.Windows.Input;

namespace RobotTools.Controls.Editor.Completion
{
    public abstract class CompletionContextInfo
    {
        public CompletionType CompletionType
        {
            get;
            set;
        }
        public Key LastKeyPressed
        {
            get;
            private set;
        }
        public char LastChar
        {
            get;
            private set;
        }
        public string FilterText
        {
            get;
            private set;
        }
        public int LineNumber
        {
            get;
            set;
        }
        public string Path
        {
            get;
            set;
        }
        public int Column
        {
            get;
            set;
        }
        public string CaretLine
        {
            get;
            set;
        }
        public string LastToken
        {
            get;
            set;
        }
        public string StringBeforeCaret
        {
            get;
            set;
        }
        public string FirstWordInLine
        {
            get;
            set;
        }

        protected CompletionContextInfo(CompletionType type, string filter, Key lastKey, char lastChar)
        {
            CompletionType = type;
            FilterText = filter;
            LastKeyPressed = lastKey;
            LastChar = lastChar;
        }

    }
}
