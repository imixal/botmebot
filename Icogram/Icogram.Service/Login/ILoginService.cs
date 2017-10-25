using System.Security.Claims;
using System.Threading.Tasks;
using Icogram.ViewModels.Login;

namespace Icogram.Service.Login
{
    public interface ILoginService
    {
        Task<ClaimsIdentity> Login(LoginViewModel loginViewModel);

        Task RestoreRassword(RestorePasswordViewModel restorePasswordViewModel);
    }
}