using System.Diagnostics;
using System.IO;
namespace RobotTools.UI.Editor;

public static class Global
{
    public static string StartupPath=> Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
    public const string LogFile = "logFile.txt";
    public const string ImgError = "..\\Images\\resources-error.png";
    public const string ImgInfo = "..\\Images\\resources-info.png";
    public const string IconObjectBrowser = "pack://application:,,/Resources/resources-objectbrowser.png";
    public const string IconProperty = "pack://application:,,/Resources/property-blue.png";
    public const string ImgConst = "..\\Images\\resources-vxconstant_icon.png";
    public const string ImgStruct = "..\\Images\\resources-vxstruct_icon.png";
    public const string ImgMethod = "..\\Images\\resources-vxmethod_icon.png";
    public const string ImgEnum = "..\\Images\\resources-vxenum_icon.png";
    public const string ImgField = "..\\Images\\resources-vxfield_icon.png";
    public const string ImgValue = "..\\Images\\resources-vxvaluetype_icon.png";
    public const string ImgSignal = "..\\Images\\resources-vxevent_icon.png";
    public const string ImgXyz = "..\\Images\\resources-vxXYZ_icon.png";
    public const string ImgSrc = "..\\Images\\resources-srcfile.png";
    public const string ImgDat = "..\\Images\\resources-datfile.png";
    public const string ImgSps = "..\\Images\\resources-spsfile.png";
}
