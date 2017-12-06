using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FormValidator
{
    /// <summary>
    /// Builder for creating FormValidators with fluent interface
    /// </summary>
    public class FormValidatorBuilder
    {
        private readonly List<RequiredField> _requiredFields = new List<RequiredField>();
        private NumberStyles _numberStyles = NumberStyles.AllowDecimalPoint;
        private CultureInfo _cultureInfo = CultureInfo.CurrentCulture;

        /// <summary>
        /// Creates a new FormValidator builder
        /// </summary>
        public static FormValidatorBuilder New()
        {
            return new FormValidatorBuilder();
        }

        // Intergers
        public FormValidatorBuilder RequiresInteger(string fieldName)
        {
            return AddRequiredField(new IntegersField(fieldName, 1, 1));
        }
        public FormValidatorBuilder RequiresInteger(string fieldName, int min, int max)
        {
            return AddRequiredField(new ParsedIntegersField(fieldName, 1, 1, min, max));
        }
        public FormValidatorBuilder RequiresInteger(string fieldName, Func<int, bool> predicate)
        {
            return AddRequiredField(new PredicateIntegersField(fieldName, 1, 1, predicate));
        }

        public FormValidatorBuilder RequiresIntegers(string fieldName, int amount)
            => RequiresIntegers(fieldName, amount, amount);
        public FormValidatorBuilder RequiresIntegers(string fieldName, int minAmount, int maxAmount)
        {
            return AddRequiredField(new IntegersField(fieldName, minAmount, maxAmount));
        }

        public FormValidatorBuilder RequiresIntegers(string fieldName, int amount, Func<int, bool> predicate)
            => RequiresIntegers(fieldName, amount, amount, predicate);
        public FormValidatorBuilder RequiresIntegers(string fieldName, int minAmount, int maxAmount, Func<int, bool> predicate)
        {
            return AddRequiredField(new PredicateIntegersField(fieldName, minAmount, maxAmount, predicate));
        }

        public FormValidatorBuilder RequiresIntegers(string fieldName, int amount, int minValue, int maxValue)
            => RequiresIntegers(fieldName, amount, amount, minValue, maxValue);
        public FormValidatorBuilder RequiresIntegers(string fieldName, int minAmount, int maxAmount, int minValue, int maxValue)
        {
            return AddRequiredField(new ParsedIntegersField(fieldName, minAmount, maxAmount, minValue, maxValue));
        }
        
        
        // Rationals
        public FormValidatorBuilder RequiresRational(string fieldName)
        {
            return AddRequiredField(new RationalsField(fieldName, 1, 1));
        }
        public FormValidatorBuilder RequiresRational(string fieldName, double min, double max)
        {
            return AddRequiredField(new ParsedRationalsField(fieldName, 1, 1, min, max));
        }
        public FormValidatorBuilder RequiresRational(string fieldName, Func<double, bool> predicate)
        {
            return AddRequiredField(new PredicateRationalsField(fieldName, 1, 1, predicate));
        }

        public FormValidatorBuilder RequiresRationals(string fieldName, int amount)
            => RequiresRationals(fieldName, amount, amount);
        public FormValidatorBuilder RequiresRationals(string fieldName, int minAmount, int maxAmount)
        {
            return AddRequiredField(new RationalsField(fieldName, minAmount, maxAmount));
        }

        public FormValidatorBuilder RequiresRationals(string fieldName, int amount, Func<double, bool> predicate)
            => RequiresRationals(fieldName, amount, amount, predicate);
        public FormValidatorBuilder RequiresRationals(string fieldName, int minAmount, int maxAmount, Func<double, bool> predicate)
        {
            return AddRequiredField(new PredicateRationalsField(fieldName, minAmount, maxAmount, predicate));
        }

        public FormValidatorBuilder RequiresRationals(string fieldName, int amount, double minValue, double maxValue)
            => RequiresRationals(fieldName, amount, amount, minValue, maxValue);
        public FormValidatorBuilder RequiresRationals(string fieldName, int minAmount, int maxAmount, double minValue, double maxValue)
        {
            return AddRequiredField(new ParsedRationalsField(fieldName, minAmount, maxAmount, minValue, maxValue));
        }


        // Strings
        public FormValidatorBuilder RequiresString(string fieldName, int minLength = 1, int maxLength = -1)
        {
            return AddRequiredField(new StringsField(fieldName, 1, 1, minLength, maxLength));
        }
        public FormValidatorBuilder RequiresString(string fieldName, Func<string, bool> predicate)
        {
            return AddRequiredField(new PredicateStringsField(fieldName, 1, 1, predicate));
        }
        public FormValidatorBuilder RequiresStringWithPattern(string fieldName, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, 1, 1, pattern));
        }


        public FormValidatorBuilder RequiresStrings(string fieldName, int amount, int minLength = 1, int maxLength = -1)
            => RequiresStrings(fieldName, amount, amount, minLength, maxLength);
        public FormValidatorBuilder RequiresStrings(string fieldName, int minAmount, int maxAmount, int minLength = 1, int maxLength = -1)
        {
            return AddRequiredField(new StringsField(fieldName, minAmount, maxAmount, minLength, maxLength));
        }

        public FormValidatorBuilder RequiresStrings(string fieldName, int amount, Func<string, bool> predicate)
            => RequiresStrings(fieldName, amount, amount, predicate);
        public FormValidatorBuilder RequiresStrings(string fieldName, int minAmount, int maxAmount, Func<string, bool> predicate)
        {
            return AddRequiredField(new PredicateStringsField(fieldName, minAmount, maxAmount, predicate));
        }

        public FormValidatorBuilder RequiresStringsWithPattern(string fieldName, int amount, Regex pattern)
            => RequiresStringsWithPattern(fieldName, amount, amount, pattern);
        public FormValidatorBuilder RequiresStringsWithPattern(string fieldName, int minAmount, int maxAmount, Regex pattern)
        {
            return AddRequiredField(new PatternStringsField(fieldName, minAmount, maxAmount, pattern));
        }
        

        public FormValidatorBuilder WithNumberStyles(NumberStyles numberStyles)
        {
            _numberStyles = numberStyles;
            return this;
        }
        public FormValidatorBuilder WithCultureInfo(CultureInfo cultureInfo)
        {
            _cultureInfo = cultureInfo;
            return this;
        }

        /// <summary>
        /// Builds a form validator and returns it. 
        /// </summary>
        /// <returns>FormValidator instance</returns>
        public FormValidator Build()
        {
            return new FormValidator(_requiredFields, _numberStyles, _cultureInfo);
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
        
        private FormValidatorBuilder AddRequiredField(RequiredField requiredField)
        {
            if (_requiredFields.Any(field => field.FieldName == requiredField.FieldName))
                throw new ArgumentException("Field already added to builder", nameof(requiredField.FieldName));
            _requiredFields.Add(requiredField);
            return this;
        }
        
        private FormValidatorBuilder()
        {
            
        }
    }
}