using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    public class IntegersField : BasicField
    {
        private static readonly Regex ValidationRegex = new Regex("-?\\d+", RegexOptions.Compiled);

        public IntegersField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName,
            minAmount, maxAmount, optional)
        {
        }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(value => (value == "" && Optional) || ValidationRegex.IsMatch(value));
        }
    }
}