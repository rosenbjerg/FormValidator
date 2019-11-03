using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Validation.File
{
    public class FileField : FormField
    {
        public FileField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            
        }

        protected override bool IsSatisfied(IReadOnlyList<IFormFile> files)
        {
            return files.Any() || Optional;
        }
    }
}