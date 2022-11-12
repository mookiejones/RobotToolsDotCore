using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace RobotTools.Core.Utilities
{
    public static class ApplicationHelper
    {
        private static Assembly _executingAssembly;
        private static Version _executingAssemblyVersion;
        private static DateTime? _compileDate;



        private static string RootDirectory
        {
            get { return Path.GetDirectoryName(ExecutingAssembly.Location); }
        }

        /// <summary>
        ///     Gets the executing assembly.
        /// </summary>
        /// <value>The executing assembly.</value>
        private static Assembly ExecutingAssembly
        {
            get { return _executingAssembly ?? (_executingAssembly = Assembly.GetExecutingAssembly()); }
        }

        /// <summary>
        ///     Gets the executing assembly version.
        /// </summary>
        /// <value>The executing assembly version.</value>
        private static Version ExecutingAssemblyVersion
        {
            get
            {
                return _executingAssemblyVersion ?? (_executingAssemblyVersion = ExecutingAssembly.GetName().Version);
            }
        }

        public static string Major
        {
            get { return ExecutingAssemblyVersion.Major.ToString(CultureInfo.InvariantCulture); }
        }

        public static string Minor
        {
            get { return ExecutingAssemblyVersion.Minor.ToString(CultureInfo.InvariantCulture); }
        }

        public static string Build
        {
            get { return ExecutingAssemblyVersion.Build.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        ///     Gets the compile date of the currently executing assembly.
        /// </summary>
        /// <value>The compile date.</value>
        public static DateTime CompileDate
        {
            get
            {
                if (!_compileDate.HasValue)
                    _compileDate = RetrieveLinkerTimestamp(ExecutingAssembly.Location);
                return (DateTime)_compileDate;
            }
        }


        public static string Company
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                return attributes.Length == 0 ? "" : ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }


        public static string Copyright
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }


        public static string Product
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public static string Title
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length <= 0)
                    return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
                var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                return titleAttribute.Title != ""
                    ? titleAttribute.Title
                    : Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public static string Version
        {
            get { return ExecutingAssembly.GetName().Version.ToString(); }
        }


        public static string Description
        {
            get
            {
                var attributes = ExecutingAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        /// <summary>
        ///     Retrieves the linker timestamp.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <remarks>http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html</remarks>
        private static DateTime RetrieveLinkerTimestamp(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var b = new byte[2048];
            FileStream s = null;
            try
            {
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                    s.Close();
            }
            var dt =
                new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(BitConverter.ToInt32(b,
                    BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset));
            return dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
        }
    }
}
