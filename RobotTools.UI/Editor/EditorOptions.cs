using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Xml;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using RobotTools.Core.Utilities;

namespace RobotTools.UI.Editor
{
    [Localizable(false)]
    [Serializable]
    public sealed class EditorOptions : TextEditorOptions
    {
        private static EditorOptions _instance;
        [NonSerialized] private bool _allowScrollingBelowDocument;
        [NonSerialized] private Color _backgroundColor = Colors.White;
        private Color _borderColor = Colors.Transparent;

        private double _borderThickness;
        private bool _enableAnimations = true;
        private bool _enableFolding = true;
        private Color _foldToolTipBackgroundBorderColor = Colors.WhiteSmoke;
        [NonSerialized] private Color _foldToolTipBackgroundColor = Colors.Red;
        private double _foldToolTipBorderThickness = 1.0;
        [NonSerialized] private Color _fontColor = Colors.Black;
        private bool _highlightcurrentline = true;
        [NonSerialized] private Color _lineNumbersFontColor = Colors.Gray;
        private Color _lineNumbersForeground = Colors.Gray;
        private bool _mouseWheelZoom = true;
        [NonSerialized] private Color _selectedBorderColor = Colors.Orange;
        private double _selectedBorderThickness = 1.0;
        [NonSerialized] private Color _selectedFontColor = Colors.White;
        [NonSerialized] private Color _selectedTextBackground = Colors.SteelBlue;
        [NonSerialized] private Color _selectedTextBorderColor = Colors.Orange;
        [NonSerialized] private Color _selectedlinecolor = Colors.Yellow;
        private bool _showlinenumbers = true;
        private string _timestampFormat = "ddd MMM d hh:mm:ss yyyy";
        private bool _wrapWords = true;

        public EditorOptions()
        {
            RegisterSyntaxHighlighting();
        }

        private static string GetOptionsPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
            var directory = Path.Combine(appDataPath, "RobotTools");
            Directory.CreateDirectory(directory);
            var result = Path.Combine(directory, "Options.xml");
            return result; //Path.Combine(Global.StartupPath, "Options.xml"); }
        }
        private static string _optionsPath;
        private static string OptionsPath
        {
            get => _optionsPath ?? (_optionsPath = GetOptionsPath()); 
        }


        public static EditorOptions Instance
        {
            get { return _instance ?? (_instance = ReadXml()); }
            set { _instance = value; }
        }

     

