using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Validation.File;

namespace Validation
{
    /// <summary>
    /// Builder for creating FormValidators with fluent interface
    /// </summary>
    public class ValidatorBuilder
    {
        private readonly List<BasicField> _requiredFields = new List<BasicField>();
        private NumberStyles _numberStyles = NumberStyles.AllowDecimalPoint;
        private CultureInfo _cultureInfo = CultureInfo.CurrentCulture;

        /// <summary>
        /// Creates a new FormValidator builder
        /// </summary>
        public static ValidatorBuilder New()
        {
            return new ValidatorBuilder();
        }

        // Intergers
        public ValidatorBuilder RequiresInteger(string fieldName, Func<int, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new IntegersField(fieldName, 1, 1))
                : AddRequiredField(new PredicateIntegersField(fieldName, 1, 1, predicate));
        }
        public ValidatorBuilder CanHaveInteger(string fieldName, Func<int, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new IntegersField(fieldName, 1, 1, true))
                : AddRequiredField(new PredicateIntegersField(fieldName, 1, 1, predicate, true));
        }

        public ValidatorBuilder RequiresIntegers(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new IntegersField(fieldName, minAmount, maxAmount))
                : AddRequiredField(new PredicateIntegersField(fieldName, minAmount, maxAmount, predicate));
        }
        public ValidatorBuilder CanHaveIntegers(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new IntegersField(fieldName, minAmount, maxAmount, true))
                : AddRequiredField(new PredicateIntegersField(fieldName, minAmount, maxAmount, predicate, true));
        }


        // Rationals
        public ValidatorBuilder RequiresRational(string fieldName, Func<double, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new RationalsField(fieldName, 1, 1))
                : AddRequiredField(new PredicateRationalsField(fieldName, 1, 1, predicate));
        }
        public ValidatorBuilder CanHaveRational(string fieldName, Func<double, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new RationalsField(fieldName, 1, 1, true))
                : AddRequiredField(new PredicateRationalsField(fieldName, 1, 1, predicate, true));
        }

        public ValidatorBuilder RequiresRationals(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new RationalsField(fieldName, minAmount, maxAmount))
                : AddRequiredField(new PredicateRationalsField(fieldName, minAmount, maxAmount, predicate));
        }
        public ValidatorBuilder CanHaveRationals(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new RationalsField(fieldName, minAmount, maxAmount, true))
                : AddRequiredField(new PredicateRationalsField(fieldName, minAmount, maxAmount, predicate, true));
        }



        // Strings
        public ValidatorBuilder RequiresString(string fieldName, Func<string, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new StringsField(fieldName, 1, 1))
                : AddRequiredField(new PredicateStringsField(fieldName, 1, 1, predicate));
        }
        public ValidatorBuilder RequiresStringWithPattern(string fieldName, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, 1, 1, pattern));
        }
        public ValidatorBuilder CanHaveString(string fieldName, Func<string, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new StringsField(fieldName, 1, 1, true))
                : AddRequiredField(new PredicateStringsField(fieldName, 1, 1, predicate, true));
        }
        public ValidatorBuilder CanHaveStringWithPattern(string fieldName, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, 1, 1, pattern, true));
        }
        public ValidatorBuilder RequiresStrings(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new StringsField(fieldName, minAmount, maxAmount))
                : AddRequiredField(new PredicateStringsField(fieldName, minAmount, maxAmount, predicate));
        }
        public ValidatorBuilder RequiresStringsWithPattern(string fieldName, int minAmount, int maxAmount, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, minAmount, maxAmount, pattern));
        }
        public ValidatorBuilder CanHaveStrings(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new StringsField(fieldName, minAmount, maxAmount, true))
                : AddRequiredField(new PredicateStringsField(fieldName, minAmount, maxAmount, predicate, true));
        }
        public ValidatorBuilder CanHaveStringsWithPattern(string fieldName, int minAmount, int maxAmount, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, minAmount, maxAmount, pattern, true));
        }

        // Files
        public ValidatorBuilder RequiresFile(string fieldName, Func<IFormFile, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new FileField(fieldName, 1, 1))
                : AddRequiredField(new PredicateFileField(fieldName, 1, 1, predicate));
        }
        public ValidatorBuilder CanHaveFile(string fieldName, Func<IFormFile, bool> predicate = null)
        {
            return predicate == null
                ? AddRequiredField(new FileField(fieldName, 1, 1, true))
                : AddRequiredField(new PredicateFileField(fieldName, 1, 1, predicate, true));
        }
        
        
        public ValidatorBuilder WithNumberStyles(NumberStyles numberStyles)
        {
            _numberStyles = numberStyles;
            return this;
        }

        public ValidatorBuilder WithCultureInfo(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            return this;
        }

        /// <summary>
        /// Builds a form validator and returns it. 
        /// </summary>
        /// <returns>FormValidator instance</returns>
        public virtual Validator Build()
        {
            return new Validator(_requiredFields, _numberStyles, _cultureInfo);
        }

        /// <summary>
        /// Clears the builder so it can be used
        /// </summary>
        public void Clear()
        {
            _requiredFields.Clear();
            _numberStyles = NumberStyles.AllowDecimalPoint;
            _cultureInfo = CultureInfo.CurrentCulture;
        }

        private ValidatorBuilder AddRequiredField(BasicField basicField)
        {
            if (_requiredFields.Any(field => field.Fieldname == basicField.Fieldname))
                throw new ArgumentException("Field already added to builder", basicField.Fieldname);
            _requiredFields.Add(basicField);
            return this;
        }

        private ValidatorBuilder()
        {
        }
    }
}