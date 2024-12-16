using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITransactionService _transactionService;
        public TransactionController(IHttpContextAccessor httpContextAccessor, ITransactionService transactionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _transactionService = transactionService;
        }
        public IActionResult Display()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var transactions = _transactionService.GetAllTransactionsAsync(token).Result;
            var transactionViewModel = new TransactionViewModel
            {
                transactionDetails = transactions
            };
            return View("GetTransactions", transactionViewModel);
        }

        [Route("Transaction/All/{query}")]
        public async Task<IEnumerable<TransactionDetails>> SearchTransaction([FromRoute]string? query)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            var transactions = await _transactionService.GetAllTransactionsAsync(token,query);
            return transactions;

        }

        //redirect to borrow form view
        [Route("Transaction/Borrow")]
        public IActionResult Borrow() {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var borrowData = new BorrowData
            {
                UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId")),
                Username = _httpContextAccessor.HttpContext.Session.GetString("Username")

            };
            var borrowmodel = new TransactionViewModel
            {
                BorrowData = borrowData
            };
            return View("Borrow", borrowmodel);

        }

        //redirect to return formview
        [Route("Transaction/Id/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionById([FromRoute] int id)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var transaction = await _transactionService.GetTransactionByIdAsync(id, token);
            return Ok(transaction);
        }

        [Route("Transaction/Return")]
        public IActionResult Return(int? Tid)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var borrowData = new BorrowData
            {
                UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetString("UserId")),
                Username = _httpContextAccessor.HttpContext.Session.GetString("Username")

            };
            AddTransactionModel model = new AddTransactionModel();
            TransactionDetails transactionDetails = new TransactionDetails();
            var returnmodel = new TransactionViewModel
            {
                BorrowData = borrowData
            };
            if (Tid != null)
            {
                
                var transaction = _transactionService.GetTransactionByIdAsync(Tid.Value, token).Result;
                model.BookId = transaction.BookId;
                model.StudentId = transaction.StudentId;
                model.BarCode = transaction.BarCode;
                transactionDetails.Name = transaction.Name;
                transactionDetails.Title = transaction.Title;
                returnmodel.Transaction = model;
                returnmodel.TransactionDetails = transactionDetails;
            }
            
            return View("Return", returnmodel);
        }
        

        [Route("Transaction/Register")]
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionViewModel model)
        {
            
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            
            var transac = new TransactionsEntity
            {
                StudentId = model.Transaction.StudentId,
                UserId = model.Transaction.UserId,
                BookId = model.Transaction.BookId,
                BarCode = model.Transaction.BarCode,
                TransactionType = model.Transaction.TransactionType,
                Date = model.Transaction.Date
            };
            var result = await _transactionService.AddTransactionAsync(transac, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding transaction.");
                return RedirectToAction("Borrow");
            }
            return RedirectToAction("Display");
        }

        [Route("Transaction/RegisterReturn")]
        [HttpPost]
        public async Task<IActionResult> AddBookReturn(TransactionViewModel model)
        {

            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var transac = new TransactionsEntity
            {
                StudentId = model.Transaction.StudentId,
                UserId = model.Transaction.UserId,
                BookId = model.Transaction.BookId,
                BarCode = model.Transaction.BarCode,
                TransactionType = model.Transaction.TransactionType,
                Date = model.Transaction.Date
            };
            var result = await _transactionService.AddTransactionAsync(transac, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding transaction.");
                return RedirectToAction("Return");
            }
            return RedirectToAction("Display");
        }

        //[HttpGet]
        //public async Task<IActionResult> GetTransactions()
        //{
        //    var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    var response = await _transactionService.GetAllTransactionsAsync(token);
        //    //if(type != null)
        //    //{
        //    //    response = response.Where(x => x.TransactionType == type).ToList();
        //    //}
        //    var transactions = new TransactionViewModel
        //    {
        //        transactionDetails = response
        //    };
        //    //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")  // Check if it's an AJAX request
        //    //{
        //    //    return PartialView("_GetTransactionsPartial", transactions);
        //    //}

        //    return View("GetTransactions",transactions);
        //}
    }
}
