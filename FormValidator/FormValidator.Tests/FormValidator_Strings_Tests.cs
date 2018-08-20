using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validation.Tests
{
    [TestClass]
    public class FormValidator_Strings_Tests
    {
        [TestMethod]
        public void Validate_TwoFields_Strings_Unbounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"firstname", "John"},
                {"lastname", "Doe"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("firstname")
                .RequiresString("lastname")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_Unbounded_WrongFields_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"food", "salad"},
                {"drink", "water"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("firstname")
                .RequiresString("lastname")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }


        [TestMethod]
        public void Validate_TwoFields_Strings_Bounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"firstname", "John"},
                {"lastname", "Doe"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("firstname", firstname => firstname.Length >= 1 && firstname.Length <= 50)
                .RequiresString("lastname", lastname => lastname.Length >= 1 && lastname.Length <= 50)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_Bounded_FieldsExists_OutsideBounds_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"language", "danish"},
                {"country", "DK"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("language", language => language.Length >= 1 && language.Length <= 2)
                .RequiresString("country", country => country.Length >= 1 && country.Length <= 2)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [TestMethod]
        public void Validate_TwoFields_Strings_MultipleValues_Bounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"vowels", new StringValues(new [] {"a", "e", "o"})},
                {"consonants", new StringValues(new [] {"q", "v", "n", "d", "m"})}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .RequiresStrings("consonants", 4, 5, consonant => consonant.Length == 1)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [TestMethod]
        public void Validate_TwoFields_Strings_MultipleValues_Bounded_NoFields_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .RequiresStrings("consonants", 4, 5, consonant => consonant.Length == 1)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_MultipleValues_Bounded_NoFields_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .CanHaveStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .CanHaveStrings("consonants", 4, 5, consonant => consonant.Length == 1)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_MultipleValues_Bounded_OneField_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"consonants", new StringValues(new [] {"a", "e", "o"})}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .CanHaveStrings("consonants", 4, 5, consonant => consonant.Length == 1)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Validate_OneField_Strings_Pattern_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"email", "john@doe.net"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStringWithPattern("email", new Regex("\\w+@\\w+\\.\\w+"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [TestMethod]
        public void Validate_OneField_Strings_Pattern_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"email", "john"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStringWithPattern("email", new Regex("\\w+@\\w+\\.\\w+"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Validate_NoFields_Strings_Pattern_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresStringWithPattern("email", new Regex("\\w+@\\w+\\.\\w+"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_Bounded_Pattern_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"vowels", new StringValues(new [] {"a", "e", "o"})},
                {"consonants", new StringValues(new [] {"q", "v", "n", "d", "m"})}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .RequiresStringsWithPattern("consonants", 4, 5, new Regex("[qwrtpsdfghjklzxcvbnm]"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Validate_TwoFields_Strings_Bounded_Pattern_FieldsExists_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"vowels", new StringValues(new [] {"a, e, o"})},
                {"consonants", new StringValues(new [] {"q", "a", "n", "d", "m"})}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .RequiresStringsWithPattern("consonants", 4, 5, new Regex("[qwrtpsdfghjklzxcvbnm]"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void Validate_OneField_String_Predicate_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"type", "diesel"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("type", s => s == "petrol" || s == "diesel")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void Validate_OneField_String_Predicate_FieldsExists_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"type", "rum"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresString("type", s => s == "petrol" || s == "diesel")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}