using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class BookInstanceController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookInstanceService _bookInstanceService;
        public BookInstanceController(IHttpContextAccessor httpContextAccessor, IBookInstanceService bookInstanceService)
        {
            _httpContextAccessor = httpContextAccessor;
            _bookInstanceService = bookInstanceService;
        }

        public IActionResult Manage()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            var books = _bookInstanceService.GetAllBookInstancesAsync(token).Result;
            var bookinstancemodel = new BookInstanceViewModel
            {
                Books = books
            };

            return View(bookinstancemodel);
        }

        public async Task<IActionResult> BookInstanceForm(int? barcode)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var bookInstanceModel = new BookInstanceViewModel();
            if (barcode == null)
            {
                return View("BookInstanceForm", bookInstanceModel);
            }
            var book = await _bookInstanceService.GetBookInstanceByBarcodeAsync(barcode.Value, token);
            bookInstanceModel.BookInstanceDetail = book;
            return View("BookInstanceForm", bookInstanceModel);
        }

        [Route("BookInstance/Add")]
        [HttpPost]
        public async Task<IActionResult> AddBookInstance(BookInstanceViewModel? model)
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
            var bookInstance = new BookInstanceEntity
            {
                BookId = model.Book.BookId,
                BarCode = model.Book.BarCode,
                IsAvailable = model.Book.IsAvailable
            };

            var result = await _bookInstanceService.AddBookInstanceAsync(bookInstance, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding book.");
                return RedirectToAction("BookInstanceForm");
            }
            return RedirectToAction("Manage");
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? barcode)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if (barcode == null)
            {
                return RedirectToAction("Manage");
            }
            await _bookInstanceService.DeleteBookInstanceAsync(barcode.Value, token);
            return RedirectToAction("Manage");
        }


        [HttpPost]
        public async Task<IActionResult> Update(BookInstanceViewModel? update)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if (update.Book.BarCode == 0)
            {
                return RedirectToAction("BookInstanceForm");
            }
            var bookInstanceUpdate = new BookInstanceEntity
            {
                BarCode = update.Book.BarCode,
                BookId = update.Book.BookId,
                IsAvailable = update.Book.IsAvailable
            };
            var result = await _bookInstanceService.UpdateBookInstanceAsync(bookInstanceUpdate, token);
            if (result)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("BookInstanceForm");

        }

        [Route("BookInstance/Barcode/{BarCode}")]
        [HttpGet("{BarCode}")]
        public async Task<IActionResult> GetBookInstanceByBarcode([FromRoute]int BarCode)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var book = await _bookInstanceService.GetBookInstanceByBarcodeAsync(BarCode, token);
            return Ok(book);
        }

    }
}
