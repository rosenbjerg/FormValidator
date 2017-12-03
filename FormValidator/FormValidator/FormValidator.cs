using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace FormValidator
{
    public class FormValidator
    {
        private readonly List<RequiredField> _requiredFields;
        private readonly NumberStyles _numberStyles;
        private readonly CultureInfo _cultureInfo;
        
        public FormValidator(IEnumerable<RequiredField> requiredFields, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            _requiredFields = new List<RequiredField>(requiredFields);
            _numberStyles = numberStyles;
            _cultureInfo = cultureInfo;
        }

        public bool Validate(IFormCollection form)
        {
            var unsatisfied = _requiredFields.Where(rf => !rf.IsSatisfied(form, _numberStyles, _cultureInfo));
            if (unsatisfied.Any())
                return false;
            return true;
            return !unsatisfied.Any();
        }
    }
}