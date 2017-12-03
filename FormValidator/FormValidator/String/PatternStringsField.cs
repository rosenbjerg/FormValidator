using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class PatternStringsField : PatternStringField
    {
        private readonly int _minAmount;
        private readonly int _maxAmount;

        public PatternStringsField(string fieldName, int minAmount, int maxAmount, Regex pattern) : base(fieldName, pattern)
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