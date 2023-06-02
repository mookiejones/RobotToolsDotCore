namespace RobotTools.Core.Kop;

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "KUKARoboter.Contracts")]
public partial class Version
{

    /// <remarks/>
    public byte Build { get; set; }

    /// <remarks/>
    public byte Major { get; set; }

    /// <remarks/>
    public byte Minor { get; set; }

    /// <remarks/>
    public ushort Revision { get; set; }
}
