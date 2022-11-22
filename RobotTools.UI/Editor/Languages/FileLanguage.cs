using System;
using System.Collections.Generic;
using System.IO;
using RobotTools.UI.Editor.Languages.Robot;
using System.Collections.ObjectModel;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Snippets;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using System.ComponentModel;
using RobotTools.UI.Editor.Folding;
using RobotTools.UI.Utilities;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace RobotTools.UI.Editor.Languages
{
    public abstract class FileLanguage : ObservableObject
    {
        #region Constructors

        protected FileLanguage()
        {
            Instance = this;
            //RobotMenuItems=GetMenuItems();
        }

        protected const RegexOptions RO = (RegexOptions.IgnoreCase | RegexOptions.Multiline);

        protected FileLanguage(string filename)
        {
           
            string dir = Path.GetDirectoryName(filename);
            bool dirExists = dir != null && Directory.Exists(dir);
            string ext = Path.GetExtension(filename);
            string fn = Path.GetFileNameWithoutExtension(filename);
            Instance = this;

            /*
            if (this is KUKA && ext == ".sub")
            {
                SourceName = Path.GetFileName(filename);
            }
            else if ((this is KUKA) && ((ext == ".src") || (ext == ".dat")))
            {
                SourceName = fn + ".src";
                DataName = fn + ".dat";
            }
            else
            {
                SourceName = Path.GetFileName(filename);
                DataName = string.Empty;
            }

            if (SourceName != null && (dirExists && File.Exists(Path.Combine(dir, SourceName))))
                SourceText += File.ReadAllText(Path.Combine(dir, SourceName));


            if (DataName != null)
                if (dirExists && File.Exists(Path.Combine(dir, DataName)))
                    DataText += File.ReadAllText(Path.Combine(dir, DataName));

            RawText = SourceText + DataText;
            Instance = this;
            RobotMenuItems = GetMenuItems();
            */

           
           

            RawText = SourceText + DataText;
            Instance = this;
            RobotMenuItems = GetMenuItems();

        }

        #endregion

        #region Properties

        #region RootPath

        /// <summary>
        ///     The <see cref="RootPath" /> property's name.
        /// </summary>

        /// <summary>
        ///     Sets and gets the RootPath property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public DirectoryInfo RootPath { get; set; }

        #endregion

        #region FileName



        private string _filename = String.Empty;

        /// <summary>
        ///     Sets and gets the FileName property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string FileName { get; set; }

        #endregion



        internal readonly List<IVariable> _allVariables = new List<IVariable>();
        private readonly List<IVariable> _enums = new List<IVariable>();
        private readonly ReadOnlyCollection<IVariable> _readOnlyAllVariables = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlyFields = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlyFunctions = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlyenums = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlypositions = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlysignals = null;
        private readonly ReadOnlyCollection<IVariable> _readOnlystructures = null;
        private readonly List<IVariable> _signals = new List<IVariable>();
        private readonly List<IVariable> _structures = new List<IVariable>();
        private List<IVariable> _fields = new List<IVariable>();
        private List<IVariable> _functions = new List<IVariable>();
        private List<IVariable> _positions = new List<IVariable>();

        public string Name
        {
            get { return RobotType == "" ? String.Empty : RobotType.ToString(); }
        }

        private string RobotType = "";
        

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        public static int Progress { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        /// <summary>
        ///     Text of _files For searching
        /// </summary>
        public string RawText { private get; set; }

        public static FileLanguage Instance { get; set; } = new KukaLanguage();
        internal string SourceText { get; private set; }
        internal string DataText { get; private set; }

        // ReSharper disable once UnusedMember.Global
        public string SnippetPath
        {
            get { return ".\\Editor\\Config _files\\Snippet.xml"; }
        }

        // ReSharper disable once UnusedMember.Global
        public string Intellisense
        {
            get { return String.Concat(RobotType.ToString(), "Intellisense.xml"); }
        }

        // ReSharper disable once UnusedMember.Global
        public string SnippetFilePath
        {
            get { return String.Concat(RobotType.ToString(), "Snippets.xml"); }
        }

        public string Filename { get; set; }

        protected string ConfigFilePath
        {
            get { return String.Concat(RobotType.ToString(), "Config.xml"); }
        }

        internal string SyntaxHighlightFilePath
        {
            get { return String.Concat(RobotType.ToString(), "Highlight.xshd"); }
        }

        internal string StyleFilePath
        {
            get { return String.Concat(RobotType.ToString(), "Style.xshd"); }
        }

        public static int FileCount { get; private set; }

        public ReadOnlyCollection<IVariable> AllVariables
        {
            get { return _readOnlyAllVariables ?? new ReadOnlyCollection<IVariable>(_allVariables); }
        }

        public ReadOnlyCollection<IVariable> Functions
        {
            get { return _readOnlyFunctions ?? new ReadOnlyCollection<IVariable>(_functions); }
        }

        public ReadOnlyCollection<IVariable> Fields
        {
            get { return _readOnlyFields ?? new ReadOnlyCollection<IVariable>(_fields); }
        }

        public ReadOnlyCollection<IVariable> Positions
        {
            get { return _readOnlypositions ?? new ReadOnlyCollection<IVariable>(_positions); }
        }


        public ReadOnlyCollection<IVariable> Enums
        {
            get { return _readOnlyenums ?? new ReadOnlyCollection<IVariable>(_enums); }
        }


        public ReadOnlyCollection<IVariable> Structures
        {
            get { return _readOnlystructures ?? new ReadOnlyCollection<IVariable>(_structures); }
        }

        public ReadOnlyCollection<IVariable> Signals
        {
            get { return _readOnlysignals ?? new ReadOnlyCollection<IVariable>(_signals); }
        }

        #region Snippets

        private ReadOnlyObservableCollection<Snippet> _readOnlySnippets;

        public ReadOnlyObservableCollection<Snippet> Snippets
        {
            get
            {
                return _readOnlySnippets ??
                       (_readOnlySnippets = new ReadOnlyObservableCollection<Snippet>(GetSnippets()));
            }
        }

        public ObservableCollection<Snippet> GetSnippets()
        {
            throw new NotImplementedException();

        }

        #endregion

        #region Object Browser Variables

        private readonly ObservableCollection<IVariable> _objectBrowserVariables = new ObservableCollection<IVariable>();
        private readonly ReadOnlyObservableCollection<IVariable> _readOnlyBrowserVariables = null;

        public ReadOnlyObservableCollection<IVariable> ObjectBrowserVariable
        {
            get
            {
                return _readOnlyBrowserVariables ?? new ReadOnlyObservableCollection<IVariable>(_objectBrowserVariables);
            }
        }

        #endregion


        #region Files

        private readonly List<System.IO.FileInfo> _files = new List<System.IO.FileInfo>();
        private readonly ReadOnlyCollection<System.IO.FileInfo> _readOnlyFiles = null;

        public ReadOnlyCollection<System.IO.FileInfo> Files
        {
            get { return _readOnlyFiles ?? new ReadOnlyCollection<System.IO.FileInfo>(_files); }
        }

        #endregion

        #endregion

        #region  Members

        public abstract string CommentChar { get; }

        #endregion

        #region  Properties

        public List<string> SearchFilters { get; }


        protected abstract string ShiftRegex { get; }

        public abstract Regex MethodRegex { get; }

        /// <summary>
        ///     Regular Expression for finding Fields
        ///     <remarks> Used in Editor.FindBookmarks</remarks>
        /// </summary>
        public abstract Regex FieldRegex { get; }

        public abstract Regex EnumRegex { get; }

        public abstract Regex XYZRegex { get; }

        public abstract Regex StructRegex { get; }
        public abstract Regex SignalRegex { get; }

        /// <summary>
        ///     Regular Expression for Functions
        /// </summary>
        internal abstract string FunctionItems { get; }

        internal IList<ICompletionData> CodeCompletion { get; }
        public FoldingStrategy FoldingStrategy { get; set; }
        internal virtual bool IsFileValid(System.IO.FileInfo file)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region  Methods

        /// <summary>
        ///     Source file extension
        /// </summary>
// ReSharper disable once UnusedMember.Global
        internal string SourceFile { get; }


        // ReSharper disable once InconsistentNaming
        public virtual string ExtractXYZ(string positionstring)
        {
            throw new NotImplementedException();
        }
        //TODO Need to figure a way to use multiple extensions

        public virtual string FoldTitle(FoldingSection section, TextDocument doc)
        {
            throw new NotImplementedException();

        }

        #endregion


        /// <summary>
        ///     Strips Comment Character from string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual string CommentReplaceString(string text)
        {
            string pattern = string.Format("^([ ]*)([{0}]*)([^\r\n]*)", CommentChar);
            var rgx = new Regex(pattern);
            Match m = rgx.Match(text);
            if (m.Success)
            {
                return m.Groups[1] + m.Groups[3].ToString();
            }
            return text;
        }

        public virtual int CommentOffset(string text)
        {
            //TODO Create Result Regex
            var rgx = new Regex(@"(^[\s]+)");
            {
                Match m = rgx.Match(text);
                if (m.Success)
                    return m.Groups[1].Length;
                //return m.Groups[1].ToString()+ m.Groups[2].ToString();
            }


            return 0;
        }

        /// <summary>
        ///     Trims Line and Then Returns if first Character is a comment Character
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLineCommented(string text)
        {
            return text.Trim().IndexOf(CommentChar, StringComparison.Ordinal).Equals(0);
        }

        #region Folding Section

        private static bool IsValidFold(string text, string s, string e)
        {
            text = text.Trim();
            bool bSp = text.StartsWith(s);
            bool bEp = text.StartsWith(e);

            if (!(bSp | bEp)) return false;

            string lookfor = bSp ? s : e;

            //TODO Come Back and fix this
            if (text.Substring(text.IndexOf(lookfor, StringComparison.Ordinal) + lookfor.Length).Length == 0)
                return true;

            string cAfterString = text.Substring(text.IndexOf(lookfor, StringComparison.Ordinal) + lookfor.Length, 1);


            char cc = Convert.ToChar(cAfterString);
            bool isLetter = Char.IsLetterOrDigit(cc);

            return (!isLetter);
        }

        protected static IEnumerable<LanguageFold> CreateFoldingHelper(ITextSource document, string startFold,
            string endFold, bool defaultclosed)
        {
            var newFoldings = new List<LanguageFold>();
            var startOffsets = new Stack<int>();
            var doc = (document as TextDocument);
            endFold = endFold.ToLower();
#pragma warning disable 219
            int err = 0;
#pragma warning restore 219

            //TODO Instead of Parsing through lines, I may want to search the textrope

            if (doc != null)
                foreach (DocumentLine dd in doc.Lines)
                {
                    DocumentLine line = doc.GetLineByNumber(dd.LineNumber);
                    string text = doc.GetText(line.Offset, line.Length).ToLower();
                    string eval = text.TrimStart();

                    try
                    {
                        if (!IsValidFold(text, startFold, endFold))
                            continue;

                        if (eval.StartsWith(startFold))
                        {
                            startOffsets.Push(line.Offset);
                            continue;
                        }


                        if (eval.StartsWith(endFold) && startOffsets.Count > 0)
                        {
                            // Might Be for EndFolds
                            bool valid;
                            if (endFold == "end")
                            {
                                if (text.Length == endFold.Length)
                                    valid = true;
                                else
                                {
                                    char[] ee = text.ToCharArray(endFold.Length, 1);
                                    valid = !char.IsLetterOrDigit(ee[0]);
                                }
                            }
                            else
                                valid = true; // Not an End Statement
                            if (valid)
                            {
                                //Add a new folder to the list
                                int s = startOffsets.Pop();

                                int e = line.Offset + text.Length;

                                string str = doc.GetText(s + startFold.Length + 1, line.Offset - s - endFold.Length);


                                var nf = new LanguageFold(s, e, str, startFold, endFold, defaultclosed);
                                newFoldings.Add(nf);
                            }
                        }
                        else
                            err++;
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause
                    {
                        //TODO May want to put in messaging later about the folds
                        //                        MessageViewModel.AddError("LanguageClass.CreateFoldingHelper", ex);
                    }
                }

            return newFoldings;
        }

        #endregion



        private void GetVariables()
        {

        }





        //TODO Signal Path for KUKARegex currently displays linear motion
        private static IEnumerable<IVariable> FindMatches(Regex matchstring, string imgPath, string filepath)
        {
            //TODO Go Back and Change All Regex to be case insensitive

            var result = new List<IVariable>();
            try
            {
                string text = File.ReadAllText(filepath);

                // Dont Include Empty Values
                if (String.IsNullOrEmpty(matchstring.ToString())) return result;

                Match m = matchstring.Match(text);

                while (m.Success)
                {
                    result.Add(new Variable
                    {
                        Declaration = m.Groups[0].ToString(),
                        Offset = m.Index,
                        Type = m.Groups[1].ToString(),
                        Name = m.Groups[2].ToString(),
                        Value = m.Groups[3].ToString(),
                        Path = filepath,
                        Icon = ImageHelper.LoadBitmap(imgPath)
                    });
                    m = m.NextMatch();
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

     

        
        private int _bwProgress;
        /// <summary>
        ///     Sets and gets the BWProgress property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int BWProgress { get => _bwProgress; set => SetProperty(ref _bwProgress, value); }

        
        #region BWFilesMin

        private int _bwFilesMin;

        /// <summary>
        ///     Sets and gets the BWFilesMin property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int BWFilesMin { get => _bwFilesMin; set => SetProperty(ref _bwFilesMin, value); }

        #endregion

        #region BWFilesMax

        /// <summary>
        ///     The <see cref="BWFilesMax" /> property's name.
        /// </summary>
        public const string BWFilesMaxPropertyName = "BWFilesMax";

        private int _bwFilesMax;

        /// <summary>
        ///     Sets and gets the BWFilesMax property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int BWFilesMax { get => _bwFilesMax; set => SetProperty(ref _bwFilesMax, value); }

        #endregion
         
        /// <summary>
        ///     The <see cref="BWProgressVisibility" /> property's name.
        /// </summary>
        public const string BWProgressVisibilityPropertyName = "BWProgressVisibility";

        private Visibility _bwProgressVisibility = Visibility.Collapsed;

        /// <summary>
        ///     Sets and gets the BWProgressVisibility property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Visibility BWProgressVisibility { get => _bwProgressVisibility; set => SetProperty(ref _bwProgressVisibility, value); }

























 

        #region Properties
        
   
        #region RobotMenuItems
        /// <summary>
        /// The <see cref="RobotMenuItems" /> property's name.
        /// </summary>
    
        private MenuItem _robotMenuItems;

        /// <summary>
        /// Sets and gets the RobotMenuItems property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public MenuItem RobotMenuItems
        {
            get => _robotMenuItems;
            set=>SetProperty(ref _robotMenuItems,value);
        }
 
        #endregion






        private readonly ObservableCollection<MenuItem> _menuItems = new ObservableCollection<MenuItem>();
        readonly ReadOnlyObservableCollection<MenuItem> _readonlyMenuItems = null;
        public IEnumerable<MenuItem> MenuItems { get { return _readonlyMenuItems ?? new ReadOnlyObservableCollection<MenuItem>(_menuItems); } }


        #endregion

  

        private MenuItem GetMenuItems()
        {
            var rd = new ResourceDictionary { Source = new Uri("/RobotTools.UI;component/Editor/Templates/MenuDictionary.xaml", UriKind.RelativeOrAbsolute) };
            var i = rd[RobotType + "Menu"] as MenuItem ?? new MenuItem();
            return i;
        }

     
        


 



        // Try to Find Variables

        #region Automatic ObjectBrowser
        string _rootName = string.Empty;
        //TODO Split this up for a robot by robot basis
        private const string TargetDirectory = "KRC";
        bool _rootFound;
        public void GetRootDirectory(string dir)
        {
            //Search Backwards from current point to root directory
            var dd = new DirectoryInfo(dir);

            // Cannot Parse Directory
            if (dd.Name == dd.Root.Name) _rootFound = true;

            try
            {
                while (dd.Parent != null && ((!_rootFound) && (dd.Parent.Name != TargetDirectory)))
                {
                    GetRootDirectory(dd.Parent.FullName);
                }


                if (_rootFound) return;

                if (dd.Parent != null)
                    if (dd.Parent.Parent != null)
                        if (dd.Parent.Parent.Parent != null)
                            _rootName = dd.Parent != null && dd.Parent.Parent.Name != "C" ? dd.Parent.Parent.FullName : dd.Parent.Parent.Parent.FullName;

                var r = new DirectoryInfo(_rootName);

                var f = r.GetDirectories();


                if (f.Length < 1) return;
                if ((f[0].Name == "C") && (f[1].Name == "KRC"))
                    _rootName = r.FullName;

                _rootFound = true;

                GetRootFiles(_rootName);
                FileCount = Files.Count;

                GetVariables();
                _allVariables.AddRange(Functions);
                _allVariables.AddRange(Fields);
                _allVariables.AddRange(Positions);
                _allVariables.AddRange(Signals);


            }
            catch (Exception ex)
            {
                //var msg = new ErrorMessage("Get Root Directory", ex);
                //Messenger.Default.Send(msg);


            }

            // Need to get further to the root so that i can interrogate system files as well.
        }



        private string _kukaCon;
        private void GetRootFiles(string dir)
        {
            foreach (var d in Directory.GetDirectories(dir))
            {
                foreach (var f in Directory.GetFiles(d))
                {
                    try
                    {
                        var file = new System.IO.FileInfo(f);
                        if (file.Name.ToLower() == "kuka_con.mdb")
                            _kukaCon = file.FullName;
                        _files.Add(file);
                    }
                    catch (Exception e)
                    {
                        //var msg = new ErrorMessage("Error When Getting Files for Object Browser", e);
                        //Messenger.Default.Send(msg);
                    }

                }

                GetRootFiles(d);
            }
        }


        #region Properties for Background Worker and StatusBar

        

     

  

    
        BackgroundWorker _bw;

        #endregion
 

        void _bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BWProgress = e.ProgressPercentage;
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            OnPropertyChanged("Functions");
            OnPropertyChanged("Fields");
            OnPropertyChanged("Files");
            OnPropertyChanged("Positions");
            BWProgressVisibility = Visibility.Collapsed;

            // Dispose of Background worker
            _bw = null;
            //TODO Open Variable Monitor

        }

        void backgroundVariableWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BWFilesMax = Files.Count;
            var i = 0;
            _functions = new List<IVariable>();
            _fields = new List<IVariable>();
            _positions = new List<IVariable>();
            foreach (var f in Files)
            {

                // Check to see if file is ok to check for values
                if (IsFileValid(f))
                {
                    _functions.AddRange(FindMatches(MethodRegex, Global.ImgMethod, f.FullName));
                    _structures.AddRange(FindMatches(StructRegex, Global.ImgStruct, f.FullName));
                    _fields.AddRange(FindMatches(FieldRegex, Global.ImgField, f.FullName));
                    _signals.AddRange(FindMatches(SignalRegex, Global.ImgSignal, f.FullName));
                    _enums.AddRange(FindMatches(EnumRegex, Global.ImgEnum, f.FullName));
                    _positions.AddRange(FindMatches(XYZRegex, Global.ImgXyz, f.FullName));
                }
                i++;
                _bw.ReportProgress(i);
            }
        }


         
        #endregion
    }

}
