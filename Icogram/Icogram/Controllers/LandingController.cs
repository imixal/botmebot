using System.Web.Mvc;

namespace Icogram.Controllers
{
    public class LandingController : Controller
    {
        [Route("landing")]
        public ActionResult Index()
        {
            return View();
        }
    }
}