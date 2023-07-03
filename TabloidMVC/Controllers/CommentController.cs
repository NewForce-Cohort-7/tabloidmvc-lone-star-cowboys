﻿using Microsoft.AspNetCore.Http;
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
                comment.CreateDateTime = DateTime.Now;
                comment.PostId = id;
                comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepo.AddComment(comment);

                return RedirectToAction("Index", "Comment", new { id = id });
            }
            catch
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
