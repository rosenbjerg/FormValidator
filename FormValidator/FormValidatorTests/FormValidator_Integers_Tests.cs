using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Validation;

namespace FormValidatorTests
{
    [TestFixture]
    public class FormValidator_Integers_Tests
    {
        
        [Test]
        public void Validate_OneField_Integer_Unbounded_FieldExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"number", "32423" },
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresInteger("number")
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        
        [Test]
        public void Validate_OneField_Integer_AllBounded_FieldExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"zip", "9000" },
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresInteger("zip", zip => zip > 1000 && zip < 9999)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        
        [Test]
        public void Validate_TwoFields_Integer_AllBounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"age", "24"},
                {"zip", "9000"},
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresInteger("zip", zip => zip > 1000 && zip < 9999)
                .RequiresInteger("age", age => age > 0 && age < 150)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        
        [Test]
        public void Validate_TwoFields_Integer_Unbounded_NoFields_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresInteger("zip")
                .RequiresInteger("age")
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsFalse(valid);
        }
        
        [Test]
        public void Validate_TwoFields_Integer_Unbounded_WrongFields_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"firstname", "John"},
                {"lastname", "Doe"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresInteger("zip")
                .RequiresInteger("age")
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void Validate_TwoFields_Integer_Unbounded_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"zip", "1"},
                {"age", new StringValues(new []{"1", "2"})},
                {"age2", new StringValues(new []{"1", "2"})}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveInteger("zip")
                .CanHaveIntegers("age", 2, 3)
                .CanHaveIntegers("age2", 2, 3, i => i > 0 && i < 3)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void Validate_QueryCollection_TwoFields_Integer_Unbounded_Valid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"zip", "1"},
                {"age", new StringValues(new []{"1", "2"})},
                {"age2", new StringValues(new []{"1", "2"})}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveInteger("zip")
                .CanHaveIntegers("age", 2, 3)
                .CanHaveIntegers("age2", 2, 3, i => i > 0 && i < 3)
                .WithCultureInfo(CultureInfo.CurrentCulture)
                .WithNumberStyles(NumberStyles.Integer)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        [TestCase(1, 2, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3", "a")]
        [TestCase(4, 5, false, "1", "2", "3", "a", "b")]
        [TestCase(4, 5, true, "1", "2", "3", "4")]
        [TestCase(4, 5, true, "1", "2", "3", "4", "5")]
        public void Validate_OneField_MultipleIntegers_DataTest(int min, int max, bool expected, params string[] values)
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"blah", new StringValues(values)}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresIntegers("blah", min, max)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.AreEqual(valid, expected);
        }
        
        [TestCase(1, 2, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3")]
        [TestCase(4, 5, true, "1", "2", "3", "4")]
        [TestCase(4, 5, false, "1", "2", "3", "a")]
        [TestCase(4, 5, true, "1", "2", "3", "4", "5")]
        [TestCase(4, 5, false, "1", "2", "3", "a", "b")]
        [TestCase(4, 5, false, "-1", "2", "3", "4", "105")]
        public void Validate_OneField_MultipleIntegers_Predicate_DataTest(int min, int max, bool expected, params string[] values)
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"blah", new StringValues(values)}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresIntegers("blah", min, max, i => i > 0 && i < 10)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.AreEqual(valid, expected);
        }
    }
}