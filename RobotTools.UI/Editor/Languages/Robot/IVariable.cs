﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media.Imaging;

namespace RobotTools.UI.Editor.Languages.Robot
{
    public interface IVariable
    {
        bool IsSelected { get; set; }
        BitmapImage Icon { get; set; }
        string Name { get; set; }

        [Localizable(false)]
        string Type { get; set; }

        string Path { get; set; }
        string Value { get; set; }
        string Comment { get; set; }
        string Declaration { get; set; }
        string Description { get; set; }
        int Offset { get; set; }
    }
}
