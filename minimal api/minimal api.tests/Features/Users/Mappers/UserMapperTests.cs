using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Users.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace dotnet_starter.Tests.Features.Users.Mappers
{
    public class UserMapperTests
    {
        [Fact]
        public void ToDto_MapsAllProperties()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", Name.Create("Test", "User"));

            // Act
            var dto = UserMapper.ToDto(user);

            // Assert
            Assert.NotNull(dto.Id); // We can't know the ID, but it shouldn't be null
            Assert.Equal("testuser", dto.Username);
            Assert.Equal("test@example.com", dto.Email);
            // CreatedAt is set in the constructor, so we can test it's not the default value
            Assert.NotEqual(default, dto.CreatedAt);
            // UpdatedAt should be null initially
            Assert.Null(dto.UpdatedAt);
        }

        [Fact]
        public void ToDto_WithNullName_MapsOtherProperties()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", null);

            // Act
            var dto = UserMapper.ToDto(user);

            // Assert
            Assert.NotNull(dto.Id); // We can't know the ID, but it shouldn't be null
            Assert.Equal("testuser", dto.Username);
            Assert.Equal("test@example.com", dto.Email);
        }

        [Fact]
        public void ToDtoList_MapsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User("user1", "user1@example.com", Name.Create("User", "One")),
                new User("user2", "user2@example.com", Name.Create("User", "Two")),
                new User("user3", "user3@example.com", Name.Create("User", "Three"))
            };

            // Act
            var dtos = UserMapper.ToDtoList(users);

            // Assert
            Assert.Equal(3, dtos.Count());

            // Extract usernames for comparison
            var usernames = dtos.Select(d => d.Username).ToList();
            Assert.Contains("user1", usernames);
            Assert.Contains("user2", usernames);
            Assert.Contains("user3", usernames);
        }

        [Fact]
        public void ToDtoList_WithEmptyList_ReturnsEmptyCollection()
        {
            // Arrange
            var users = new List<User>();

            // Act
            var dtos = UserMapper.ToDtoList(users);

            // Assert
            Assert.Empty(dtos);
        }
    }
}