using System;
using System.Collections.Generic;
using System.Text;

namespace RobotTools.UI.Editor;

public  class EditorCommands
{
    private static RoutedUICommand saveCommand;
    private static RoutedUICommand reloadCommand;
    static EditorCommands()
    {
        saveCommand = new RoutedUICommand("Save File", "SaveCommand", typeof(EditorCommands));
        reloadCommand = new RoutedUICommand("Reload","ReloadCommand",typeof(EditorCommands));
    }

    public static RoutedUICommand SaveCommand => saveCommand;

    public static RoutedUICommand ReloadCommand => reloadCommand;



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
        window.CommandBindings.Add(new CommandBinding(ReloadCommand, ReloadCommand_Executed, ReloadCommand_CanExecute));
    }

    private static void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private static void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        throw new NotImplementedException();
    }
}
