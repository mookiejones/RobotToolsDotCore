using System;
using System.Windows.Markup;
using AvalonDock.Converters;

namespace RobotTools.Converter
{
    [MarkupExtensionReturnType(typeof(BoolToVisibilityConverter))]
    internal class BoolToVisibilityExtension:MarkupExtension
    {
        private BoolToVisibilityConverter _boolToVisibilityConverter;

        public override object ProvideValue(IServiceProvider serviceProvider) => _boolToVisibilityConverter??(_boolToVisibilityConverter= new BoolToVisibilityConverter());
    }
}
