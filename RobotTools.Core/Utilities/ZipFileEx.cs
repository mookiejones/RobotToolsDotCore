using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Ionic.Zip;

namespace RobotTools.Core.Utilities
{
    internal static class ZipFileEx
    {
        public static string GetText(this ZipEntry entry)
        {
            using (var ms = new MemoryStream())
            {

                // Extract the file
                entry.Extract(ms);

                ms.Position = 0;

                using (var streamReader = new StreamReader(ms))
                {
                    var result = streamReader.ReadToEnd();
                    return result;
                }
            }
        }
    }
}
