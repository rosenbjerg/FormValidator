using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class RationalField : RequiredField
    {
        protected static Regex ValidationRegex = new Regex($"-?\\d+(\\{NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}\\d+)?", RegexOptions.Compiled);
        
        public RationalField(string fieldName) : base(fieldName) { }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   ValidationRegex.IsMatch(field[0]);
        }

        /// <summary>
        /// Change decimal separator.
        /// Default is NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
        /// </summary>
        /// <param name="separator">New decimal separator</param>
        public static void SetDecimalSeparator(string separator)
        {
            var regex = $"-?\\d+({Regex.Escape(separator)}\\d+)?";
            ValidationRegex = new Regex(regex, RegexOptions.Compiled);
        }
    }
}