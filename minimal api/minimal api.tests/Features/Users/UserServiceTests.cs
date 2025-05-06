using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Users;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace dotnet_starter.Tests.Features.Users
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User("user1", "user1@example.com", Name.Create("User", "One")),
                new User("user2", "user2@example.com", Name.Create("User", "Two"))
            };

            _mockUserRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(users);
            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockUserRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
        {
            // Arrange
            var userId = "user123";
            var user = new User("testuser", "test@example.com", Name.Create("Test", "User"));

            // Setup the mock to return the user with the right ID
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal("testuser", result.Username);
            Assert.Equal("test@example.com", result.Email);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            var userId = "nonexistent";

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _userService.GetUserByIdAsync(userId));

            Assert.Contains($"User with ID {userId} not found", exception.Message);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_WithValidData_CreatesAndReturnsUser()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "Password123!"
            };

            _mockUserRepository.Setup(repo => repo.ExistsByEmailAsync(createUserDto.Email))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(repo => repo.ExistsByUsernameAsync(createUserDto.Username))
                .ReturnsAsync(false);

            // Setup the mock to return a new user with the right properties
            _mockUserRepository.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync((User user, string _) => user);

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.Equal(createUserDto.Username, result.Username);
            Assert.Equal(createUserDto.Email, result.Email);

            _mockUserRepository.Verify(repo => repo.ExistsByEmailAsync(createUserDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.ExistsByUsernameAsync(createUserDto.Username), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>(), createUserDto.Password), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "newuser",
                Email = "existing@example.com",
                Password = "Password123!"
            };

            _mockUserRepository.Setup(repo => repo.ExistsByEmailAsync(createUserDto.Email))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _userService.CreateUserAsync(createUserDto));

            Assert.Contains("Email is already in use", exception.Message);
            _mockUserRepository.Verify(repo => repo.ExistsByEmailAsync(createUserDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_WithExistingUsername_ThrowsException()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "existing",
                Email = "newuser@example.com",
                Password = "Password123!"
            };

            _mockUserRepository.Setup(repo => repo.ExistsByEmailAsync(createUserDto.Email))
                .ReturnsAsync(false);

            _mockUserRepository.Setup(repo => repo.ExistsByUsernameAsync(createUserDto.Username))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _userService.CreateUserAsync(createUserDto));

            Assert.Contains("Username is already taken", exception.Message);
            _mockUserRepository.Verify(repo => repo.ExistsByEmailAsync(createUserDto.Email), Times.Once);
            _mockUserRepository.Verify(repo => repo.ExistsByUsernameAsync(createUserDto.Username), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidData_UpdatesAndReturnsUser()
        {
            // Arrange
            var userId = "user123";
            var updateUserDto = new UpdateUserDto
            {
                Email = "updated@example.com",
                Username = "updateduser"
            };

            // Create a user that will be returned by the repository
            var user = new User("originaluser", "original@example.com", Name.Create("Original", "User"));

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(updateUserDto.Email))
                .ReturnsAsync((User?)null);

            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync(updateUserDto.Username))
                .ReturnsAsync((User?)null);

            _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userService.UpdateUserAsync(userId, updateUserDto);

            // Assert
            Assert.Equal(updateUserDto.Username, result.Username);
            Assert.Equal(updateUserDto.Email, result.Email);

            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WithInvalidUserId_ThrowsException()
        {
            // Arrange
            var userId = "nonexistent";
            var updateUserDto = new UpdateUserDto
            {
                Email = "updated@example.com"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _userService.UpdateUserAsync(userId, updateUserDto));

            Assert.Contains($"User with ID {userId} not found", exception.Message);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_CallsRepositoryDeleteMethod()
        {
            // Arrange
            var userId = "user123";
            _mockUserRepository.Setup(repo => repo.DeleteAsync(userId))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(repo => repo.DeleteAsync(userId), Times.Once);
        }
    }
}