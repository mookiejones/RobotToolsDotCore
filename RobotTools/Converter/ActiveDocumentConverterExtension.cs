using System;
using System.Windows.Markup;

namespace RobotTools.Converter;

/// <summary>
/// Provides shorthand to initialise a new <see cref="ActiveDocumentConverter"/> for a <see cref="Snackbar"/>.
/// </summary>
[MarkupExtensionReturnType(typeof(ActiveDocumentConverter))]
public class ActiveDocumentConverterExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        new ActiveDocumentConverter();
     }
