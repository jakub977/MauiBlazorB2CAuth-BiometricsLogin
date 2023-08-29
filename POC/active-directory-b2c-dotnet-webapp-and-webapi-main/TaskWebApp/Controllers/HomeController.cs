using Microsoft.Identity.Client;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using TaskWebApp.Utils;

namespace TaskWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Claims()
        {
            ViewBag.Message = "Your application description page.";

            IConfidentialClientApplication clientApp = MsalAppBuilder.BuildConfidentialClientApplication();
            string accountId = ClaimsPrincipal.Current.GetB2CMsalAccountIdentifier(Globals.SignUpSignInPolicyId);
            IAccount account = await clientApp.GetAccountAsync(accountId);

            var userTokenCache = clientApp.AppTokenCache;


            IEnumerable<IAccount> accounts = await clientApp.GetAccountsAsync();



            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }
    }
}