using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class IntegersField : IntegerField
    {
        protected readonly int MinAmount;
        protected readonly int MaxAmount;
        
        public IntegersField(string fieldName, int minAmount, int maxAmount) : base(fieldName)
        {
            MinAmount = minAmount;
            MaxAmount = maxAmount;
        }
        
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= MinAmount &&
                   field.Count <= MaxAmount &&
                   field.All(ValidationRegex.IsMatch);
        }
    }
}
