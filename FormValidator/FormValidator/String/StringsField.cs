using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class StringsField : RequiredField
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public StringsField(string fieldName, int minAmount, int maxAmount, int minLength = 0, int maxLength = -1) : base(fieldName, minAmount, maxAmount)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(s => s.Length >= _minLength && 
                                  (_maxLength == -1 || s.Length <= _maxLength));
        }

        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(query, out var field) &&
                   field.All(s => s.Length >= _minLength && 
                                  (_maxLength == -1 || s.Length <= _maxLength));
        }
    }
}