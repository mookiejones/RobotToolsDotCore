using System.Collections.Generic;
using System.IO;

namespace RobotTools.Core.Kop;

public class KopFiles:List<KopFile>
{
    private readonly string _filePath;
    public KopFiles() { }

    public KopFiles(string filePath)
    {
        _filePath = filePath;
            GetFiles();  
    }

    private void GetFiles()
    {
        var files = Directory.EnumerateFiles(_filePath, "*.kop", SearchOption.AllDirectories);



        foreach (var file in files)
        {
            var kopFile = new KopFile(file);
            Add(kopFile);
        }
    }

    public void CopyToDirectory(string directoryToCreate)
    {
        if (Count == 0)
            return;

        Directory.CreateDirectory(directoryToCreate);

        foreach(var kopFile in this)
        {
            var name = kopFile.Name;
            var path = kopFile.Filename;
            var version = $"V_{kopFile.Version.Replace(".", "_")}";

            var parentPath = Path.Combine(directoryToCreate, name,version);

            var filename = Path.GetFileName(path);
            Directory.CreateDirectory(parentPath);

            var filePath = Path.Combine(parentPath, filename);

            File.Copy(kopFile.Filename, filePath,true);


        }

    }


}
