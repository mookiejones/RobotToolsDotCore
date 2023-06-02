
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Ionic.Zip;

using RobotTools.Core.Utilities;

namespace RobotTools.Core.Kop;


/*
 * Name: KUKA.PROFINET MS.kop
Location: C:\kuka\AceShopRobot\KUKA.PROFINET MS\V_4_1_4_18\KUKA.PROFINET MS.kop
Size: 12.48 MB
Name: KUKA.PROFINET MS
Version: 4.1.4.18 
Type: Integrated
Supported KRC version: 8.5
Creator: KUKA Roboter GmbH
Contact: 
Date: 8/4/2020
Created with: KopFileBuilder 1.1.28
Last changed on: 8/4/2020
Last changed with: KopFileBuilder 1.1.28

Dependencies:
KRC
Supported versions:
8.5 ≤ KRC < 8.6

Platform dependencies:
No dependencies
*/

public class KopFile
{

    public override string ToString()
    {
        var sb = new StringBuilder();

        var props = GetType().GetProperties()
            .Select(o => $"{o.Name}: {o.GetValue(this)}");

        foreach (var prop in props)
            sb.AppendLine(prop);

        sb.AppendLine("Dependencies");
        foreach(var dependency in _optionData.Dependencies)
        {
            sb.AppendLine($"Name: {dependency.Identifier}, Type: {dependency.DependencyType}, Ignore In Option Management :{dependency.IgnoreDependencyInWoVOptionManagement}");
            foreach (var version in dependency.Versions)
                sb.AppendLine($"Version:{version.Version}, IsSupported:{version.IsSupported}");
        }

        var result = sb.ToString();
        return result;

    }
    #region Properties
    public string Filename { get; private set; }

    public string Name => _optionData.DisplayName;
    public string Version { get; private set; }

    public string Type { get; private set; }

    public string SupportedVersion { get; private set; }

    public string Customer => _optionData.Customer;
    public string Creator { get; private set; }

    public string Contact { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime ModifiedDate { get; private set; }
    public DateTime Date { get; private set; }

    public long Size { get; private set; }

    private OptionData _optionData;

    public string SupportedKRCVersion => _optionData.SupportedKRCVersion;

    #endregion
    /// <summary>
    /// Constructor
    /// </summary>
    public KopFile()
    {

    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fileName">Filename of kop</param>
    public KopFile(string fileName)
    {
        Filename = fileName;
        ReadFile();
    }

    private void ReadFile()
    {

        var stop = Filename.Contains("ready", StringComparison.InvariantCultureIgnoreCase);
        var fileInfo = new FileInfo(Filename);
        CreateDate = fileInfo.CreationTime.Date;
        ModifiedDate = fileInfo.LastWriteTime;
        Size = fileInfo.Length;
        using (var zipFile = new ZipFile(Filename))
        {

            // Find Version.ini

            var version = zipFile.Entries.FirstOrDefault(o => o.FileName.Contains("Version.ini", StringComparison.InvariantCultureIgnoreCase));

            var text = version.GetText();

            // Get Date                
            Date = GetDate(text);

            var build= text.Match("Build=([.\\d]+)");
            
            // Get Version
            var value = text.Match("Version=V([.\\d]+)");
            if (version != null) {
                Version = value;

                if (build != null)
                    Version = $"{Version}.{build}";
                    }


            var optionDataEntry = zipFile.Entries.FirstOrDefault(o => o.FileName.Contains("MetaData.xml", StringComparison.InvariantCultureIgnoreCase));

            var xml = optionDataEntry.GetText();

            _optionData = xml.FromXml<OptionData>();
             
        }

         DateTime GetDate(string value)
        {
            // Get Date
            var date = Regex.Match(value, @"Date=([\d]{4})([\d]{2})([\d]{2})", RegexOptions.IgnoreCase);


            var year = Convert.ToInt32(date.Groups[1].Value);
            var month = Convert.ToInt32(date.Groups[2].Value);
            var day = Convert.ToInt32(date.Groups[3].Value);

            return new DateTime(year, month, day);
        }
    }
}
 
