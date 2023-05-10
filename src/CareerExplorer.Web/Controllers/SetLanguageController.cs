using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CareerExplorer.Web.Controllers
{
    public class SetLanguageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(5) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
