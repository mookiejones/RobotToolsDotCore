using System;
using System.IO;
using System.Threading;

namespace RobotTools.Core.Utilities
{
    public static class FileHelper
    {
        public static bool AreEqual(string path1, string path2)
        {
            var fullName = new System.IO.FileInfo(path1).FullName;
            var fullName2 = new System.IO.FileInfo(path2).FullName;
            return fullName.Equals(fullName2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string CopyIfExisting(string sourcePath, string targetPath)
        {
            if (!File.Exists(sourcePath))
            {
                throw new ArgumentException("File must exist.", "sourcePath");
            }
            string text;
            if (Directory.Exists(targetPath))
            {
                text = targetPath;
                targetPath = Path.Combine(targetPath, GetName(sourcePath));
            }
            else
            {
                text = Path.GetDirectoryName(targetPath);
                if (text == null)
                {
                    throw new InvalidOperationException("Target path should not be null.");
                }
            }
            Directory.CreateDirectory(text);
            File.Copy(sourcePath, targetPath, true);
            return targetPath;
        }

        public static void CopyIfExisting(string sourceDirectory, string pattern, string targetDirectory)
        {
            var files = Directory.GetFiles(sourceDirectory, pattern);
            for (var i = 0; i < files.Length; i++)
            {
                var sourcePath = files[i];
                CopyIfExisting(sourcePath, targetDirectory);
            }
        }

        public static void DeleteIfExisting(string path)
        {
            DeleteIfExisting(path, true);
        }

        public static void DeleteIfExisting(string path, bool force)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (!File.Exists(path))
            {
                return;
            }
            if (force)
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }
            File.Delete(path);
            for (var i = 0; i < 10; i++)
            {
                if (!File.Exists(path))
                {
                    return;
                }
                Thread.Sleep(20);
            }
        }

        public static void DeleteIfExisting(string directory, string pattern)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            var files = Directory.GetFiles(directory, pattern);
            for (var i = 0; i < files.Length; i++)
            {
                var path = files[i];
                DeleteIfExisting(path);
            }
        }

        public static string GetName(string path)
        {
            var fileName = Path.GetFileName(path);
            if (fileName == null)
            {
                throw new InvalidOperationException("Could not acquire filename from " + path);
            }
            return fileName;
        }

        public static string GetNameWithoutExtension(string path)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            if (fileNameWithoutExtension == null)
            {
                throw new InvalidOperationException("Could not acquire filename from " + path);
            }
            return fileNameWithoutExtension;
        }

        public static void MakeWriteable(string path)
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }
        }

        public static void Move(string sourcePath, string targetPath)
        {
            var directoryName = Path.GetDirectoryName(targetPath);
            if (directoryName == null)
            {
                return;
            }
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.Move(sourcePath, targetPath);
        }

        /// <summary>
        /// Used to help prevent from freezing when network directory doesnt exist
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>  
        public static bool DoesDirectoryExist(this FileInfo file)
        {

            if (file.DirectoryName != null)
            {
                var d = new DirectoryInfo(file.DirectoryName);

                try
                {
                    if (Directory.GetDirectories(d.Root.ToString()).Length > 0)
                        return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
    }
}
