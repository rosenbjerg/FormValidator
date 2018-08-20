using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validation.Tests
{
    [TestClass]
    public class FormValidator_Rationals_Tests
    {
        [TestMethod]
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

        [TestMethod]
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


        [TestMethod]
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

        [TestMethod]
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
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
    }
}