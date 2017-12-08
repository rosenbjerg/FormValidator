using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace FormValidator
{
    public interface ISatisfiable
    {
        string Fieldname { get; }
        bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo);
        bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo);
    }

    public class OptionalField : ISatisfiable
    {
        private readonly RequiredField _required;

        public OptionalField(RequiredField reqField)
        {
            Fieldname = reqField.Fieldname;
            _required = reqField;
        }

        public string Fieldname { get; }

        public bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return !form.ContainsKey(Fieldname) ||
                   _required.IsSatisfied(form, numberStyles, cultureInfo);
        }
        public bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            return !query.ContainsKey(Fieldname) ||
                   _required.IsSatisfied(query, numberStyles, cultureInfo);
        }
    }
    
    public abstract class RequiredField : ISatisfiable
    {
        private readonly int _minAmount;
        private readonly int _maxAmount;
        
        public string Fieldname { get; }
        
        public abstract bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo);
        public abstract bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo);

        protected RequiredField(string fieldName, int minAmount, int maxAmount, bool optional = false)
        {
            Fieldname = fieldName;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
            Optional = optional;
        }

        public bool Optional { get; }

//        
//        public virtual bool IsSatisfied(IFormCollection form, NumberStyles numberStyles, CultureInfo cultureInfo)
//        {
//            return TryGetField(form, out var field) &&
//                   field.Count >= MinAmount &&
//                   field.Count <= MaxAmount;
//        }
//        public virtual bool IsSatisfied(IQueryCollection query, NumberStyles numberStyles, CultureInfo cultureInfo)
//        {
//            return TryGetField(query, out var field) &&
//                   field.Count >= MinAmount &&
//                   field.Count <= MaxAmount;
//        }
//        
//        
//        
        protected bool TryGetField(IFormCollection form, out StringValues field)
        {
            return form.TryGetValue(Fieldname, out field) &&
                   field.Count >= _minAmount &&
                   field.Count <= _maxAmount;
        }
        protected bool TryGetField(IQueryCollection query, out StringValues field)
        {
            return query.TryGetValue(Fieldname, out field) &&
                   field.Count >= _minAmount &&
                   field.Count <= _maxAmount;
        }
    }
}