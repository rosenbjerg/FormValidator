using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class ParsedRationalsField : RationalsField
    {
        private readonly double _minValue;
        private readonly double _maxValue;

        public ParsedRationalsField(string fieldName, int minAmount, int maxAmount, double minValue, double maxValue) 
            : base(fieldName, minAmount, maxAmount)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    parsed >= _minValue && 
                                    parsed <= _maxValue);
        }
    }
}