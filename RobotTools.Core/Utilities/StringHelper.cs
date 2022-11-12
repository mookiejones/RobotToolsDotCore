namespace RobotTools.Core.Utilities
{
    public static class StringHelper
    {
        /// <summary>
        /// Extend the string constructor with a string.Format like syntax.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
    }
}
