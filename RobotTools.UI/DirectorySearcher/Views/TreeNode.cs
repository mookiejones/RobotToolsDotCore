using System.Collections.Generic;

using RobotTools.Core.Kop;

namespace RobotTools.UI.DirectorySearcher.Views;

public class TreeNode
{
    public string Name { get; set; }
    public List<KopFile> KopFiles { get; set; } 
}
