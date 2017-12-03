using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class PatternStringField : RequiredField
    {
        protected readonly Regex ValidationRegex;

        public PatternStringField(string fieldName, Regex regex) : base(fieldName)
        {
            ValidationRegex = regex;
        }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   ValidationRegex.IsMatch(field[0]);
        }
    }
}