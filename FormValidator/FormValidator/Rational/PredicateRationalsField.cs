using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class PredicateRationalsField : RequiredField
    {
        private readonly Func<double, bool> _predicate;

        public PredicateRationalsField(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate) 
            : base(fieldName, minAmount, maxAmount)
        {
            _predicate = predicate;
        }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(form, out var field) &&
                   field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return TryGetField(query, out var field) &&
                   field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    _predicate(parsed));
        }
    }
}