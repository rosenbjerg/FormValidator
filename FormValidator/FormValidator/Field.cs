using System.Collections.Generic;
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

    /// <summary>
    /// Class representing fields only found on IFormCollection, fx. IFormField, thus cannot be satisfied by a IQueryCollection
    /// </summary>
    public abstract class FormField : BasicField
    {
        protected bool TryGetFileField(IFormCollection form, out IReadOnlyList<IFormFile> files)
        {
            files = form.Files.GetFiles(Fieldname);
            return files != null;
        }

        protected bool AmountOk(IReadOnlyList<IFormFile> field)
        {
            return field.Count >= MinAmount && field.Count <= MaxAmount;
        }

        public override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return false;
        }

        protected FormField(string fieldName, int minAmount, int maxAmount, bool optional) : base(fieldName,
            minAmount, maxAmount, optional) { }
    }
}