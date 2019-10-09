using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using Validation;

namespace FormValidatorTests
{
    [TestFixture]
    public class FormValidator_Files_Tests
    {
        [Test]
        public void File_OneFile_WrongName_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file2")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void File_OneFile_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveFile("file", f => false)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void File_ThreeFiles_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw"),
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw"),
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresFiles("file", 3, 3)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void File_ThreeFiles_Predicate_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw"),
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw"),
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveFiles("file", 3, 3, f => f.FileName.Contains("daw"))
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void File_TwoFiles_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw"),
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw2")
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file", f => true)
                .RequiresFile("file2")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void File_QueryCollection_AllMissing_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file", f => false)
                .RequiresFile("file2")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void File_AllMissing_Invalid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file", f => false)
                .RequiresFile("file2")
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
        [Test]
        public void File_OneFile_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file", f => true)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void File_NoFile_Valid()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection
            {
                new FormFile(Stream.Null, 0, 0, "file", "adwdaw")
            });
            var validator = ValidatorBuilder
                .New()
                .CanHaveFile("file", f => true)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsTrue(valid);
        }
        [Test]
        public void File_QueryCollection_Invalid()
        {
            // Arrange
            var form = new QueryCollection(new Dictionary<string, StringValues>());
            var validator = ValidatorBuilder
                .New()
                .RequiresFile("file", f => true)
                .Build();

            // Act
            var valid = validator.Validate(form);

            // Assert
            Assert.IsFalse(valid);
        }
    }
}