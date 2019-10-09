using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    public class PredicateRationalsField : PredicateField<double>
    {
        public PredicateRationalsField(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, predicate, optional)
        { }
        
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!form.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                    SatisfiesPredicate(parsed));
        }
        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!query.TryGetValue(Fieldname, out var field)) return Optional;
            return AmountOk(field) && field.All(val => double.TryParse(val, numberStyles, cultureInfo, out var parsed) &&
                                                       SatisfiesPredicate(parsed));
        }
    }
}