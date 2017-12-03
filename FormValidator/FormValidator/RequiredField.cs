using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public abstract class RequiredField
    {
        protected RequiredField(string fieldName)
        {
            FieldName = fieldName;
        }
        
        public string FieldName { get; }
        enum FieldType
        {
            Integer,
            Decimal,
            String,
            
            
        }

        public abstract bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo);
    }
}