namespace RobotTools.Core.Kop;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "KUKARoboter.Contracts")]
public partial class Dependency
{

    /// <remarks/>
    public string DependencyType { get; set; }

    /// <remarks/>
    public string Identifier { get; set; }

    /// <remarks/>
    public bool IgnoreDependencyInWoVOptionManagement { get; set; }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("DependencyVersion", IsNullable = false)]
    public DependencyVersion[] Versions { get; set; }
}
