using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateStringField : ParsedIntegerField
    {
        private readonly Func<string, bool> _predicate;

        public PredicateStringField(string fieldName, Func<string, bool> predicate) : base(fieldName)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   _predicate(field[0]);
        }
    }
}