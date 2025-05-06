using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using Xunit;

namespace dotnet_starter.Tests.Domain.Entities
{
    public class PostTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            var title = Title.Create("Test Title");
            var content = Content.Create("Test Content");
            var user = new User("testuser", "test@example.com", null);

            // Act
            var post = new Post(title, content, user);

            // Assert
            Assert.Equal(title, post.Title);
            Assert.Equal(content, post.Content);
            Assert.Equal(user.Id, post.UserId);
            Assert.Equal(user, post.User);
            Assert.Equal(PostStatus.Draft, post.Status); // Default status
            Assert.NotEqual(Guid.Empty, post.Id);
            Assert.True(DateTime.UtcNow.Subtract(post.CreatedAt).TotalSeconds < 5); // Created recently
            Assert.Null(post.UpdatedAt);
        }

        [Fact]
        public void Update_ModifiesTitleAndContent_AndSetsUpdatedAt()
        {
            // Arrange
            var initialTitle = Title.Create("Initial Title");
            var initialContent = Content.Create("Initial Content");
            var user = new User("testuser", "test@example.com", null);
            var post = new Post(initialTitle, initialContent, user);

            var newTitle = Title.Create("New Title");
            var newContent = Content.Create("New Content");

            // Act
            post.Update(newTitle, newContent);

            // Assert
            Assert.Equal(newTitle, post.Title);
            Assert.Equal(newContent, post.Content);
            Assert.NotNull(post.UpdatedAt);
            Assert.True(DateTime.UtcNow.Subtract(post.UpdatedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public void Publish_SetsStatusToPublished_AndUpdatesTimestamp()
        {
            // Arrange
            var title = Title.Create("Test Title");
            var content = Content.Create("Test Content");
            var user = new User("testuser", "test@example.com", null);
            var post = new Post(title, content, user);

            // Act
            post.Publish();

            // Assert
            Assert.Equal(PostStatus.Published, post.Status);
            Assert.NotNull(post.UpdatedAt);
            Assert.True(DateTime.UtcNow.Subtract(post.UpdatedAt.Value).TotalSeconds < 5);
        }

        [Fact]
        public void Archive_SetsStatusToArchived_AndUpdatesTimestamp()
        {
            // Arrange
            var title = Title.Create("Test Title");
            var content = Content.Create("Test Content");
            var user = new User("testuser", "test@example.com", null);
            var post = new Post(title, content, user);

            // Act
            post.Archive();

            // Assert
            Assert.Equal(PostStatus.Archived, post.Status);
            Assert.NotNull(post.UpdatedAt);
            Assert.True(DateTime.UtcNow.Subtract(post.UpdatedAt.Value).TotalSeconds < 5);
        }
    }
}