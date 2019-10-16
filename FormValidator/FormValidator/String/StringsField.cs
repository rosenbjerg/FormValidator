using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    class StringsField : BasicField
    {
        public StringsField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName,
            minAmount, maxAmount, optional)
        {
        }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return true;
        }
    }
}