using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorService _authorService;

        public AuthorController(IHttpContextAccessor httpContextAccessor, IAuthorService authorService)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorService = authorService;
        }
        public IActionResult Manage()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            var authors = _authorService.GetAllAuthorsAsync(token).Result;
            var authormodel = new AuthorViewModel
            {
                Authors = authors,
                Author = null
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")  // Check if it's an AJAX request
            {
                return PartialView("Manage", authormodel);
            }
            return View(authormodel);
        }


        //Get all authors
        [Route("Author/AllAuthors")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery]string? query=null)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var authors = await _authorService.GetAllAuthorsAsync(token,query);
            return Json(authors);
        }


        public async Task<IActionResult> AuthorForm(int? id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            AuthorViewModel authormodel = new AuthorViewModel();
            if(id == null)
            {
                return View("AuthorForm", authormodel);
            }
            var author = await _authorService.GetAuthorByIdAsync(id.Value, token);
            authormodel.Author = author;
            return View("AuthorForm", authormodel);
        }
        [Route("Author/Register")]
        [HttpPost]
        public async Task<IActionResult> AddAuthor(AuthorViewModel? model)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            //ModelState.Remove("AuthorId");
            //if (ModelState.IsValid == false)
            //{
            //    return RedirectToAction("AddAuthorForm");
            //}
            var author = new AuthorsEntity
            {
                Name = model.Author.Name,
                Bio = model.Author.Bio
            };
            var result = await _authorService.AddAuthorAsync(author, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding author.");
                return RedirectToAction("AuthorForm");
            }
            return RedirectToAction("Manage");
        }

        [Route("Author/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if(id == null)
            {
                return RedirectToAction("Manage");
            }
            await _authorService.DeleteAuthorAsync(id.Value, token);
            return RedirectToAction("Manage");
        }

        [Route("Author/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(AuthorViewModel? update)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if (update.Author.AuthorId == 0)
            {
                return RedirectToAction("AuthorForm");
            }

            var authorUpdate = new AuthorsEntity
            {
                AuthorId = update.Author.AuthorId,
                Name = update.Author.Name,
                Bio = update.Author.Bio
            };
            var result = await _authorService.UpdateAuthorAsync(authorUpdate, token);
            if(result)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("AuthorForm");

        }


        //[HttpPost]
        //public async Task<IActionResult> AddOrUpdateAuthor(AuthorViewModel model)
        //{
        //    var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }

        //    if (model.Author != null && model.Author.AuthorId > 0)
        //    {
        //        // Update logic
        //        var author = await _authorService.GetAuthorByIdAsync(model.Author.AuthorId, token);
        //        if (author == null)
        //        {
        //            return NotFound();
        //        }

        //        author.Name = model.Author.Name;
        //        author.Bio = model.Author.Bio;

        //        var result = await _authorService.UpdateAuthorAsync(author, token);
        //        if (!result)
        //        {
        //            ModelState.AddModelError("", "Error updating author.");
        //            return RedirectToAction("AddAuthorForm");
        //        }
        //    }
        //    else
        //    {
        //        // Add logic
        //        var newAuthor = new AuthorsEntity
        //        {
        //            Name = model.AddAuthorModel.Name,
        //            Bio = model.AddAuthorModel.Bio
        //        };

        //        var result = await _authorService.AddAuthorAsync(newAuthor, token);
        //        if (!result)
        //        {
        //            ModelState.AddModelError("", "Error adding author.");
        //            return RedirectToAction("AddAuthorForm");
        //        }
        //    }

        //    return RedirectToAction("Manage");
        //}

        //[Route("Author/EditAuthor")]
        //[HttpPost]
        //public async Task<IActionResult> EditAuthor(Add)
    }
}
