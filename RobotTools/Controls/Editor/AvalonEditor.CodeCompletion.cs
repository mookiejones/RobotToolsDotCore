using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace RobotTools.Controls.Editor
{
    public partial class AvalonEditor
    {
        public bool UseCodeCompletion { get; set; } = true;

        private void Complete(char newChar)
        {
        }

        partial void RegisterEvents()
        {

            // Register Changes
            TextArea.TextEntering += HandleTextEntering;
            TextArea.TextEntered += HandleTextEntered;
        }

        private void HandleTextEntered(object sender, TextCompositionEventArgs e)
        {

        }
        private void HandleTextEntering(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
