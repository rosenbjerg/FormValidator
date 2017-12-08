using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class FormValidator
    {
        private readonly List<ISatisfiable> _requiredFields;
        private readonly NumberStyles _numberStyles;
        private readonly CultureInfo _cultureInfo;
        
        public FormValidator(IEnumerable<ISatisfiable> requiredFields, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            _requiredFields = new List<ISatisfiable>(requiredFields);
            _numberStyles = numberStyles;
            _cultureInfo = cultureInfo;
        }

        public bool Validate(IFormCollection form)
        {
            var unsatisfied = _requiredFields.Where(rf => !rf.IsSatisfied(form, _numberStyles, _cultureInfo));
            if (unsatisfied.Any())
                return false;
            return true;
        }
        public bool Validate(IQueryCollection query)
        {
            var unsatisfied = _requiredFields.Where(rf => !rf.IsSatisfied(query, _numberStyles, _cultureInfo));
            if (unsatisfied.Any())
                return false;
            return true;
        }
    }
}