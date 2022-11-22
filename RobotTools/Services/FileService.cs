using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using MahApps.Metro.Controls;
using RobotTools.ViewModels;

namespace RobotTools.Services
{
    internal class FileService:IFileService
    {

        public FileService(string path) { }

        public List<string> Files { get; set; } = new List<string>();


        public FileService()
        {
#if DEBUG
            var files = System.IO.Directory.EnumerateFiles(@"C:\Robots\starlord","*.src",SearchOption.AllDirectories)
                .Take(3);
           foreach(var file in files)
            {
                Files.Add(file);
            }
#endif
        }

        public FileViewModel Open(string filePath)
        {
            var result = new FileViewModel(filePath);
            return result;
        }
            }
}
