using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateRationalField : ParsedIntegerField
    {
        private readonly Func<double, bool> _predicate;

        public PredicateRationalField(string fieldName, Func<double, bool> predicate) : base(fieldName)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   int.TryParse(field[0], numberStyles, cultureInfo, out var val) &&
                   _predicate(val);
        }
    }
}