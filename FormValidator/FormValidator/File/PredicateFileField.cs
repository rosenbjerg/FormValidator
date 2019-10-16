using System;
using System.Collections.Generic;
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

        protected override bool IsSatisfied(IReadOnlyList<IFormFile> files)
        {
            return files.All(file => _predicate(file));
        }
    }
}