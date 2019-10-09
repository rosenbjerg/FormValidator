using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Validation;

namespace FormValidatorTests
{
    [TestFixture]
    public class FormValidator_Strings_Tests
    {
        [Test]
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

        [Test]
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


        [Test]
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

        [Test]
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
        [Test]
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
        [Test]
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

        [Test]
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

        [Test]
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
        [Test]
        public void OneField_Strings_MultipleValues_Bounded_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"consonants", "adb"}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveString("consonants", s => s == "123")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void OneField_Strings_MultipleValues_Bounded_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"consonants", "adb"}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveString("consonants", s => s == "adb")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void Validate_TwoFields_Strings_MultipleValues_Bounded_OneField_Pattern_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"consonants", new StringValues(new [] {"a", "e", "o"})}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveStringsWithPattern("vowels", 2, 5, new Regex("[aeo]"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void Validate_TwoFields_String_Unbounded_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"zip", "1"},
                {"zip2", "1"},
                {"age", new StringValues(new []{"1", "2"})},
                {"age2", new StringValues(new []{"1", "2"})},
                {"age3", new StringValues(new []{"1", "2"})}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveStringWithPattern("zip2", new Regex("[12]"))
                .CanHaveStrings("age", 2, 3)
                .CanHaveStrings("age2", 2, 3, i => i == "1" || i == "2")
                .CanHaveStringsWithPattern("age3", 2, 3, new Regex("[12]"))
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void Validate_OneField_Strings_Pattern_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
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
        [Test]
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

        [Test]
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

        [Test]
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
        [Test]
        public void Validate_QueryCollection_TwoFields_Strings_Bounded_Pattern_FieldsExists_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"vowels", new StringValues(new [] {"a", "e", "o"})},
                {"vowels1", new StringValues(new [] {"a", "e", "o"})},
                {"vowels2", new StringValues(new [] {"a", "e", "o"})},
                {"consonants", new StringValues(new [] {"q", "v", "n", "d", "m"})}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("vowels", 2, 5, vowel => vowel.Length == 1)
                .RequiresStrings("vowels1", 2, 3)
                .RequiresStringsWithPattern("consonants", 4, 5, new Regex("[qwrtpsdfghjklzxcvbnm]"))
                .CanHaveStrings("vowels2", 2, 5, vowel => vowel.Length == 1)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
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

        [Test]
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

        [Test]
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
        
        [TestCase(1, 2, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3")]
        [TestCase(4, 5, true, "1", "2", "3", "4")]
        [TestCase(4, 5, true, "1", "2", "3", "4", "5")]
        public void Validate_OneField_MultipleString_DataTest(int min, int max, bool expected, params string[] values)
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"blah", new StringValues(values)}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStrings("blah", min, max)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.AreEqual(valid, expected);
        }
        [TestCase(1, 2, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3", "a")]
        [TestCase(4, 5, false, "1", "2", "3", "a", "b")]
        [TestCase(4, 5, true, "1", "2", "3", "4")]
        [TestCase(4, 5, true, "1", "2", "3", "4", "5")]
        public void Validate_OneField_MultipleString_Pattern_DataTest(int min, int max, bool expected, params string[] values)
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"blah", new StringValues(values)}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresStringsWithPattern("blah", min, max, new Regex("[1-5]"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.AreEqual(valid, expected);
        }
    }
}