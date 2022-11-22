﻿using System;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.UI.Editor.Languages.Robot
{
    public sealed class Variable : ObservableObject,IVariable
    {
        private string _comment = string.Empty;
        private string _declaration = string.Empty;
        private string _description = string.Empty;
        private BitmapImage _icon;
        private bool _isSelected=false;
        private string _name = string.Empty;
        private int _offset;
        private string _path = string.Empty;
        private string _type = string.Empty;
        private string _value = string.Empty;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public BitmapImage Icon { get =>_icon; set=>SetProperty(ref _icon,value);}

        public string Description { get =>_description; set=>SetProperty(ref _description,value);}

        public string Name { get =>_name; set=>SetProperty(ref _name,value);}

        public string Type { get =>_type; set=>SetProperty(ref _type,value);}

        public string Path { get =>_path; set=>SetProperty(ref _path,value);}

        public string Value { get =>_value; set=>SetProperty(ref _value,value);}

        public string Comment { get =>_comment; set=>SetProperty(ref _comment,value);}

        public string Declaration { get =>_declaration; set=>SetProperty(ref _declaration,value);}

        public int Offset { get =>_offset; set=>SetProperty(ref _offset,value);}

        #region Returns

        /// <summary>
        ///     The <see cref="Returns" /> property's name.
        /// </summary>
        private const string ReturnsPropertyName = "Returns";

        private string _returns = String.Empty;

        /// <summary>
        ///     Sets and gets the Returns property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Returns { get =>_returns; set=>SetProperty(ref _returns,value);}


        #endregion

        #region ShowDeclaration


        private bool _showDeclaration;
        /// <summary>
        ///     Sets and gets the ShowDeclaration property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowDeclaration { get =>_showDeclaration; set=>SetProperty(ref _showDeclaration,value);}
        

        #endregion

        #region ShowReturns

       
        private bool _showReturns;

        /// <summary>
        ///     Sets and gets the ShowReturns property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool ShowReturns { get =>_showReturns; set=>SetProperty(ref _showReturns,value);}
        

        #endregion

        #region ShowOffset

         

        private string _showOffset = String.Empty;

        /// <summary>
        ///     Sets and gets the ShowOffset property.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string ShowOffset { get =>_showOffset; set=>SetProperty(ref _showOffset,value);}
        

        #endregion
    }
}
