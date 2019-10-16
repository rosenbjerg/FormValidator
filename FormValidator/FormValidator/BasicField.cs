using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    /// <summary>
    /// Class representing files found on both IQueryCollection and IFormCollection
    /// </summary>
    public abstract class BasicField
    {
        protected int MinAmount { get; }
        protected int MaxAmount { get; }
        public string Fieldname { get; }
        protected bool Optional { get; }

        protected abstract bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo);

        internal virtual bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return query.TryGetValue(Fieldname, out var values)
                ? values.Count >= MinAmount && values.Count <= MaxAmount && IsSatisfied(values, numberStyles, cultureInfo)
                : Optional;
        }

        internal virtual bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(Fieldname, out var values)
                ? values.Count >= MinAmount && values.Count <= MaxAmount && IsSatisfied(values, numberStyles, cultureInfo)
                : Optional;
        }

        protected BasicField(string fieldName, int minAmount, int maxAmount, bool optional) 
        {
            Fieldname = fieldName;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Optional = optional;
        }
    }
}