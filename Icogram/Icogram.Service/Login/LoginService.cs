using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Icogram.DataAccessLayer.EntityFramework.Identity;
using Icogram.DataAccessLayer.Interfaces;
using Icogram.ViewModels.Login;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Icogram.Service.Login
{
    public class LoginService : ILoginService
    {
        private readonly IIcogramUnitOfWork _icogramUnitOfWork;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationRoleManager _applicationRoleManager;

        public LoginService(IIcogramUnitOfWork icogramUnitOfWork)
        {
            _icogramUnitOfWork = icogramUnitOfWork;
            _applicationUserManager = _icogramUnitOfWork.UserManager;
            _applicationRoleManager = _icogramUnitOfWork.RoleManager;
        }


        public async Task<ClaimsIdentity> Login(LoginViewModel loginViewModel)
        {
            var user = await _applicationUserManager.FindAsync(loginViewModel.Username, loginViewModel.Password);
            if (user == null) return null;
            var claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String));
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName, ClaimValueTypes.String));
            var roles = await _applicationUserManager.GetRolesAsync(user.Id);
            var role = roles.First();
            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role, ClaimValueTypes.String));
            claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                "OWIN Provider", ClaimValueTypes.String));

            return claim;
        }

        public Task RestoreRassword(RestorePasswordViewModel restorePasswordViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}