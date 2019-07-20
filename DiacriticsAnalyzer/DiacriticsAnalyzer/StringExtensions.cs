using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiacriticsAnalyzer
{
    static class StringExtensions
    {

        private static Dictionary<char, string> map = new Dictionary<char, string>() {
          { 'ä', "ae" },
          { 'ö', "oe" },
          { 'ü', "ue" },
          { 'Ä', "Ae" },
          { 'Ö', "Oe" },
          { 'Ü', "Ue" },
          { 'ß', "ss" }
        };

        internal static string RemoveGermanDiacritics(this string literal)
        {
            return literal.Aggregate(
              new StringBuilder(),
              (sb, c) => map.TryGetValue(c, out var r) ? sb.Append(r) : sb.Append(c)
              ).ToString();
        }

        internal static bool ContainsGermanDiacritics(this string literal) {
            return literal.ToCharArray().Any(c => map.ContainsKey(c));
        }


    }
}
