using System.Diagnostics;
using System.IO;
namespace RobotTools.Controls.Editor
{
    public static class Global
    {
        public static string StartupPath
        {
            get { return
            
             Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName); }
        }
    }
}