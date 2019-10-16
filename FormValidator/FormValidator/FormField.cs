using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation
{
    /// <summary>
    /// Class representing fields only found on IFormCollection, fx. IFormField, thus cannot be satisfied by a IQueryCollection
    /// </summary>
    public abstract class FormField : BasicField
    {
        protected override bool IsSatisfied(StringValues values, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return false;
        }
        internal override bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return false;
        }

        protected abstract bool IsSatisfied(IReadOnlyList<IFormFile> files);
        
        internal override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            var files = form.Files.GetFiles(Fieldname);
            if (files == null || !files.Any()) return Optional;
            return files.Count >= MinAmount && files.Count <= MaxAmount && IsSatisfied(files);
        }
        protected FormField(string fieldName, int minAmount, int maxAmount, bool optional) : base(fieldName,
            minAmount, maxAmount, optional) { }
    }
}