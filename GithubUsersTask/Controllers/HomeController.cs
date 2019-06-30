using GithubUsersTask.Common.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GithubUsersTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGithubUsersService _githubUsersService;

        public HomeController(IGithubUsersService githubUsersService)
        {
            _githubUsersService = githubUsersService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string username)
        {
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(username))
            {
                // get github user using github api service
                var user = await _githubUsersService.GetUserAsync(username);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Github user not found!";
                    return View();
                }

                return View("Details", user);
            }

            ViewBag.ErrorMessage = "Please provide valid username!";
            return View();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            // handling exceptions related to network/communication 
            // with github api directly at controller level and notifying user.
            // additionally we can log this exception to file or database logging using nlog. 
            filterContext.ExceptionHandled = true;

            // redirecting to index view with error information
            var viewResult = new ViewResult
            {
                ViewName = "Index"
            };
            viewResult.ViewBag.ErrorMessage = string.Format("Something went wrong. Please try again! Technical Details: {0}", filterContext.Exception.ToString());

            filterContext.Result = viewResult;
        }
    }
}