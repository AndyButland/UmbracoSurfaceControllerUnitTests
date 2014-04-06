namespace SurfaceControllerUnitTests.Controllers
{
    using System;
    using System.Web.Mvc;
    using SurfaceControllerUnitTests.Controllers.CommandHandler;
    using SurfaceControllerUnitTests.Models;
    using Umbraco.Web;

    public class BlogPostSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        BlogPostSurfaceControllerCommandHandler _commandHandler;

        public BlogPostSurfaceController(): base()
        {
            SetUpCommandHandler();
        }

        public BlogPostSurfaceController(UmbracoContext ctx) : base(ctx)
        {
            SetUpCommandHandler();
        }

        private void SetUpCommandHandler()
        {
            _commandHandler = new BlogPostSurfaceControllerCommandHandler();
            _commandHandler.ModelState = ModelState;
            _commandHandler.TempData = TempData;
        }

        [HttpPost]
        public ActionResult CreateComment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            TempData.Add("CustomMessage", "Thanks for your comment.");

            return RedirectToCurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult CreateCommentWithHandler(CommentViewModel model)
        {
            if (!_commandHandler.HandleCreateComment(model))
            {
                return CurrentUmbracoPage();
            }

            return RedirectToCurrentUmbracoPage();
        }
    }
}