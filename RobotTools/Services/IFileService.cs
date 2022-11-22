using System.Collections.Generic;
using RobotTools.ViewModels;

namespace RobotTools.Services
{
    internal interface IFileService
    {
        List<string> Files { get; set; }
        FileViewModel Open(string filePath);
    }
}
