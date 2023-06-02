using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using RobotTools.UI.Editor.Folding;
using RobotTools.UI.Editor.Completion;

namespace RobotTools.UI.Editor.Languages;

public sealed class LanguageBase : LanguageClass
{
    public LanguageBase()
    {
    }

    public LanguageBase(string file)
         
    {
    }

    public  List<string> SearchFilters
    {
        get { return DefaultSearchFilters; }
    }

    private static List<string> DefaultSearchFilters
    {
        get
        {
            return new List<string>
            {
                "*.*"
            };
        }
    }

   

    internal  string FunctionItems
    {
        get { return string.Empty; }
    }

    internal  FoldingStrategy FoldingStrategy { get; set; }

  
    internal  IList<ICompletionData> CodeCompletion
    {
        get
        {
            return new List<ICompletionData>
            {
                new CompletionData("Item1")
            };
        }
    }

    public  Regex MethodRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  Regex StructRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  Regex FieldRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  Regex EnumRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  void Initialize(string filename)
    {
        
    }

    public  string CommentChar
    {
        get { throw new NotImplementedException(); }
    }

    public  Regex SignalRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  Regex XYZRegex
    {
        get { return new Regex(string.Empty); }
    }

    public  bool IsFileValid(FileInfo file)
    {
        return false;
    }

  
    internal  string FoldTitle(FoldingSection section, TextDocument doc)
    {
        if (doc == null)
        {
            throw new ArgumentNullException("doc");
        }
        var array = Regex.Split(section.Title, "�");
        var offset = section.StartOffset + array[0].Length;
        var length = section.Length - array[0].Length;
        return doc.GetText(offset, length);
    }

 
}
