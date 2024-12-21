using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookService _bookService;
        public BookController(IHttpContextAccessor httpContextAccessor, IBookService bookService)
        {
            _httpContextAccessor = httpContextAccessor;
            _bookService = bookService;
        }
        public IActionResult Manage()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            var books = _bookService.GetAllBooksAsync(token).Result;
            var bookmodel = new BookViewModel
            {
                Books = books
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")  // Check if it's an AJAX request
            {
                return View("Manage", bookmodel);
            }
            return View(bookmodel);
        }

        [Route("Book/AllBooks")]
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors([FromQuery] string? query = null)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var authors = await _bookService.GetAllBooksAsync(token, query);
            return Json(authors);
        }

        [Route("Book/Id/{bookId}")]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> Id([FromRoute]int bookId)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                RedirectToAction("Login", "Auth");
            }
            var book = await _bookService.GetBookByIdAsync(bookId, token);
            return Ok(book);
        }

        public async Task<IActionResult> BookForm(int? id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var bookmodel = new BookViewModel();
            if(id == null)
            {
                return View("BookForm", bookmodel);
            }
            var book = await _bookService.GetBookByIdAsync(id.Value, token);
            bookmodel.Book = book;
            return View("BookForm", bookmodel);
        }

        [Route("Book/Register")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookViewModel? model)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            //if (ModelState.IsValid == false)
            //{
            //    return RedirectToAction("Manage");
            //}
            var book = new BooksEntity
            {
                Title = model.Book.Title,
                Genre = model.Book.Genre,
                AuthorId = model.Book.AuthorId,
                ISBN = model.Book.ISBN
            };
            var result = await _bookService.AddBookAsync(book, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding book.");
                return RedirectToAction("BookForm");
            }
            return RedirectToAction("Manage");
        }


        [Route("Book/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if (id == null)
            {
                return RedirectToAction("Manage");
            }
            await _bookService.DeleteBookAsync(id.Value, token);
            return RedirectToAction("Manage");
        }

        [Route("Book/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(BookViewModel? update)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if(update.Book.BookId == 0)
            {
                return RedirectToAction("BookForm");
            }
            var bookUpdate = new BooksEntity
            {
                BookId = update.Book.BookId,
                Title = update.Book.Title,
                Genre = update.Book.Genre,
                AuthorId = update.Book.AuthorId,
                ISBN = update.Book.ISBN
            };
            var result = await _bookService.UpdateBookAsync(bookUpdate, token);
            if (result)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("BookForm");

        }
    }
}
