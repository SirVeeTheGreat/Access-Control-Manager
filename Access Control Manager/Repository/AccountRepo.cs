using Access_Control_Manager.Interface;
using Access_Control_Manager.Models;
using Access_Control_Manager.Services;
using FireSharp.Interfaces;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;


using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace Access_Control_Manager.Repository
{
    public class AccountRepo : IAccount
    {
        private FireBaseConnect _connect;
        private Firebase.Auth.IFirebaseAuthProvider _authProvider;
        private IFirebaseClient _client;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUser _user;

        public AccountRepo(ILogger<AccountRepo> logger, IHttpContextAccessor httpContextAccessor, IApplicationUser user)
        {

            _connect = new FireBaseConnect();
            _authProvider = _connect.authProvider;
            _client = _connect.firebaseClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _user = user;
        }



        public async Task<string> Login(Login login)
        {
            string role = ""; 
            try
            {
                var response = await _authProvider.SignInWithEmailAndPasswordAsync(login.Email, login.Password);
                string token = response.FirebaseToken;

                var user = response.User;

                if (!String.IsNullOrEmpty(token))
                {
                    var claims = new List<Claim>();

                    try
                    {
                        _logger.LogInformation("User Email : " + user.Email.ToString());
                        role = await _user.GetUserRoleByEmail(user.Email.ToString());
                        claims.Add(new Claim(ClaimTypes.Email, user.Email));
                        claims.Add(new Claim(ClaimTypes.Authentication, token));
                        if (role == "Super" || role == "Assistant" || role == "Manager")
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = false,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                        };

                        await _httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        
                        return role;
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e.ToString());
                        return "Authentication login failed";
                    }
                }
                else
                {
                    return "Token null";
                }
            }
            catch
            {
                return "Invalid Email Address Or Password";
            }           
        }

        public async Task SignUp(SignUp signUp)
        {
            try
            {
                await _authProvider.CreateUserWithEmailAndPasswordAsync(signUp.Email, signUp.ConfirmPassword, signUp.Name, true);
            }
            catch(Exception e)
            {
                _logger.LogInformation(e.ToString());
            }
 
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
