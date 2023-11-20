using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Access_Control_Manager.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using SendGrid.Helpers.Mail;


namespace Access_Control_Manager.Controllers
{
    public class AuthController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthController> _logger;
        private readonly IApplicationUser _user;
        private readonly IAccount _auth;

        public AuthController(IHttpContextAccessor httpContextAccessor, ILogger<AuthController> logger, IApplicationUser user, IAccount auth)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _user = user;
            _auth = auth;
        }

      
        [HttpGet]
        public async Task<IActionResult> SignUp()
        {

            ViewBag.Campus = await GetCampus();
            return View();
        }


        public async Task<List<SelectListItem>> GetCampus()
        {
            var campuses = await _user.GetCampus();

            var list = new List<SelectListItem>();

            foreach (var item in campuses)
            {
                var selectItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString(),
                };

                list.Add(selectItem);
            }
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "---- Select Campus ----"
            };

            list.Insert(0, defItem);

            return list;

        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp signUp, int Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response = await _user.GetUserByEmail(signUp.Email);   

            if(response != null)
            {
                ModelState.AddModelError("", "User already has an account");
                return View();
            }

            try
            {
                User user = new User
                {
                    Email = signUp.Email,
                    Name = signUp.Name,
                    Surname = signUp.Surname,
                    Role = signUp.Role,
                    StudentStaffNumber = signUp.StudentStaffNumber,
                    Department = signUp.Department,
                    CampusId = Id,
                };

                await _user.CreateUser(user);
                await _auth.SignUp(signUp);
                TempData["Message"] = "User Successfully Added";
                return RedirectToAction(nameof(Login));
            }
            catch(Exception ex) 
            {
                _logger.LogInformation(ex.ToString());
                ModelState.AddModelError("", "Operation failed, Something went wrong");
                return View();
            }
          
        }

        [HttpGet]
        public async Task<ActionResult> Login(string returnUrl)
        {
         
            try
            {
                
              

                if (User.Identity.IsAuthenticated)
                {


                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToLocal(returnUrl);
                    }

                    string email = User.FindFirst(ClaimTypes.Email)?.Value;
                    var role = await _user.GetUserRoleByEmail(email);

                    if (role == "Super")
                    {
                        return RedirectToAction("SuperUserHome", "Home");
                    }
                    else if (role == "Assistant")
                    {
                        return RedirectToAction("AssistantUserHome", "Home");
                    }
                    else
                    {
                        return View("AccessDenied");
                    }
                }
                else
                {
                    Login login = new Login();
                    return View(login);
                }
               
            }
            catch (Exception ex)
            {
              
                return View("Error");
            }

        

        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var response = await _user.GetUserByEmail(login.Email);

                if (response == null)
                {
                        TempData["Message"] = "No account found";                       
                        return View(login);                    
                }

                var feedback = await _auth.Login(login);

                if (feedback == "Authentication login failed")
                {
                    ModelState.AddModelError("", "Authentication login failed");
                    return View(login);
                }
                if (feedback == "Invalid Email Address Or Password")
                {
                    ModelState.AddModelError("", feedback);
                    return View(login);
                }

                _httpContextAccessor.HttpContext.Session.SetString("Email", login.Email);
                if (feedback == "Super")
                {
                    _httpContextAccessor.HttpContext.Session.SetString("AccessRight", "Super");
                }
                if (feedback == "Assistant")
                {
                    _httpContextAccessor.HttpContext.Session.SetString("AccessRight", "Assistant");
                }

                if (feedback == "Manager")
                {
                    _httpContextAccessor.HttpContext.Session.SetString("AccessRight", "Manager");
                }

                if (!String.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToLocal(returnUrl);
                }
                else 
                {
                   
                 
                    if (feedback == "Super")
                    {
                        return RedirectToAction("SuperUserHome", "Home");
                    }
                    else if (feedback == "Assistant")
                    {
                        return RedirectToAction("AssistantUserHome", "Home");
                    }
                    else
                    {
                        return View("AccessDenied");
                    }
                }
                            
            }
            else
            {
                ModelState.AddModelError("", "Invalid user credentials");
            }

            return View(login);

        }

  
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
            
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
           
            HttpContext.Session.Remove("AccessRight");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Clear();
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);         
            return RedirectToAction(nameof(Login));
        }


        

    }
}
