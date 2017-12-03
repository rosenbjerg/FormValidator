using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    class StringField : RequiredField
    {
        protected readonly int MinLength;
        protected readonly int MaxLength;

        public StringField(string fieldName, int minLength = 0, int maxLength = -1) : base(fieldName)
        {
            MinLength = minLength;
            MaxLength = maxLength;
        }

        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   field[0].Length >= MinLength && 
                   (MaxLength == -1 || field[0].Length <= MaxLength);
        }
    }
}