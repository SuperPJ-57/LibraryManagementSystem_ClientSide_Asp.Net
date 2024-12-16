using ClientSideLibraryManagementSystem.Models;
using Lms.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStudentService _studentService;
        public StudentController(IHttpContextAccessor httpContextAccessor, IStudentService studentService)
        {
            _httpContextAccessor = httpContextAccessor;
            _studentService = studentService;
        }
        public IActionResult Manage()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            var response = _studentService.GetAllStudentsAsync(token).Result;
            var students = new StudentViewModel
            {
                Students = response
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")  // Check if it's an AJAX request
            {
                return View("Manage",students);
            }

            return View(students);
        }
        
        public async Task<IActionResult> StudentForm(int? id)
       {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            StudentViewModel studentModel = new StudentViewModel();
            if(id == null)
            {
                return View("StudentForm",studentModel);
            }
            var student = await _studentService.GetStudentByIdAsync(id.Value, token);

            studentModel = new StudentViewModel
            {
                Student = student,
                Students = null,
                AddStudentModel = null
            };
            return View("StudentForm", studentModel);

        }

        [Route("Student/Id/{studentId}")]
        [HttpGet("{studentId}")]
        public async Task<IActionResult> Id([FromRoute] int studentId)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                RedirectToAction("Login", "Auth");
            }
            var student = await _studentService.GetStudentByIdAsync(studentId, token);
            return Ok(student);
        }

        [Route("Student/Add")]
        [HttpPost]
        public async Task<IActionResult> AddStudent(StudentViewModel model)
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
            var student = new StudentsEntity
            {
                Name = model.Student.Name,
                Email = model.Student.Email,
                ContactNumber = model.Student.ContactNumber,
                Department = model.Student.Department
            };

            var result = await _studentService.AddStudentAsync(student, token);
            if (!result)
            {
                ModelState.AddModelError("", "Error adding student.");
                return RedirectToAction("StudentForm");
            }
            return RedirectToAction("Manage");
        }


        [Route("Student/Delete")]
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
            await _studentService.DeleteStudentAsync(id.Value, token);
            return RedirectToAction("Manage");
        }

        [Route("Student/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(StudentViewModel? update)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            if (update.Student.StudentId == 0)
            {
                return RedirectToAction("StudentForm");
            }
            var studentUpdate = new StudentsEntity
            {
                StudentId = update.Student.StudentId,
                Name = update.Student.Name,
                Email = update.Student.Email,
                ContactNumber = update.Student.ContactNumber,
                Department = update.Student.Department

            };
            var result = await _studentService.UpdateStudentAsync(studentUpdate, token);
            if (result)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("AddStudentForm");

        }
    }
}
