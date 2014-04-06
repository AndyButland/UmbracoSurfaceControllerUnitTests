namespace SurfaceControllerUnitTests.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SurfaceControllerUnitTests.Controllers;
    using SurfaceControllerUnitTests.Models;

    [TestClass]
    public class BlogPostSurfaceControllerTests
    {
        [TestMethod]
        public void CreateComment_WithValidComment_RedirectsWithMessage()
        {
            // Arrange
            var controller = new BlogPostSurfaceController();
            var model = new CommentViewModel
            {
                Name = "Fred",
                Email = "fred@freddie.com",
                Comment = "Can I test this?",
            };

            // Act
            var result = controller.CreateComment(model);

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
