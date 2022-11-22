using System.Windows.Input;

namespace RobotTools
{
    public class AppCommand
    {
        #region CommandFramework Fields
        private static RoutedUICommand newFile;
        private static RoutedUICommand loadFile;

        private static RoutedUICommand pinUnpin;
        private static RoutedUICommand addMruEntry;
        private static RoutedUICommand removeMruEntry;

        private static RoutedUICommand browseURL;
        private static RoutedUICommand showStartPage;
        #endregion CommandFramework Fields

        #region Static Constructor (Constructs static application commands)
        /// <summary>
        /// Define custom commands and their key gestures
        /// </summary>
        static AppCommand()
        {
            InputGestureCollection inputs = null;

            // Initialize pin command (to set or unset a pin in MRU and re-sort list accordingly)
            inputs = new InputGestureCollection();
            pinUnpin = new RoutedUICommand("Pin or Unpin", "Pin", typeof(AppCommand), inputs);

            // Execute add recent files list etnry pin command (to add another MRU entry into the list)
            inputs = new InputGestureCollection();
            addMruEntry = new RoutedUICommand("Add Entry", "AddEntry", typeof(AppCommand), inputs);

            // Execute remove pin command (remove a pin from a recent files list entry)
            inputs = new InputGestureCollection();
            removeMruEntry = new RoutedUICommand("Remove Entry", "RemoveEntry", typeof(AppCommand), inputs);

            // Execute file open command (without user interaction)
            inputs = new InputGestureCollection();
            loadFile = new RoutedUICommand("Open ...", "LoadFile", typeof(AppCommand), inputs);

            // Execute file open command (without user interaction)
            inputs = new InputGestureCollection();
            browseURL = new RoutedUICommand("Open URL ...", "OpenURL", typeof(AppCommand), inputs);

            // Execute file open command (without user interaction)
            inputs = new InputGestureCollection();
            newFile = new RoutedUICommand("New File", "NewFile", typeof(AppCommand), inputs);

            inputs = new InputGestureCollection();
            showStartPage = new RoutedUICommand("Show Start Page", "ShowStartPage", typeof(AppCommand), inputs);
        }
        #endregion Static Constructor

        #region CommandFramwork Properties (Exposes Commands to which the UI can bind to)
        /// <summary>
        /// Execute pin/unpin command (to set or unset a pin in MRU and re-sort list accordingly)
        /// </summary>
        public static RoutedUICommand PinUnpin=>pinUnpin;

        /// <summary>
        /// Execute add recent files list etnry pin command (to add another MRU entry into the list)
        /// </summary>
        public static RoutedUICommand AddMruEntry => addMruEntry;

        /// <summary>
        /// Execute remove pin command (remove a pin from a recent files list entry)
        /// </summary>
        public static RoutedUICommand RemoveMruEntry => removeMruEntry;

        /// <summary>
        /// Execute file open command (without user interaction)
        /// </summary>
        public static RoutedUICommand LoadFile => loadFile;

        public static RoutedUICommand BrowseURL => browseURL;

        public static RoutedUICommand ShowStartPage => showStartPage;
        #endregion CommandFramwork_Properties
    }
}
