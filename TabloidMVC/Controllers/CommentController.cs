using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepo;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepo = commentRepository;
        }
        // GET: CommentsController
        public ActionResult Index(int id)
        {
            //This grabs the comments for the specific post as a list and orders them from most recent according to the time it was created
            List<Comment> comments = _commentRepo.GetCommentByPostId(id).OrderByDescending(x => x.CreateDateTime).ToList();

            return View(comments);
        }

        // GET: CommentsController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CommentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Comment comment)
        {
            try
            {
                //This all had to be added since the code will not create it itself, nor is it information the user would know off the top of their head to input.
                //We create the DateTime and add it right then and ther
                comment.CreateDateTime = DateTime.Now;
                //This is where we add the Post.Id since code doesn't do it manually like a normal Id does
                comment.PostId = id;
                //Filling in the UserProfileId by matching it with the CurrentUserProfileId
                comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepo.AddComment(comment);

                //This return statement takes the user back to ex:/Comment/Index/2
                return RedirectToAction("Index", "Comment", new { id = id });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }
        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

    }
}
