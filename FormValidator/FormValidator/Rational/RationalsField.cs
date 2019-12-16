using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    public class RationalsField : BasicField
    {
        public RationalsField(string fieldName, int minAmount, int maxAmount, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, optional)
        {
        }

        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return values.All(value => (value == "" && Optional) || double.TryParse(value, numberStyles, cultureInfo, out _));
        }
    }
}