using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Validation
{
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