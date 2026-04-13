using System.Text.RegularExpressions;

namespace Portal.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string SomenteNumeros(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return Regex.Replace(input, @"\D", "");
        }

        public static string FormatarComoCpfCnpj(this string doc)
        {
            if (string.IsNullOrWhiteSpace(doc)) return doc;
            doc = doc.SomenteNumeros();

            if (doc.Length == 11)
                return Convert.ToUInt64(doc).ToString(@"000\.000\.000\-00");

            if (doc.Length == 14)
                return Convert.ToUInt64(doc).ToString(@"00\.000\.000\/0000\-00");

            return doc;
        }
    }
}
