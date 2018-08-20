using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Validation.Tests
{
    [TestClass]
    public class FormValidator_Integers_Tests
    {
        
        [TestMethod]
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
        
        [TestMethod]
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
        
        [TestMethod]
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
        
        [TestMethod]
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
        
        [TestMethod]
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
        
        
    }
}