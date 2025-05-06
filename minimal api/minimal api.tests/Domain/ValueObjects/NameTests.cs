using dotnet_starter.Domain.ValueObjects;
using System;
using Xunit;

namespace dotnet_starter.Tests.Domain.ValueObjects
{
    public class NameTests
    {
        [Fact]
        public void Create_WithValidNames_ReturnsNameObject()
        {
            // Arrange
            string firstName = "John";
            string lastName = "Doe";

            // Act
            var name = Name.Create(firstName, lastName);

            // Assert
            Assert.NotNull(name);
            Assert.Equal(firstName, name.FirstName);
            Assert.Equal(lastName, name.LastName);
        }

        [Fact]
        public void Create_WithNullFirstName_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Name.Create(null, "Doe"));
            Assert.Contains("First name", exception.Message);
        }

        [Fact]
        public void Create_WithNullLastName_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => Name.Create("John", null));
            Assert.Contains("Last name", exception.Message);
        }

        [Theory]
        [InlineData("", "Doe")]
        [InlineData("John", "")]
        public void Create_WithEmptyValue_ThrowsArgumentException(string firstName, string lastName)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => Name.Create(firstName, lastName));
        }

        [Fact]
        public void ToString_ReturnsFullName()
        {
            // Arrange
            var name = Name.Create("John", "Doe");
            var expected = "John Doe";

            // Act
            var result = name.ToString();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Equals_WithSameValues_ReturnsTrue()
        {
            // Arrange
            var name1 = Name.Create("John", "Doe");
            var name2 = Name.Create("John", "Doe");

            // Act & Assert
            Assert.Equal(name1, name2);
            Assert.True(name1.Equals(name2));
            Assert.True(name1 == name2);
            Assert.False(name1 != name2);
        }

        [Theory]
        [InlineData("Jane", "Doe")]
        [InlineData("John", "Smith")]
        public void Equals_WithDifferentValues_ReturnsFalse(string firstName, string lastName)
        {
            // Arrange
            var name1 = Name.Create("John", "Doe");
            var name2 = Name.Create(firstName, lastName);

            // Act & Assert
            Assert.NotEqual(name1, name2);
            Assert.False(name1.Equals(name2));
            Assert.False(name1 == name2);
            Assert.True(name1 != name2);
        }
    }
}