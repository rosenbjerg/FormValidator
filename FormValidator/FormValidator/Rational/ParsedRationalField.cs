using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class ParsedRationalField : RationalField
    {
        private readonly double _minValue;
        private readonly double _maxValue;

        public ParsedRationalField(string fieldName, double minValue, double maxValue) 
            : base(fieldName)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   double.TryParse(field[0], numberStyles, cultureInfo, out var val) &&
                   val >= _minValue && 
                   val <= _maxValue;
        }
    }
}