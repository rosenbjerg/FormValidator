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
    public class FormValidator_Rationals_Tests
    {
        [Test]
        public void Validate_TwoFields_Rational_Unbounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"height", "123.12"},
                {"weight", "321.32"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height")
                .RequiresRational("weight")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [Test]
        public void Validate_TwoFields_Rational_Unbounded_WrongFields_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"firstname", "John"},
                {"lastname", "Doe"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("weight")
                .RequiresRational("height")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [TestCase(1, 2, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3")]
        [TestCase(4, 5, false, "1", "2", "3", "a")]
        [TestCase(4, 5, false, "1", "2", "3", "a", "b")]
        [TestCase(4, 5, true, "1", "2", "3", "4")]
        [TestCase(4, 5, true, "1", "2", "3", "4", "5")]
        public void Validate_OneField_MultipleRationals_DataTest(int min, int max, bool expected, params string[] values)
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"blah", new StringValues(values)}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRationals("blah", min, max)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.AreEqual(valid, expected);
        }
        [Test]
        public void Validate_TwoFields_Rationals_Unbounded_Valid()
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
                .CanHaveRational("zip")
                .CanHaveRationals("age", 2, 3)
                .CanHaveRationals("age2", 2, 3, i => i > 0 && i < 3)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void Validate_QueryCollection_TwoFields_Rationals_Unbounded_Valid()
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
                .CanHaveRational("zip")
                .CanHaveRationals("age", 2, 3)
                .CanHaveRationals("age2", 2, 3, i => i > 0 && i < 3)
                .Build();
            
            // Act
            var valid = validator.Validate(form);
            
            // Assert
            Assert.IsTrue(valid);
        }
   
        [Test]
        public void Validate_TwoFields_Rational_Bounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"height", "123,12"},
                {"weight", "175,32"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height", height => height > 10 && height < 300)
                .RequiresRational("weight", weigth => weigth > 10 && weigth < 300)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [Test]
        public void Validate_TwoFields_Rational_WrongSeparator_Bounded_FieldsExists_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"height", "123.12"},
                {"weight", "175.32"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height", height => height > 10 && height < 300)
                .RequiresRational("weight", weigth => weigth > 10 && weigth < 300)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void Validate_TwoFields_Rational_AltSeparator_Bounded_FieldsExists_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"height", "123.12"},
                {"weight", "175.32"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height", height => height > 10 && height < 300)
                .RequiresRational("weight", weigth => weigth > 10 && weigth < 300)
                .WithCultureInfo(CultureInfo.InvariantCulture)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }

        [Test]
        public void Validate_TwoFields_Rational_Bounded_ValuesOutsideBounds_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"height", "323,12"},
                {"weight", "375,32"}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height", height => height > 10 && height < 300)
                .RequiresRational("weight", weigth => weigth > 10 && weigth < 300)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }

        [Test]
        public void Validate_Rational_Bounded_NoFields_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("height", height => height > 10 && height < 300)
                .RequiresRational("weight", weigth => weigth > 10 && weigth < 300)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void Validate_EmptyField_Optional_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
                {"flops", ""}
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveRational("flops")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        
        [Test]
        public void Validate_EmptyField_Required_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>
            {
                {"flops", ""}
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("flops")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        
        [Test]
        public void Validate_NullForm_Invalid()
        {
            // Arrange
            IQueryCollection form = null;
            
            var validator = ValidatorBuilder
                .New()
                .RequiresRational("flops")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}