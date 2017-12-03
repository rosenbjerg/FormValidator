using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class ParsedIntegersField : IntegersField
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
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= MinAmount &&
                   field.Count <= MaxAmount &&
                   field.All(val => int.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    parsed >= _minValue && 
                                    parsed <= _maxValue);
        }
    }
}