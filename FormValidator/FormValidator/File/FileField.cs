using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Validation.File
{
    public class FileField : FormField
    {
        public FileField(string fieldName, int minAmount, int maxAmount, bool optional = false) : base(fieldName, minAmount, maxAmount, optional)
        {
            
        }

        public sealed override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            if (!TryGetFileField(form, out var files)) return Optional;
            return AmountOk(files);
        }
    }
}