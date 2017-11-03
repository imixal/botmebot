using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Icogram.Models.ResourcesModels;
using Icogram.Service.Login;
using Icogram.ViewModels.Login;
using Service;

namespace Icogram.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ICrudService<Resource> _resourceCrudService;

        private IAuthenticationManager _authenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }


        public AuthenticationController(ILoginService loginService, ICrudService<Resource> resourceCrudService)
        {
            _loginService = loginService;
            _resourceCrudService = resourceCrudService;
        }

        [HttpGet]
        public async Task<ActionResult> Login(ErrorViewModel model)
        {
            if (model == null)
            {
                model = new ErrorViewModel();
            }
            model.Resources =await _resourceCrudService.GetAllAsync();

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