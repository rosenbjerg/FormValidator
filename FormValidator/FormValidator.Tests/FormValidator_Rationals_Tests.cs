using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FormValidator.Tests
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresRational("height", 100, 250)
                .RequiresRational("weight", 10, 200)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresRational("height", 100, 250)
                .RequiresRational("weight", 10, 200)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresRational("height", 100, 250)
                .RequiresRational("weight", 10, 200)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresRational("height", 100, 250)
                .RequiresRational("weight", 10, 200)
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
            var validator = FormValidatorBuilder
                .New()
                .RequiresRational("height", 100, 250)
                .RequiresRational("weight", 10, 200)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}