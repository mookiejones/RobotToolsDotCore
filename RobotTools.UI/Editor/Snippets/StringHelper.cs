﻿using System.Collections.Generic;
using System.Linq;
namespace RobotTools.UI.Editor.Snippets
{
    internal static class StringHelper
    {
        public static bool ContainsDeclaration(this string code, Dictionary<string, Declaration> declarations)
        {
            return declarations.Values.Select(current => code.Contains(current.Id)).Any(flag => flag);
        }

        public static string GetTheNextId(this string code, Dictionary<string, Declaration> declarations)
        {
            string result = null;
            var num = 2147483647;
            foreach (var current in declarations.Values)
            {
                var num2 = code.IndexOf(current.Id, System.StringComparison.Ordinal);
                if (num2 == -1 || num2 >= num) continue;
                num = num2;
                result = current.Id;
            }
            return result;
        }
    }
}
