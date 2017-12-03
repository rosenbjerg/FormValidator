using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormValidator.Tests
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresString("firstname", 1, 50)
                .RequiresString("lastname", 1, 50)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresString("language", 1, 2)
                .RequiresString("country", 1, 2)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, 1, 1)
                .RequiresStrings("consonants", 5, 1, 1)
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
            var form = new FormCollection(new Dictionary<string, StringValues>());
            var validator = FormValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, 1, 1)
                .RequiresStrings("consonants", 5, 1, 1)
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, 1, 1)
                .RequiresStringsWithPattern("consonants", 5, new Regex("[qwrtpsdfghjklzxcvbnm]"))
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, 1, 1)
                .RequiresStringsWithPattern("consonants", 5, new Regex("[qwrtpsdfghjklzxcvbnm]"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}