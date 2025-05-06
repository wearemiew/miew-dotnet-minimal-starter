using dotnet_starter.Domain.ValueObjects;
using System;
using Xunit;

namespace dotnet_starter.Tests.Domain.ValueObjects
{
    public class TitleTests
    {
        [Fact]
        public void Create_WithValidValue_ReturnsTitleObject()
        {
            // Arrange
            string validTitle = "This is a valid title";

            // Act
            var title = Title.Create(validTitle);

            // Assert
            Assert.NotNull(title);
            Assert.Equal(validTitle, title.Value);
        }

        [Fact]
        public void Create_WithNullValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Title.Create(null));
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithEmptyValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Title.Create(string.Empty));
            Assert.Contains("Title cannot be empty", exception.Message);
        }

        [Fact]
        public void Create_WithValueExceedingMaxLength_ThrowsArgumentException()
        {
            // Arrange
            var longTitle = new string('a', 201);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Title.Create(longTitle));
            Assert.Contains("Title cannot exceed", exception.Message);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            // Arrange
            string titleValue = "Test title";
            var title = Title.Create(titleValue);

            // Act
            var result = title.ToString();

            // Assert
            Assert.Equal(titleValue, result);
        }

        [Fact]
        public void Equals_WithSameValue_ReturnsTrue()
        {
            // Arrange
            var title1 = Title.Create("Test title");
            var title2 = Title.Create("Test title");

            // Act & Assert
            Assert.Equal(title1, title2);
            Assert.True(title1.Equals(title2));
            Assert.True(title1 == title2);
            Assert.False(title1 != title2);
        }

        [Fact]
        public void Equals_WithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var title1 = Title.Create("Title 1");
            var title2 = Title.Create("Title 2");

            // Act & Assert
            Assert.NotEqual(title1, title2);
            Assert.False(title1.Equals(title2));
            Assert.False(title1 == title2);
            Assert.True(title1 != title2);
        }
    }
}