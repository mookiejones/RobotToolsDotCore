namespace RobotTools.Core.Kop;


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "KUKARoboter.Contracts")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "KUKARoboter.Contracts", IsNullable = false)]
public partial class OptionData
{


    /// <remarks/>
    public string Customer { get; set; }

    /// <remarks/>
    public DependentOptions DependentOptions { get; set; }

    /// <remarks/>
    public string DisplayName { get; set; }

    /// <remarks/>
    public string Name { get; set; }

    /// <remarks/>
    public string SupportedKRCVersion { get; set; }

    /// <remarks/>
    public string UserAccessibleDirs { get; set; }

    /// <remarks/>
    public Version Version { get; set; }

    /// <remarks/>
    public string VersionAppendix { get; set; }

    /// <remarks/>
    public string SchemaVersion { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Dependency", IsNullable = false)]
    public Dependency[] Dependencies { get; set; }
}
