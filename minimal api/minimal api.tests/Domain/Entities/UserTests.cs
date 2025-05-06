using System;
using System.Reflection;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using Xunit;

namespace dotnet_starter.Tests.Domain.Entities
{
    public class UserTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            var username = "testuser";
            var email = "test@example.com";
            var name = Name.Create("John", "Doe");

            // Act
            var user = new User(username, email, name);

            // Assert
            Assert.Equal(username, user.UserName);
            Assert.Equal(email, user.Email);
            Assert.Equal(name, user.Name);
            Assert.Equal(UserStatus.Active, user.Status);
            Assert.True(DateTime.UtcNow.Subtract(user.CreatedAt).TotalSeconds < 5);
            Assert.Null(user.UpdatedAt);
            Assert.Empty(user.Posts);
        }

        [Fact]
        public void Update_ModifiesNameAndStatus_AndSetsUpdatedAt()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", Name.Create("John", "Doe"));
            var newName = Name.Create("Jane", "Smith");
            var newStatus = UserStatus.Inactive;

            // Act
            user.Update(newName, newStatus);

            // Assert
            Assert.Equal(newName, user.Name);
            Assert.Equal(newStatus, user.Status);
            Assert.NotNull(user.UpdatedAt);
            Assert.True(DateTime.UtcNow.Subtract(user.UpdatedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public void Deactivate_SetsStatusToInactive_AndUpdatesTimestamp()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", Name.Create("John", "Doe"));

            // Act
            user.Deactivate();

            // Assert
            Assert.Equal(UserStatus.Inactive, user.Status);
            Assert.NotNull(user.UpdatedAt);
            Assert.True(DateTime.UtcNow.Subtract(user.UpdatedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public void Constructor_WithNoName_SetsNameToNull()
        {
            // Arrange & Act
            var user = new User("testuser", "test@example.com", null);

            // Assert
            Assert.Null(user.Name);
            Assert.Equal(UserStatus.Active, user.Status);
            Assert.True(DateTime.UtcNow.Subtract(user.CreatedAt).TotalSeconds < 5);
        }
    }
}