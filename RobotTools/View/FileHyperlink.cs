using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RobotTools.View
{
    /// <summary>
    /// Interaction logic for FileHyperlink.xaml
    /// </summary>
    public partial class FileHyperlink : UserControl
    {
        #region fields
        private static readonly DependencyProperty NavigateUriProperty =
          DependencyProperty.Register("NavigateUri", typeof(string), typeof(FileHyperlink));

        private static readonly DependencyProperty TextProperty =
          DependencyProperty.Register("Text", typeof(string), typeof(FileHyperlink));

        private static RoutedCommand mCopyUri;
        private static RoutedCommand mNavigateToUri;
        private static RoutedCommand mOpenContainingFolder;

        private System.Windows.Documents.Hyperlink mHypLink;
        #endregion fields

        #region constructor
        static FileHyperlink()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileHyperlink),
                      new FrameworkPropertyMetadata(typeof(FileHyperlink)));

            mCopyUri = new RoutedCommand("CopyUri", typeof(FileHyperlink));

            CommandManager.RegisterClassCommandBinding(typeof(FileHyperlink), new CommandBinding(mCopyUri, CopyHyperlinkUri));
            CommandManager.RegisterClassInputBinding(typeof(FileHyperlink), new InputBinding(mCopyUri, new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl-C")));

            mNavigateToUri = new RoutedCommand("NavigateToUri", typeof(FileHyperlink));
            CommandManager.RegisterClassCommandBinding(typeof(FileHyperlink), new CommandBinding(mNavigateToUri, Hyperlink_CommandNavigateTo));
            ////CommandManager.RegisterClassInputBinding(typeof(FileHyperlink), new InputBinding(mCopyUri, new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl-C")));

            mOpenContainingFolder = new RoutedCommand("OpenContainingFolder", typeof(FileHyperlink));
            CommandManager.RegisterClassCommandBinding(typeof(FileHyperlink), new CommandBinding(mOpenContainingFolder, Hyperlink_OpenContainingFolder));
        }

        public FileHyperlink()
        {
            mHypLink = null;
        }
        #endregion constructor

        #region properties
        public static RoutedCommand CopyUri
        {
            get
            {
                return mCopyUri;
            }
        }

        public static RoutedCommand NavigateToUri
        {
            get
            {
                return mNavigateToUri;
            }
        }

        public static RoutedCommand OpenContainingFolder
        {
            get
            {
                return mOpenContainingFolder;
            }
        }

        /// <summary>
        /// Declare NavigateUri property to allow a user who clicked
        /// on the dispalyed Hyperlink to navigate their with their installed browser...
        /// </summary>
        public string NavigateUri
        {
            get { return (string)GetValue(NavigateUriProperty); }
            set { SetValue(NavigateUriProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region Methods
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            mHypLink = GetTemplateChild("PART_Hyperlink") as System.Windows.Documents.Hyperlink;
            Debug.Assert(mHypLink != null, "No Hyperlink in ControlTemplate!");

            // Attach hyperlink event clicked event handler to Hyperlink ControlTemplate if there is no command defined
            // Commanding allows calling commands that are external to the control (application commands) with parameters
            // that can differ from whats available in this control (using converters and what not)
            //
            // Therefore, commanding overrules the Hyperlink.Clicked event when it is defined.
            if (mHypLink != null)
            {
                if (mHypLink.Command == null)
                    mHypLink.RequestNavigate += Hyperlink_RequestNavigate;
            }
        }

        /// <summary>
        /// Process command when a hyperlink has been clicked.
        /// Start a web browser and let it browse to where this points to...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Hyperlink_CommandNavigateTo(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null || e == null) return;

            e.Handled = true;

            FileHyperlink whLink = sender as FileHyperlink;

            if (whLink == null) return;

            try
            {
                Process.Start(new ProcessStartInfo(whLink.NavigateUri));
                ////OpenFileLocationInWindowsExplorer(whLink.NavigateUri.OriginalString);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("{0}\n'{1}'.", ex.Message, (whLink.NavigateUri == null ? string.Empty : whLink.NavigateUri.ToString())),
                                              "Error finding requested resource", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static void Hyperlink_OpenContainingFolder(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null || e == null) return;

            e.Handled = true;

            FileHyperlink whLink = sender as FileHyperlink;

            if (whLink == null) return;

            OpenFileLocationInWindowsExplorer(whLink.NavigateUri);
        }

        /// <summary>
        /// A hyperlink has been clicked. Start a web browser and let it browse to where this points to...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CopyHyperlinkUri(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null || e == null) return;

            e.Handled = true;

            FileHyperlink whLink = sender as FileHyperlink;

            if (whLink == null) return;

            try
            {
                Clipboard.SetText(whLink.NavigateUri);
            }
            catch
            {
            }
        }

        /// <summary>
        /// A hyperlink has been clicked. Start a web browser and let it browse to where this points to...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(NavigateUri));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("{0}\n'{1}'.", ex.Message, (NavigateUri == null ? string.Empty : NavigateUri.ToString())),
                                "Error finding requested resource", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Convinience method to open Windows Explorer with a selected file (if it exists).
        /// Otherwise, Windows Explorer is opened in the location where the file should be at.
        /// </summary>
        /// <param name="oFileName"></param>
        /// <returns></returns>
        public static bool OpenFileLocationInWindowsExplorer(object oFileName)
        {
            string sFileName = oFileName as string;

            if ((sFileName == null ? string.Empty : sFileName).Length == 0) return true;

            try
            {
                if (System.IO.File.Exists(sFileName) == true)
                {
                    // combine the arguments together it doesn't matter if there is a space after ','
                    string argument = @"/select, " + sFileName;

                    Process.Start("explorer.exe", argument);
                    return true;
                }
                else
                {
                    string sParentDir = System.IO.Directory.GetParent(sFileName).FullName;

                    if (System.IO.Directory.Exists(sParentDir) == false)
                        MessageBox.Show(string.Format("The directory '{0}' does not exist or cannot be accessed.", sParentDir),
                                        "Error finding requested resource", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        // combine the arguments together it doesn't matter if there is a space after ','
                        string argument = @"/select, " + sParentDir;

                        Process.Start("explorer.exe", argument);

                        return true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("{0}\n'{1}'.", ex.Message, (sFileName == null ? string.Empty : sFileName)),
                                "Error finding requested resource", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return true;
        }
        #endregion
    }
}
