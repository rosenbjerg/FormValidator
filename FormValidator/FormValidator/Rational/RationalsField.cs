using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class RationalsField : RationalField
    {
        private readonly int _minAmount;
        private readonly int _maxAmount;

        public RationalsField(string fieldName, int minAmount, int maxAmount) 
            : base(fieldName)
        {
            _minAmount = minAmount;
            _maxAmount = maxAmount;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= _minAmount &&
                   field.Count <= _maxAmount &&
                   field.All(ValidationRegex.IsMatch);
            
        }
    }
}