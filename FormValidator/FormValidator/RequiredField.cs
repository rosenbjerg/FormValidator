using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace FormValidator
{
    public abstract class RequiredField
    {
        private readonly int _minAmount;
        private readonly int _maxAmount;
        
        protected RequiredField(string fieldName, int minAmount, int maxAmount)
        {
            FieldName = fieldName;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
        }
        
        public string FieldName { get; }
        
        public abstract bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo);

        protected bool TryGetField(IFormCollection form, out StringValues field)
        {
            return form.TryGetValue(FieldName, out field) &&
                   field.Count >= _minAmount &&
                   field.Count <= _maxAmount;
        }
    }
}