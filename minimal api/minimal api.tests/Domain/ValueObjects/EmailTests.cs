using dotnet_starter.Domain.ValueObjects;
using System;
using Xunit;

namespace dotnet_starter.Tests.Domain.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Create_WithValidEmail_ReturnsEmailObject()
        {
            // Arrange
            string validEmail = "test@example.com";

            // Act
            var email = Email.Create(validEmail);

            // Assert
            Assert.NotNull(email);
            Assert.Equal(validEmail, email.Value);
        }

        [Fact]
        public void Create_WithNullValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Email.Create(null));
            Assert.Contains("Email cannot be empty", exception.Message);
        }

        [Fact]
        public void Create_WithEmptyValue_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Email.Create(string.Empty));
            Assert.Contains("Email cannot be empty", exception.Message);
        }

        [Theory]
        [InlineData("invalidemail")]
        [InlineData("invalid@")]
        [InlineData("@invalid.com")]
        [InlineData("inv alid@example.com")]
        public void Create_WithInvalidEmail_ThrowsArgumentException(string invalidEmail)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Email.Create(invalidEmail));
            Assert.Contains("Invalid email format", exception.Message);
        }

        [Fact]
        public void ToString_ReturnsValue()
        {
            // Arrange
            string emailValue = "test@example.com";
            var email = Email.Create(emailValue);

            // Act
            var result = email.ToString();

            // Assert
            Assert.Equal(emailValue, result);
        }

        [Fact]
        public void Equals_WithSameValue_ReturnsTrue()
        {
            // Arrange
            var email1 = Email.Create("test@example.com");
            var email2 = Email.Create("test@example.com");

            // Act & Assert
            Assert.Equal(email1, email2);
            Assert.True(email1.Equals(email2));
            Assert.True(email1 == email2);
            Assert.False(email1 != email2);
        }

        [Fact]
        public void Equals_WithDifferentValue_ReturnsFalse()
        {
            // Arrange
            var email1 = Email.Create("test1@example.com");
            var email2 = Email.Create("test2@example.com");

            // Act & Assert
            Assert.NotEqual(email1, email2);
            Assert.False(email1.Equals(email2));
            Assert.False(email1 == email2);
            Assert.True(email1 != email2);
        }
    }
}