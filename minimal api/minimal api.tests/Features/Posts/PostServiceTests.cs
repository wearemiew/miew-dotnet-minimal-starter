using dotnet_starter.Application.Interfaces.Repositories;
using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Posts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace dotnet_starter.Tests.Features.Posts
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly PostService _postService;
        private readonly User _testUser;

        public PostServiceTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _postService = new PostService(_mockPostRepository.Object, _mockUserRepository.Object);

            // Create a test user that will be used across tests
            _testUser = new User("testuser", "test@example.com", Name.Create("Test", "User"));

            // Setup the mock to make ID available without reflection
            _mockUserRepository.Setup(repo => repo.GetByIdAsync("user123"))
                .ReturnsAsync(_testUser);
        }

        [Fact]
        public async Task GetAllPostsAsync_ReturnsAllPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post(Title.Create("Post 1"), Content.Create("Content 1"), _testUser),
                new Post(Title.Create("Post 2"), Content.Create("Content 2"), _testUser)
            };

            _mockPostRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetAllPostsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            _mockPostRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPostByIdAsync_WithValidId_ReturnsPost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post(Title.Create("Test Post"), Content.Create("Test Content"), _testUser);

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(post);

            // Act
            var result = await _postService.GetPostByIdAsync(postId);

            // Assert
            Assert.Equal("Test Post", result.Title);
            Assert.Equal("Test Content", result.Content);
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
        }

        [Fact]
        public async Task GetPostsByUserIdAsync_ReturnsUserPosts()
        {
            // Arrange
            var userId = "user123";
            var posts = new List<Post>
            {
                new Post(Title.Create("User Post 1"), Content.Create("Content 1"), _testUser),
                new Post(Title.Create("User Post 2"), Content.Create("Content 2"), _testUser)
            };

            _mockPostRepository.Setup(repo => repo.GetByUserIdAsync(userId))
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsByUserIdAsync(userId);

            // Assert
            Assert.Equal(2, result.Count());
            _mockPostRepository.Verify(repo => repo.GetByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetPostsByStatusAsync_ReturnsPostsWithStatus()
        {
            // Arrange
            var status = PostStatus.Published;
            var posts = new List<Post>
            {
                new Post(Title.Create("Published Post 1"), Content.Create("Content 1"), _testUser),
                new Post(Title.Create("Published Post 2"), Content.Create("Content 2"), _testUser)
            };

            // Make posts published using public API
            foreach (var post in posts)
            {
                post.Publish();
            }

            _mockPostRepository.Setup(repo => repo.GetByStatusAsync(status))
                .ReturnsAsync(posts);

            // Act
            var result = await _postService.GetPostsByStatusAsync(status);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, postDto => Assert.Equal(PostStatus.Published, postDto.Status));
            _mockPostRepository.Verify(repo => repo.GetByStatusAsync(status), Times.Once);
        }

        [Fact]
        public async Task CreatePostAsync_WithValidData_CreatesAndReturnsPost()
        {
            // Arrange
            var userId = "user123";
            var createPostDto = new CreatePostDto
            {
                Title = "New Post",
                Content = "New Content",
                Status = PostStatus.Draft
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync(_testUser);

            _mockPostRepository.Setup(repo => repo.CreateAsync(It.IsAny<Post>()))
                .ReturnsAsync((Post post) => post);

            // Act
            var result = await _postService.CreatePostAsync(userId, createPostDto);

            // Assert
            Assert.Equal("New Post", result.Title);
            Assert.Equal("New Content", result.Content);
            Assert.Equal(PostStatus.Draft, result.Status);

            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockPostRepository.Verify(repo => repo.CreateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task CreatePostAsync_WithInvalidUserId_ThrowsException()
        {
            // Arrange
            var userId = "invaliduser";
            var createPostDto = new CreatePostDto
            {
                Title = "New Post",
                Content = "New Content"
            };

            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId))
                .ReturnsAsync((User?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _postService.CreatePostAsync(userId, createPostDto));

            Assert.Contains($"User with ID {userId} not found", exception.Message);
            _mockUserRepository.Verify(repo => repo.GetByIdAsync(userId), Times.Once);
            _mockPostRepository.Verify(repo => repo.CreateAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePostAsync_WithValidData_UpdatesAndReturnsPost()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDto
            {
                Title = "Updated Title",
                Content = "Updated Content",
                Status = PostStatus.Published
            };

            var post = new Post(Title.Create("Original Title"), Content.Create("Original Content"), _testUser);

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync(post);

            _mockPostRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Post>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _postService.UpdatePostAsync(postId, updatePostDto);

            // Assert
            Assert.Equal("Updated Title", result.Title);
            Assert.Equal("Updated Content", result.Content);
            Assert.Equal(PostStatus.Published, result.Status);

            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
            _mockPostRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePostAsync_WithInvalidPostId_ThrowsException()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var updatePostDto = new UpdatePostDto
            {
                Title = "Updated Title"
            };

            _mockPostRepository.Setup(repo => repo.GetByIdAsync(postId))
                .ReturnsAsync((Post?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _postService.UpdatePostAsync(postId, updatePostDto));

            Assert.Contains($"Post with ID {postId} not found", exception.Message);
            _mockPostRepository.Verify(repo => repo.GetByIdAsync(postId), Times.Once);
            _mockPostRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task DeletePostAsync_CallsRepositoryDeleteMethod()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockPostRepository.Setup(repo => repo.DeleteAsync(postId))
                .Returns(Task.CompletedTask);

            // Act
            await _postService.DeletePostAsync(postId);

            // Assert
            _mockPostRepository.Verify(repo => repo.DeleteAsync(postId), Times.Once);
        }
    }
}