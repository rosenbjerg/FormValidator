using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class StringsField : StringField
    {
        private readonly int _minAmount;
        private readonly int _maxAmount;

        public StringsField(string fieldName, int minAmount, int maxAmount, int minLength = 0, int maxLength = -1) : base(fieldName, minLength, maxLength)
        {
            _minAmount = minAmount;
            _maxAmount = maxAmount;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count >= _minAmount && 
                   field.Count <= _maxAmount &&
                   field.All(s => s.Length >= MinLength && 
                                  (MaxLength == -1 || s.Length <= MaxLength));
        }
    }
}