using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    public class RationalsField : BasicField
    {
        private static readonly Regex ValidationRegex = new Regex($"-?\\d+(\\{NumberFormatInfo.CurrentInfo.NumberDecimalSeparator}\\d+)?", RegexOptions.Compiled);

        public RationalsField(string fieldName, int minAmount, int maxAmount, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, optional)
        {
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return field.All(ValidationRegex.IsMatch) || Optional;
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return field.All(ValidationRegex.IsMatch) || Optional;
        }
    }
}