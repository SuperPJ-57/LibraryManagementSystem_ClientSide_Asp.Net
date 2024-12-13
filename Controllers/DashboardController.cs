﻿using ClientSideLibraryManagementSystem.Models;
using ClientSideLibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSideLibraryManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDashboardService _dashboardService;

        public DashboardController(  IHttpContextAccessor httpContextAccessor, IDashboardService dashboardService)
        {
            _httpContextAccessor = httpContextAccessor;
            _dashboardService = dashboardService;
        }
        public IActionResult Index()
        {

            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");
            DashboardData data = _dashboardService.GetDashboardDataAsync(username).Result;
            data.Username = username;

            //if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")  // Check if it's an AJAX request
            //{
            //    return View("Index", data);
            //}
            return View(data);  // Return only the partial view
        }
    }
}