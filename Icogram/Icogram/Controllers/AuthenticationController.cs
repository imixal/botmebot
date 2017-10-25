using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Icogram.Service.Login;
using Icogram.ViewModels.Login;

namespace Icogram.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginService _loginService;

        private IAuthenticationManager _authenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }


        public AuthenticationController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public ActionResult Login(ErrorViewModel model)
        {
            return View(model);
        }

        [AllowAnonymous][HttpPost]
        public async Task<ActionResult> LoginCommand(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var claim = await _loginService.Login(model);
                if (claim != null)
                {
                    _authenticationManager.SignOut();
                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddHours(8)
                    };
                    _authenticationManager.SignIn(properties, claim);

                    return RedirectToRoute("dashboard");
                }
            }

            return RedirectToAction("Login", new ErrorViewModel {Error = "Wrong"});
        }

        public ActionResult LogOut()
        {
            _authenticationManager.SignOut();

            return RedirectToRoute("login");
        }
    }
}