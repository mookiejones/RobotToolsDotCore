using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Snippets;
using ICSharpCode.AvalonEdit;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using RobotTools.UI.Editor.Folding;
using RobotTools.UI.Utilities;
using RobotTools.UI.Editor.Snippets;
using RobotTools.UI.Editor.Languages.Robot;

namespace RobotTools.UI.Editor.Languages;

internal class KukaLanguage:FileLanguage
{
    public KukaLanguage()
        : base()
    {

    }

    public KukaLanguage(string name)
        : base(name) {
        FoldingStrategy = new RegionFoldingStrategy();
        //      FoldingStrategy = new KrlFoldingStrategy();

    }



    private RelayCommand _systemFunctionCommand;

   

 

    public static List<string> Ext
    {
        get
        {
            return new List<string>
            {
                ".dat",
                ".src",
                ".ini",
                ".sub",
                ".zip",
                ".kfd"
            };
        }
    }


    public string Comment { get; set; }

   
     

    public new MenuItem MenuItems
    {
        get
        {
            var menuItem = new MenuItem
            {
                Header = "KUKA"
            };
            var newItem = new MenuItem
            {
                Header = "Test 456"
            };
            menuItem.Items.Add(newItem);
            return menuItem;
        }
    }

    private static Snippet ForSnippet
    {
        get
        {
            return new Snippet
            {
                Elements =
                {
                    new SnippetTextElement
                    {
                        Text = "for "
                    },
                    new SnippetReplaceableTextElement
                    {
                        Text = "item"
                    },
                    new SnippetTextElement
                    {
                        Text = " in range("
                    },
                    new SnippetReplaceableTextElement
                    {
                        Text = "from"
                    },
                    new SnippetTextElement
                    {
                        Text = ", "
                    },
                    new SnippetReplaceableTextElement
                    {
                        Text = "to"
                    },
                    new SnippetTextElement
                    {
                        Text = ", "
                    },
                    new SnippetReplaceableTextElement
                    {
                        Text = "step"
                    },
                    new SnippetTextElement
                    {
                        Text = "):backN\t"
                    },
                    new SnippetSelectionElement()
                }
            };
        }
    }


    public override Regex EnumRegex
    {
        get
        {
            return new Regex("^(ENUM)\\s+([\\d\\w]+)\\s+([\\d\\w,]+)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
        }
    }

    public override Regex StructRegex=> new Regex("DECL STRUC|^STRUC\\s([\\w\\d]+\\s*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        

    public override Regex FieldRegex=>new Regex(
                    "^[DECL ]*[GLOBAL ]*[CONST ]*(INT|REAL|BOOL|CHAR)\\s+([\\$0-9a-zA-Z_\\[\\],\\$]+)=?([^\\r\\n;]*);?([^\\r\\n]*)",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);

    protected override string ShiftRegex => "((E6POS [\\w]*={)X\\s([\\d.-]*)\\s*,*Y\\s*([-.\\d]*)\\s*,Z\\s*([-\\d.]*))"; 

    public override Regex MethodRegex
    => new Regex("^[GLOBAL ]*(DEF)+\\s+([\\w_\\d]+\\s*)\\(", RegexOptions.IgnoreCase | RegexOptions.Multiline);

    internal override string FunctionItems=>"((DEF|DEFFCT (BOOL|CHAR|INT|REAL|FRAME)) ([\\w_\\s]*)\\(([\\w\\]\\s:_\\[,]*)\\))"; 
    

    public override string CommentChar        => ";";

    public override Regex SignalRegex
    => new Regex("^(SIGNAL+)\\s+([\\d\\w]+)\\s+([^\\r\\;]*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

    public override Regex XYZRegex
    => new Regex("^[DECL ]*[GLOBAL ]*(POS|E6POS|E6AXIS|FRAME) ([\\w\\d_\\$]+)=?\\{?([^}}]*)?\\}?", RegexOptions.IgnoreCase | RegexOptions.Multiline);


   
    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            Dispose(true);
        }
    }

    public static bool OnlyDatExists(string filename)
    {
        return
            File.Exists(Path.Combine(Path.GetDirectoryName(filename),
                Path.GetFileNameWithoutExtension(filename) + ".src"));
    }

    [Localizable(false)]
    public static string SystemFileName()
    {
        string result;
        using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
        {
            openFileDialog.Filter = "All File (*.*)|*.*";
            openFileDialog.InitialDirectory = "C:\\krc\\bin\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                result = openFileDialog.FileName;
                return result;
            }
        }
        result = string.Empty;
        return result;
    }

