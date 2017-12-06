using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class IntegersField : RequiredField
    {
        private static readonly Regex ValidationRegex = new Regex("-?\\d+", RegexOptions.Compiled);
        
        public IntegersField(string fieldName, int minAmount, int maxAmount) : base(fieldName, minAmount, maxAmount)
        {
        }
        
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(ValidationRegex.IsMatch);
        }
    }
}