        public Color SelectedTextBackground
        {
            get { return _selectedTextBackground; }
            set
            {
                _selectedTextBackground = value;
                OnPropertyChanged(nameof(SelectedTextBackground));
            }
        }

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged(nameof(BackgroundColor));
            }
        }

        public Color FontColor
        {
            get { return _fontColor; }
            set
            {
                _fontColor = value;
                OnPropertyChanged(nameof(FontColor));
            }
        }

        public Color SelectedFontColor
        {
            get { return _selectedFontColor; }
            set
            {
                _selectedFontColor = value;
                OnPropertyChanged(nameof(SelectedFontColor));
            }
        }

        public Color SelectedBorderColor
        {
            get { return _selectedBorderColor; }
            set
            {
                _selectedBorderColor = value;
                OnPropertyChanged(nameof(SelectedBorderColor));
            }
        }

        public bool AllowScrollingBelowDocument
        {
            get { return _allowScrollingBelowDocument; }
            set
            {
                _allowScrollingBelowDocument = value;
                OnPropertyChanged(nameof(AllowScrollingBelowDocument));
            }
        }

        public Color LineNumbersFontColor
        {
            get { return _lineNumbersFontColor; }
            set
            {
                _lineNumbersFontColor = value;
                OnPropertyChanged(nameof(LineNumbersFontColor));
            }
        }

        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        public Color LineNumbersForeground
        {
            get { return _lineNumbersForeground; }
            set
            {
                _lineNumbersForeground = value;
                OnPropertyChanged(nameof(LineNumbersForeground));
            }
        }

        public Color SelectedTextBorderColor
        {
            get { return _selectedTextBorderColor; }
            set
            {
                _selectedTextBorderColor = value;
                OnPropertyChanged(nameof(SelectedTextBorderColor));
            }
        }

        public double SelectedBorderThickness
        {
            get { return _selectedBorderThickness; }
            set
            {
                _selectedBorderThickness = value;
                OnPropertyChanged(nameof(SelectedBorderThickness));
            }
        }

        public double BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = value;
                OnPropertyChanged(nameof(BorderThickness));
            }
        }

        public Color HighlightedLineColor
        {
            get { return _selectedlinecolor; }
            set
            {
                _selectedlinecolor = value;
                OnPropertyChanged(nameof(HighlightedLineColor));
            }
        }

        public Color FoldToolTipBackgroundColor
        {
            get { return _foldToolTipBackgroundColor; }
            set
            {
                _foldToolTipBackgroundColor = value;
                OnPropertyChanged(nameof(FoldToolTipBackgroundColor));
            }
        }

        public Color FoldToolTipBackgroundBorderColor
        {
            get { return _foldToolTipBackgroundBorderColor; }
            set
            {
                _foldToolTipBackgroundBorderColor = value;
                OnPropertyChanged(nameof(FoldToolTipBackgroundBorderColor));
            }
        }

        public double FoldToolTipBorderThickness
        {
            get { return _foldToolTipBorderThickness; }
            set
            {
                _foldToolTipBorderThickness = value;
                OnPropertyChanged(nameof(FoldToolTipBorderThickness));
            }
        }

        public bool WrapWords
        {
            get { return _wrapWords; }
            set
            {
                _wrapWords = value;
                OnPropertyChanged(nameof(WrapWords));
            }
        }

        public string TimestampFormat
        {
            get { return _timestampFormat; }
            set
            {
                _timestampFormat = value;
                OnPropertyChanged(nameof(TimestampFormat));
                OnPropertyChanged(nameof(TimestampSample));
            }
        }

        public string TimestampSample
        {
            get { return DateTime.Now.ToString(_timestampFormat); }
        }

        public new bool HighlightCurrentLine
        {
            get { return _highlightcurrentline; }
            set { _highlightcurrentline = value; }
        }

        [DefaultValue(true)]
        public bool EnableFolding
        {
            get { return _enableFolding; }
            set
            {
                _enableFolding = value;
                OnPropertyChanged(nameof(EnableFolding));
            }
        }

        [DefaultValue(true)]
        public bool MouseWheelZoom
        {
            get { return _mouseWheelZoom; }
            set
            {
                if (_mouseWheelZoom != value)
                {
                    _mouseWheelZoom = value;
                    OnPropertyChanged(nameof(MouseWheelZoom));
                }
            }
        }

        public bool EnableAnimations
        {
            get { return _enableAnimations; }
            set
            {
                _enableAnimations = value;
                OnPropertyChanged(nameof(EnableAnimations));
            }
        }

        public bool ShowLineNumbers
        {
            get { return  _showlinenumbers; }
            set
            {
                _showlinenumbers = value;
                OnPropertyChanged(nameof(ShowLineNumbers));
            }
        }

        public string Title
        {
            get { return "Text Editor Options"; }
        }

        ~EditorOptions()
        {
            WriteXml();
        }

        private void WriteXml()
        {
            this.Serialize(OptionsPath);

          
        }

        private static EditorOptions ReadXml()
        {
             
            EditorOptions result;
            if (!File.Exists(OptionsPath))
            {
                result =  CreateOptions(); 
            }
            else
            {
                var text = File.ReadAllText(OptionsPath);
                result=text.FromXml<EditorOptions>();
               
               
            }
            return result;
        }
        private static EditorOptions CreateOptions()
        {
            var result = new EditorOptions();
            result.Serialize(OptionsPath);
            return result;
        }

        [Localizable(false)]
        private static void Register(string name, string[] ext)
        {
            var filename = string.Format("RobotTools.UI.Editor.SyntaxHighlighting.{0}Highlight.xshd", name);
            using (var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(filename))
            {
                if (manifestResourceStream == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }
                IHighlightingDefinition highlighting;
                using (var xmlTextReader = new XmlTextReader(manifestResourceStream))
                {
                    highlighting = HighlightingLoader.Load(xmlTextReader, HighlightingManager.Instance);
                }
                HighlightingManager.Instance.RegisterHighlighting(name, ext, highlighting);
            }
        }

        private static void RegisterSyntaxHighlighting()
        {
            Register("Kuka", new[] { ".src", ".sub", ".dat" });
            // Register("KAWASAKI", Kawasaki.EXT.ToArray());
            // Register("Fanuc", Fanuc.EXT.ToArray());
            // Register("ABB", ABB.EXT.ToArray());
        }
    }
}
