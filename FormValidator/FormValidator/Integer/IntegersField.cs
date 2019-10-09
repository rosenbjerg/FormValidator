using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    public class IntegersField : BasicField
    {
        private static readonly Regex ValidationRegex = new Regex("-?\\d+", RegexOptions.Compiled);

        public IntegersField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName,
            minAmount, maxAmount, optional)
        {
        }

        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(value => ValidationRegex.IsMatch(value)) || Optional;
        }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(value => ValidationRegex.IsMatch(value));
        }
    }
}