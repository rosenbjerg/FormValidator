using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class ParsedIntegerField : IntegerField
    {
        private readonly int _min;
        private readonly int _max;

        public ParsedIntegerField(string fieldName, int min = int.MinValue, int max = int.MaxValue) : base(fieldName)
        {
            _min = min;
            _max = max;
        }
        public override bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return form.TryGetValue(FieldName, out var field) &&
                   field.Count == 1 &&
                   int.TryParse(field[0], numberStyles, cultureInfo, out var val) &&
                   val >= _min && val <= _max;
        }
    }
}