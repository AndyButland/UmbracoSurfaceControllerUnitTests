namespace SurfaceControllerUnitTests.Tests
{
    using System;
    using System.Web.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SurfaceControllerUnitTests.Controllers;
    using SurfaceControllerUnitTests.Controllers.CommandHandler;
    using SurfaceControllerUnitTests.Models;

    [TestClass]
    public class BlogPostSurfaceControllerCommandHandlerTests
    {
        [TestMethod]
        public void CreateComment_WithValidComment_ReturnsTrueWithMessage()
        {
            // Arrange
            var handler = new BlogPostSurfaceControllerCommandHandler();
            handler.ModelState = new ModelStateDictionary();
            handler.TempData = new TempDataDictionary();
            var model = new CommentViewModel
            {
                Name = "Fred",
                Email = "fred@freddie.com",
                Comment = "Can I test this?",
            };

            // Act
            var result = handler.HandleCreateComment(model);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(handler.TempData["CustomMessage"]);
        }
    }
}
