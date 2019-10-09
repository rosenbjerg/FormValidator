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
        public bool Optional { get; }
        
        protected bool AmountOk(StringValues field)
        {
            return field.Count >= MinAmount && field.Count <= MaxAmount;
        }

        public abstract bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo);
        
        public abstract bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo);

        protected BasicField(string fieldName, int minAmount, int maxAmount, bool optional) 
        {
            Fieldname = fieldName;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            Optional = optional;
        }
    }
}