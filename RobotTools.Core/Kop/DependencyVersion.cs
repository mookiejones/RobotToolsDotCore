using System.Text.RegularExpressions;

namespace RobotTools.Core.Kop
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "KUKARoboter.Contracts")]
    public partial class DependencyVersion
    {

        /// <remarks/>
        public bool IsSupported { get; set; }


        private string _version;
        /// <remarks/>        
        public string Version {
            
            get =>Regex.Replace(_version, @"\[([\d.;]+)[\]\)]", "$1");
            set =>_version=value; }
    }


}
