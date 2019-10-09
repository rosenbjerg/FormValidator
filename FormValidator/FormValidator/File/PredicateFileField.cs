using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Validation.File
{
    public class PredicateFileField : FormField
    {
        private readonly Func<IFormFile, bool> _predicate;

        public PredicateFileField(string fieldName, int minAmount, int maxAmount, Func<IFormFile, bool> predicate, bool optional = false) 
            : base(fieldName, minAmount, maxAmount, optional)
        {
            _predicate = predicate;
        }

        public sealed override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!TryGetFileField(form, out var files)) return Optional;
            return AmountOk(files) && files.All(file => _predicate(file));
        }
    }
}