using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLL;

namespace DnD_Mapper___v2.Controllers
{
    public class IdentityController : Controller
    {
        public Models.LoginUserModel loginUser { get; set; }

        public Models.SignupUserModel signupUser { get; set; }


        private readonly BLL.UserManager _userManager;

        public IdentityController(BLL.UserManager userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("user_id");
            return Redirect("../Home/Index");
        }


        [HttpPost]
        public IActionResult Login(Models.LoginUserModel user)
        {
            if (ModelState.IsValid)
            {
                
                BLL.Models.User loginUser = _userManager.AttemptLogin(user.Username, user.Password);
                if (loginUser != null)
                {
                    HttpContext.Response.Cookies.Append("user_id", loginUser.ID.ToString());
                    return Redirect("../Home/Index");
                }
                else
                {
                    ViewBag.LoginResult = "Username or password are incorrect";
                    return View();
                }                
            }
            return View();
        }        

        [HttpPost]
        public IActionResult Register(Models.SignupUserModel user)
        {
            if (ModelState.IsValid)
            {
                UserManager.RegisterState state = _userManager.RegisterUser(user.Username, user.Password);
                
                switch (state)
                {
                    case UserManager.RegisterState.UsernameInUse:
                        ViewBag.RegisterState = "Username already in use";
                        break;

                    case UserManager.RegisterState.SQLError:
                        ViewBag.RegisterState = "A communication error occured, try again later";
                        break;
                }
                
                if (state == UserManager.RegisterState.Success)
                {
                    return Redirect("../Identity/Login");
                }
            }
            return View();
        }
    }
}
