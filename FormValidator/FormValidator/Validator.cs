using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Validation
{
    
    public class Validator
    {
        private readonly List<BasicField> _fields;
        private readonly NumberStyles _numberStyles;
        private readonly CultureInfo _cultureInfo;
        
        public Validator(IEnumerable<BasicField> fields, NumberStyles numberStyles, CultureInfo cultureInfo)
        {
            _fields = new List<BasicField>(fields);
            _numberStyles = numberStyles;
            _cultureInfo = cultureInfo;
        }

        public bool Validate(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            var unsatisfied = _fields.Where(rf => !rf.IsSatisfied(form, _numberStyles, _cultureInfo));
            if (unsatisfied.Any())
                return false;
            return true;
        }
        public bool Validate(IQueryCollection query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            var unsatisfied = _fields.Where(rf => !rf.IsSatisfied(query, _numberStyles, _cultureInfo));
            if (unsatisfied.Any())
                return false;
            return true;
        }
    }
}