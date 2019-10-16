using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    class PatternStringsField : BasicField
    {
        private readonly Regex _pattern;

        public PatternStringsField(string fieldName, int minAmount, int maxAmount, Regex pattern, bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            _pattern = pattern;
        }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(value => (value == "" && Optional) || _pattern.IsMatch(value));
        }
    }
}