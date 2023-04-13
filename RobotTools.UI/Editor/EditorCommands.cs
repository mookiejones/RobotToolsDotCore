using System;
using System.Collections.Generic;
using System.Text;

namespace RobotTools.UI.Editor
{
    public  class EditorCommands
    {
        private static RoutedUICommand saveCommand;
        static EditorCommands()
        {
            saveCommand = new RoutedUICommand("Save File", "SaveCommand", typeof(EditorCommands));
        }

        public static RoutedUICommand SaveCommand => saveCommand;

        public static void SaveCommand_Executed(object sender,
                   ExecutedRoutedEventArgs e)
        {
            var viewModel = e.Parameter as IEditor;
            MessageBox.Show("Add contact command executed");
        }

        public static void SaveCommand_CanExecute(object sender,
                   CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static void Register(Window window)
        {
            window.CommandBindings.Add(new CommandBinding(SaveCommand, SaveCommand_Executed, SaveCommand_CanExecute));
        }

    }
}
