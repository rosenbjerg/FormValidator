using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class ParsedIntegersField : RequiredField
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public ParsedIntegersField(string fieldName, int minAmount, int maxAmount, int minValue, int maxValue) : base(fieldName, minAmount, maxAmount)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(val => int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    parsed >= _minValue && 
                                    parsed <= _maxValue);
        }
    }
}