using System.Web.Mvc;

namespace Icogram.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Repair()
        {
            Response.StatusCode = 500;
            return View();
        }

    }
}