using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using RobotTools.Core;

namespace RobotTools.UI.AngleConverter;

[MarkupExtensionReturnType(typeof(AngleToolTipConverter))]
public class AngleToolTipConverterExtension : MarkupExtension
{
    private AngleToolTipConverter? _converter;
    public override object ProvideValue(IServiceProvider serviceProvider) => _converter??(_converter=new AngleToolTipConverter());
}
[Localizable(false)]
public class AngleToolTipConverter : IValueConverter
{
    private string _title = string.Empty;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        object result;
        switch ((CartesianEnum)value)
        {
            case CartesianEnum.ABB_Quaternion:
                {
                    _title = "ABB Quaternion";
                    string? text = parameter?.ToString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        if (text == "V1")
                        {
                            result = _title + " Q1";
                            return result;
                        }
                        if (text == "V2")
                        {
                            result = _title + " Q2";
                            return result;
                        }
                        if (text == "V3")
                        {
                            result = _title + " Q3";
                            return result;
                        }
                        if (text == "V4")
                        {
                            result = _title + " Q4";
                            return result;
                        }
                    }
                    break;
                }
            case CartesianEnum.Roll_Pitch_Yaw:
                {
                    _title = "Roll Pitch Yaw";
                    string text = parameter.ToString();
                    if (text != null)
                    {
                        if (text == "V1")
                        {
                            result = _title + " R. Rotation around X.";
                            return result;
                        }
                        if (text == "V2")
                        {
                            result = _title + " P. Rotation around Y.";
                            return result;
                        }
                        if (text == "V3")
                        {
                            result = _title + " Y. Rotation around Z.";
                            return result;
                        }
                    }
                    break;
                }
            case CartesianEnum.Axis_Angle:
                result = "Axis Angle";
                return result;
            case CartesianEnum.Kuka_ABC:
                {
                    _title = "Kuka ABC";
                    string text = parameter.ToString();
                    if (!string.IsNullOrEmpty(text))
                    {
                        if (text == "V1")
                        {
                            result = _title + " A. Rotation around Z.";
                            return result;
                        }
                        if (text == "V2")
                        {
                            result = _title + " B. Rotation around Y.";
                            return result;
                        }
                        if (text == "V3")
                        {
                            result = _title + " C. Rotation around X.";
                            return result;
                        }
                    }
                    break;
                }
            case CartesianEnum.Euler_ZYZ:
                result = "Euler ZYZ";
                return result;
            case CartesianEnum.Alpha_Beta_Gamma:
                result = "Alpha Beta Gamma";
                return result;
        }
        result = Binding.DoNothing;
        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
