﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;
using TabloidMVC.Models;

namespace TabloidMVC.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
              _tagRepository = tagRepository;
        }

        public IActionResult Index()
        {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        // GET: TagController/Create
        public IActionResult Create()
        {

            var vm = new TagCreateViewModel();
            vm.TagOptions = _tagRepository.GetAllTags();
            return View(vm);
        }

        // POST: TagController/Create
        [HttpPost]
        public IActionResult Create(TagCreateViewModel vm)
        {
            try
            {
                vm.Tag.Name = vm.Tag.Name.Trim();

                _tagRepository.AddTag(vm.Tag);

                return RedirectToAction("Index", new {id = vm.Tag.Id});
            }
            catch
            {
                vm.TagOptions = _tagRepository.GetAllTags();
                return View(vm);
            }
        }

        // GET: TagController/Delete/5
        public IActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            tag.Name = _tagRepository.GetTagById(id).Name; // this is a hack to get the name to show up on the delete page

            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public IActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.UpdateTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
