using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    class PatternStringsField : BasicField
    {
        private readonly Regex _pattern;

        public PatternStringsField(string fieldName, int minAmount, int maxAmount, Regex pattern, bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            _pattern = pattern;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(_pattern.IsMatch);
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(_pattern.IsMatch);
        }
    }
}