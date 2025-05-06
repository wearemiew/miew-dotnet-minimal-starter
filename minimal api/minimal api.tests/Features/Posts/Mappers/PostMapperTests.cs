using dotnet_starter.Domain.Entities;
using dotnet_starter.Domain.Enums;
using dotnet_starter.Domain.ValueObjects;
using dotnet_starter.Features.Posts;
using dotnet_starter.Features.Posts.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace dotnet_starter.Tests.Features.Posts.Mappers
{
    public class PostMapperTests
    {
        [Fact]
        public void ToDto_MapsBasicProperties()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", Name.Create("Test", "User"));
            var title = Title.Create("Test Post");
            var content = Content.Create("Test Content");

            var post = new Post(title, content, user);

            // Act
            var dto = PostMapper.ToDto(post);

            // Assert
            Assert.Equal(title.Value, dto.Title);
            Assert.Equal(content.Value, dto.Content);
            Assert.Equal(user.Id, dto.UserId);
            Assert.Equal(user.UserName, dto.UserName);
            Assert.Equal(PostStatus.Draft, dto.Status); // Default status for new posts
        }

        [Fact]
        public void ToDtoList_MapsMultiplePosts()
        {
            // Arrange
            var user = new User("testuser", "test@example.com", Name.Create("Test", "User"));

            var posts = new List<Post>
            {
                new Post(Title.Create("Post 1"), Content.Create("Content 1"), user),
                new Post(Title.Create("Post 2"), Content.Create("Content 2"), user),
                new Post(Title.Create("Post 3"), Content.Create("Content 3"), user)
            };

            // Act
            var dtos = PostMapper.ToDtoList(posts);

            // Assert
            Assert.Equal(3, dtos.Count());
            Assert.Collection(dtos,
                dto => Assert.Equal("Post 1", dto.Title),
                dto => Assert.Equal("Post 2", dto.Title),
                dto => Assert.Equal("Post 3", dto.Title)
            );
        }

        [Fact]
        public void ToDtoList_WithEmptyList_ReturnsEmptyCollection()
        {
            // Arrange
            var posts = new List<Post>();

            // Act
            var dtos = PostMapper.ToDtoList(posts);

            // Assert
            Assert.Empty(dtos);
        }
    }
}