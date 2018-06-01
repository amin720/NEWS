using System;
using System.Threading.Tasks;
using System.Web;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using NEWS.Web.Areas.Admin.ViewModels;

namespace NEWS.Web.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[RoutePrefix("")]
	[Authorize]
	public class AdminController : Controller
    {
	    private readonly IUserRepository _userRepository;

	    public AdminController()
		    : this(new UserRepository())
	    {
	    }

	    public AdminController(IUserRepository userRepository)
	    {
		    _userRepository = userRepository;
	    }

	    // GET: Admin/Admin
	    [Route("")]
		public ActionResult Index()
        {
            return View();
        }

	    // GET: Admin/Admin/Login
	    [HttpGet]
	    [Route("login")]
	    [AllowAnonymous]
	    public ActionResult Login()
	    {
		    return View();
	    }

	    // product: Admin/Admin/Login
	    [HttpPost]
	    [Route("login")]
	    [AllowAnonymous]
	    [ValidateAntiForgeryToken]
	    public async Task<ActionResult> Login(LoginViewModel model)
	    {
		    try
		    {
			    var user = await _userRepository.GetLoginUserAsync(model.UserName, model.Password);

			    if (user == null)
			    {
				    //ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
				    ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
			    }

			    var authManager = HttpContext.GetOwinContext().Authentication;

			    var userIdentity = await _userRepository.CreateIdentityAsync(user);

			    authManager.SignIn(new AuthenticationProperties() { IsPersistent = model.Remmeberme }, userIdentity);


			    return RedirectToAction("Index", "Admin");
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError(string.Empty, e.Message);
			    return View(viewName: "Login");
		    }
	    }

	    // GET: Admin/Admin/Logout
	    [HttpGet]
	    [Route("logout")]
	    [AllowAnonymous]
	    public ActionResult Logout()
	    {
		    var authManager = HttpContext.GetOwinContext().Authentication;

		    authManager.SignOut();

		    return RedirectToAction("Index");
	    }

		#region Method

		private bool _isDisposed;

	    protected override void Dispose(bool disposing)
	    {
		    if (!_isDisposed)
		    {
			    _userRepository.Dispose();
		    }

		    _isDisposed = true;
		    base.Dispose(disposing);
	    }

		#endregion
	}
}