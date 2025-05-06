using dotnet_starter.Domain.ValueObjects;
using System;
using Xunit;

namespace dotnet_starter.Tests.Domain.ValueObjects
{
    public class ContentTests
    {
        [Fact]
        public void Create_WithValidValue_ReturnsContentObject()
        {
            // Arrange
            string validContent = "This is valid content for a post";

            // Act
            var content = Content.Create(validContent);

            // Assert
            Assert.NotNull(content);
            Assert.Equal(validContent, content.Value);
        }

        [Fact]
        public void Create_WithNullValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Content.Create(null));
            Assert.Equal("value", exception.ParamName);
        }

        [Fact]
        public void Create_WithEmptyValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Content.Create(string.Empty));
            Assert.Contains("Content cannot be empty", exception.Message);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            // Arrange
            string contentValue = "Test content";
            var content = Content.Create(contentValue);

            // Act
            var result = content.ToString();

            // Assert
            Assert.Equal(contentValue, result);
        }

        [Fact]
        public void Equals_WithSameValue_ReturnsTrue()
        {
            // Arrange
            var content1 = Content.Create("Test content");
            var content2 = Content.Create("Test content");

            // Act & Assert
            Assert.Equal(content1, content2);
            Assert.True(content1.Equals(content2));
            Assert.True(content1 == content2);
            Assert.False(content1 != content2);
        }

        [Fact]
        public void Equals_WithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var content1 = Content.Create("Test content 1");
            var content2 = Content.Create("Test content 2");

            // Act & Assert
            Assert.NotEqual(content1, content2);
            Assert.False(content1.Equals(content2));
            Assert.False(content1 == content2);
            Assert.True(content1 != content2);
        }
    }
}