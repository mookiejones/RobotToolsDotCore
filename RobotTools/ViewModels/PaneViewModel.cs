using System;
using System.Linq.Expressions;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.ViewModels
{
    class PaneViewModel : ObservableRecipient
  {
    public PaneViewModel()
    { }


    #region Title

    private string _title = null;
    public string Title
    {
      get { return _title; }
      set
      {
                SetProperty(ref _title, value);
      }
    }

    #endregion

    public ImageSource IconSource
    {
      get;
      protected set;
    }

    #region ContentId

    private string _contentId = null;
    public string ContentId
    {
      get { return _contentId; }
      set
      {
                SetProperty(ref _contentId, value);
      }
    }

    #endregion

    #region IsSelected

    private bool _isSelected = false;
    public bool IsSelected
    {
      get { return _isSelected; }
      set
      {
                SetProperty(ref _isSelected, value);
            }
    }

    #endregion

    #region IsActive

    private bool _isActive = false;
    public bool IsActive1
    {
      get {  return _isActive; }
      set
      {
        if (_isActive != value)
        {
                    SetProperty(ref _isActive, value);
                }
      }
    }

        #endregion
        /// <summary>
        /// Tell bound controls (via WPF binding) to refresh their display.
        ///
        /// Sample call: this.NotifyPropertyChanged(() => this.IsSelected);
        /// where 'this' is derived from <seealso cref="BaseViewModel"/>
        /// and IsSelected is a property.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="property"></param>
        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            OnPropertyChanged(memberExpression.Member.Name);
        }

    }
}
