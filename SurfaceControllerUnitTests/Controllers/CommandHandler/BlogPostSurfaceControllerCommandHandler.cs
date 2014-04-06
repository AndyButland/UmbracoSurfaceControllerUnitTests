namespace SurfaceControllerUnitTests.Controllers.CommandHandler
{
    using System;
    using System.Web.Mvc;
    using SurfaceControllerUnitTests.Models;

    public class BlogPostSurfaceControllerCommandHandler
    {
        public ModelStateDictionary ModelState { get; set; }

        public TempDataDictionary TempData { get; set; }

        public bool HandleCreateComment(CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            TempData.Add("CustomMessage", "Thanks for your comment.");
            return true;
        }
    }
}