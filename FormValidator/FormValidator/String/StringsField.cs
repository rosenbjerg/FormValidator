using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    class StringsField : BasicField
    {
        public StringsField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName,
            minAmount, maxAmount, optional)
        {
        }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field);
        }

        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field);
        }
    }
}