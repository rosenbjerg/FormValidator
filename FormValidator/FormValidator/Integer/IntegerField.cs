using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class IntegerField : RequiredField
    {
        protected static readonly Regex ValidationRegex = new Regex("-?\\d+", RegexOptions.Compiled);
        
        public IntegerField(string fieldName) : base(fieldName) { }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   ValidationRegex.IsMatch(field[0]);
        }
    }
}