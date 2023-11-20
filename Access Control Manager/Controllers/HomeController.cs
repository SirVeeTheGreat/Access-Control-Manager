using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Access_Control_Manager.Repository;
using Firebase.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using User = Access_Control_Manager.Models.User;

namespace Access_Control_Manager.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IApplicationUser _user;
        private readonly IStudent _student;

        public HomeController(ILogger<HomeController> logger, IApplicationUser user, IStudent student)
        {
            _logger = logger;
            _user = user;
            _student = student;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            

            string email = User.FindFirst(ClaimTypes.Email)?.Value;

            try
            {
                var role = await _user.GetUserRoleByEmail(email);
                if (role == "Super")
                {
                    RedirectToAction(nameof(SuperUserHome));
                }

                if (role == "Assistant")
                {
                    RedirectToAction(nameof(AssistantUserHome));
                }


                return RedirectToAction("LogOut", "Auth");
            }
            catch
            {
                return RedirectToAction("LogOut", "Auth");
            }
            

           
        
        }

        public async Task<ViewResult> Records(long id, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 1;

            var student = await _student.GetStudentRecords(id);
            return View(PaginatedList<StudentRecord>.Create(student.AsQueryable(),pageNumber ?? 1, pageSize ));
        } 


        [Authorize(Policy = "RequireSuperRole")]
        public async Task<ViewResult> Students(long searchQuery, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 4;

            var studentsQuery = await _student.GetAllStudents(searchQuery);

            if (!studentsQuery.Any())
            {
                TempData["Message"] = "Search Completed, No Record's found";
                return View(null);
            }

            return View(PaginatedList<Student>.Create(studentsQuery.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        [Authorize(Policy = "RequireSuperRole")]
        [Route("Admin")]
        public async Task<IActionResult> SuperUserHome(string seacrhQuery, string seacrhBy, int? pageNumber)
        {
            pageNumber = pageNumber ?? 1;
            int pageSize = 4;

            TempData["SearchQuery"] = seacrhQuery;
           var users = await _user.GetAllUsers(seacrhQuery, seacrhBy);

            var currentUser =  users.FirstOrDefault(a => a.Email == User.FindFirst(ClaimTypes.Email)?.Value);

            if(currentUser != null && currentUser.AccountStatus == 0)
            {
                TempData["Message"] = "Account Disabled";
                return View(null);
            }

            if(!users.Any())
            {
                TempData["Message"] = "Search Completed, No Record's found";
                return View(null);
            }

            if (!String.IsNullOrEmpty(seacrhQuery))
            {
                TempData["Message"] = $"Search Completed, {users.Count()} Record's found";
            }

            return View(PaginatedList<User>.Create(users.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            return View();
        }

        [Authorize(Policy = "RequireAssistantOrManager")]
        public IActionResult AssistantUserHome()
        {
            return View("Menu");
        }
      
        public IActionResult Privacy()
        {
            return View();
        }


        [Authorize(Policy = "RequireSuperRole")]
        public async Task<IActionResult> EnableAccount(string id)
        {
            var user = await _user.GetUserByEmail(id);
            user.AccountStatus = 1;
            await _user.Save();
            TempData["Message"] = "Account Enabled";
            return RedirectToAction(nameof(SuperUserHome));
        }


        [Authorize(Policy = "RequireSuperRole")]
        public async Task<IActionResult> DisableAccount(string id)
        {
            var user = await _user.GetUserByEmail(id);
            user.AccountStatus = 0;
            await _user.Save();
            TempData["Message"] = "Account Disabled";
            return RedirectToAction(nameof(SuperUserHome));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}