    internal override bool IsFileValid(System.IO.FileInfo file)
    {
        return FileIsValid(file);
    }


    internal bool FileIsValid(System.IO.FileInfo file)
    {
        foreach (var ext in Ext)
        {
            if (file.Extension.ToLower() == ext)
                return true;
        }
        return false;
    }
    private static Collection<string> GetPositionFromFile(int line, ITextEditorComponent editor)
    {
        var collection = new Collection<string>();
        while (true)
        {
            collection.Add(editor.Document.Lines[line].ToString());
            line++;
        }

        return collection;
    }

  

    public override string FoldTitle(FoldingSection section, TextDocument doc)
    {
        var array = Regex.Split(section.Title, "�");
        var text = section.TextContent.ToLower().Trim();
        var text2 = section.TextContent.Trim();
        var num = section.TextContent.Trim().IndexOf("%{PE}%", StringComparison.Ordinal) - "%{PE}%".Length;
        var num2 = section.TextContent.Trim().IndexOf("\r\n", StringComparison.Ordinal);
        var num3 = section.StartOffset + array[0].Length;
        //text2 = text2.Substring(text.IndexOf(array[0], StringComparison.Ordinal) + array[0].Length);

        var foldIndex = text2.IndexOf(";fold", StringComparison.InvariantCultureIgnoreCase);
        if(foldIndex != -1)
        {
            text2 = text2.Substring(foldIndex + 5);
        }
        var num4 = text2.Length - array[0].Length;
        if (num > -1)
        {
            num4 = ((num < num2) ? num : num4);
        }
        var result =  text2.Substring(0, num4);
        return result;
    }


    private static void GetInfo()
    {
    }

    public SnippetCollection Snippets()
    {
        return new SnippetCollection
        {
            ForSnippet
        };
    }

    public override string ExtractXYZ(string positionstring)
    {
        var positionBase = new PositionBase(positionstring);
        return positionBase.ExtractFromMatch();
    }

    public static string GetDatFileName(string filename)
    {
        return filename.Substring(0, filename.LastIndexOf('.')) + ".dat";
    }

    public static List<string> GetModuleFileNames(string filename)
    {
        var str = filename.Substring(0, filename.LastIndexOf('.'));
        var list = new List<string>();
        if (File.Exists(str + ".src"))
        {
            list.Add(str + ".src");
        }
        if (File.Exists(str + ".dat"))
        {
            list.Add(str + ".dat");
        }
        return list;
    }


    private sealed class RegionFoldingStrategy : FoldingStrategy
    {
        protected override IEnumerable<NewFolding> CreateNewFoldings(TextDocument document, out int firstErrorOffset)
        {
            firstErrorOffset = -1;
            var list = new List<LanguageFold>();
            list.AddRange(CreateFoldingHelper(document, ";fold", ";endfold", true));
            list.AddRange(CreateFoldingHelper(document, "def", "end", true));
            list.AddRange(CreateFoldingHelper(document, "global def", "end", true));
            list.AddRange(CreateFoldingHelper(document, "global deffct", "endfct", true));
            list.AddRange(CreateFoldingHelper(document, "deftp", "endtp", true));
            list.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
            return list;
        }
    }
}
