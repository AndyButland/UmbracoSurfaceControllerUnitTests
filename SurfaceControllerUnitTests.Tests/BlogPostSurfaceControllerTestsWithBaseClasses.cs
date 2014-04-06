namespace SurfaceControllerUnitTests.Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;
    using NUnit.Framework;
    using SurfaceControllerUnitTests.Controllers;
    using SurfaceControllerUnitTests.Models;
    using Umbraco.Core.Models;
    using Umbraco.Tests.TestHelpers;
    using Umbraco.Web.Mvc;
    using Umbraco.Web.Routing;

[TestFixture]
[DatabaseTestBehavior(DatabaseBehavior.NoDatabasePerFixture)]
public class BlogPostSurfaceControllerTestsWithBaseClasses : BaseRoutingTest
{
        [Test]        
        public void CreateComment_WithValidComment_RedirectsWithMessage()
        {
            // Arrange
            var controller = GetController();
            var model = new CommentViewModel
            {
                Name = "Fred",
                Email = "fred@freddie.com",
                Comment = "Can I test this?",
            };

            // Act
            var result = controller.CreateComment(model);

            // Assert
            var redirectToUmbracoPageResult = result as RedirectToUmbracoPageResult;
            Assert.IsNotNull(redirectToUmbracoPageResult);
            Assert.AreEqual(1000, redirectToUmbracoPageResult.PublishedContent.Id);
            Assert.IsNotNull(controller.TempData["CustomMessage"]);
        }

        [Test]
        public void CreateComment_WithInValidComment_RedisplaysForm()
        {
            // Arrange
            var controller = GetController();
            var model = new CommentViewModel
            {
                Name = "Fred",
                Email = string.Empty,
                Comment = "Can I test this?",
            };
            controller.ModelState.AddModelError("Email", "Email is required.");

            // Act
            var result = controller.CreateComment(model);

            // Assert
            var umbracoPageResult = result as UmbracoPageResult;
            Assert.IsNotNull(umbracoPageResult);
            Assert.IsNull(controller.TempData["CustomMessage"]);
        }

        private BlogPostSurfaceController GetController()
        {
            // Create contexts via test base class methods
            var routingContext = GetRoutingContext("/test");
            var umbracoContext = routingContext.UmbracoContext;
            var contextBase = umbracoContext.HttpContext;

            // We need to add a value to the controller's RouteData, otherwise calls to CurrentPage
            // (or RedirectToCurrentUmbracoPage) will fail

            // Unfortunately some types and constructors necessary to do this are marked as internal

            // Create instnce of RouteDefinition class using reflection
            // - note: have to use LoadFrom not LoadFile here to type can be cast (http://stackoverflow.com/questions/3032549/c-on-casting-to-the-same-class-that-came-from-another-assembly
            var assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umbraco.dll"));
            var reflectedRouteDefinitionType = assembly.GetType("Umbraco.Web.Mvc.RouteDefinition");
            var routeDefinition = Activator.CreateInstance(reflectedRouteDefinitionType);

            // Similarly create instance of PublishedContentRequest
            // - note: have to do this a little differently as in this case the class is public but the constructor is internal
            var reflectedPublishedContentRequestType = assembly.GetType("Umbraco.Web.Routing.PublishedContentRequest"); 
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var culture = CultureInfo.InvariantCulture;
            var publishedContentRequest = Activator.CreateInstance(reflectedPublishedContentRequestType, flags, null, new object[] { new Uri("/test", UriKind.Relative), routingContext }, culture);

            // Set properties on reflected types (not all of them, just the ones that are needed for the test to run)
            var publishedContentRequestPublishedContentProperty = reflectedPublishedContentRequestType.GetProperty("PublishedContent");
            publishedContentRequestPublishedContentProperty.SetValue(publishedContentRequest, MockIPublishedContent(), null);
            var publishedContentRequestProperty = reflectedRouteDefinitionType.GetProperty("PublishedContentRequest");
            publishedContentRequestProperty.SetValue(routeDefinition, publishedContentRequest, null);

            // Then add it to the route data tht will be passed to the controller context
            // - without it SurfaceController.CurrentPage will throw an exception of: "Cannot find the Umbraco route definition in the route values, the request must be made in the context of an Umbraco request"
            var routeData = new RouteData();
            routeData.DataTokens.Add("umbraco-route-def", routeDefinition);

            // Create the controller with the appropriate contexts
            var controller = new BlogPostSurfaceController(umbracoContext);
            controller.ControllerContext = new ControllerContext(contextBase, routeData, controller);
            controller.Url = new UrlHelper(new RequestContext(contextBase, new RouteData()), new RouteCollection());
            return controller;
        }

        private IPublishedContent MockIPublishedContent()
        {
            var mock = new Mock<IPublishedContent>();
            mock.Setup(x => x.Id).Returns(1000);
            return mock.Object;
        }
    }
}
