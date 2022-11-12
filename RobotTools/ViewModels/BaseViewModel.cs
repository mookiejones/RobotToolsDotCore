using System;
using System.ComponentModel;
using System.Linq.Expressions;

using CommunityToolkit.Mvvm.ComponentModel;

namespace RobotTools.ViewModels
{
    /// <summary>
    /// Every ViewModel class is required to implement the INotifyPropertyChanged
    /// interface in order to tell WPF when a property changed (for instance, when
    /// a method or setter is executed).
    /// 
    /// Therefore, the PropertyChanged methode has to be called when data changes,
    /// because the relevant properties may or may not be bound to GUI elements,
    /// which in turn have to refresh their display.
    /// 
    /// The PropertyChanged method is to be called by the members and properties of
    /// the class that derives from this class. Each call contains the name of the
    /// property that has to be refreshed.
    /// 
    /// The BaseViewModel is derived from from System.Windows.DependencyObject to allow
    /// resulting ViewModels the implemantion of dependency properties. Dependency properties
    /// in turn are useful when working with IValueConverter and ConverterParameters.
    /// </summary>
    public abstract class BaseViewModel:ObservableRecipient
    {
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
