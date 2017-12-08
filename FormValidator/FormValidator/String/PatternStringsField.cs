using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class PatternStringsField : RequiredField
    {
        private readonly Regex _pattern;

        public PatternStringsField(string fieldName, int minAmount, int maxAmount, Regex pattern) : base(fieldName, minAmount, maxAmount)
        {
            _pattern = pattern;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(_pattern.IsMatch);
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(query, out var field) &&
                   field.All(_pattern.IsMatch);
        }
    }
}