using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;
using NEWS.Web.Areas.Admin.Services;
using NEWS.Web.Areas.Admin.ViewModels;

namespace NEWS.Web.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("User")]
	[Authorize]
	public class UserController : Controller
    {
	    private readonly IUserRepository _userRepository;
	    private readonly IRoleRepository _roleRepository;
	    private readonly UserService _users;

	    public UserController()
	    {
		    _userRepository = new UserRepository();
		    _roleRepository = new RoleRepository();
		    _users = new UserService(ModelState, _userRepository, _roleRepository);
	    }


	    // GET: Admin/User/UserList
	    [Route("")]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> UserList()
	    {
		    var users = await _userRepository.GetAllUsersAsync();

			var modelList = new List<UserViewModel>();

		    foreach (var user in users)
		    {
			    var userRole = await _userRepository.GetRolesForUserAsync(user);

			    var model = new UserViewModel
			    {
				    FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					SelectedRole = userRole.FirstOrDefault(),
					UserName = user.UserName,
					ImageUrl = user.ImageUrl,
					Actived = user.Actived
				};

				modelList.Add(model);
		    }


		    return View(modelList);
	    }

	    // GET: Admin/User/Create
	    [Route("Create")]
	    [HttpGet]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Create()
	    {
		    var model = new UserViewModel();
		    model.LoadUserRoles(await _roleRepository.GetAllRolesAsync());

		    return View(model: model);
	    }

	    // product: Admin/User/Create
	    [Route("Create")]
	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    [Authorize(Roles = "admin")]
	    public async Task<ActionResult> Create(UserViewModel model, HttpPostedFileBase image)
	    {
		    var user = await _userRepository.GetUserByNameAsync(model.UserName);

		    if (user != null)
		    {
			    ModelState.AddModelError(string.Empty, "کاربر در حال حاضر در پنل کاربری حاضر است !");
				return View(model: model);
		    }

		    var allowedExtensionsImages = new[] {
			    ".Jpg", ".png", ".jpg", "jpeg"
		    };
		    if (image != null)
		    {
			    var fileName = Path.GetFileName(image.FileName);
			    var ext = Path.GetExtension(image.FileName); //getting the extension(ex-.jpg)
			    if (allowedExtensionsImages.Contains(ext)) //check what type of extension
			    {
				    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
				    string myfile = name + "_" + model.UserName + ext; //appending the name with id
				    // store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
				    var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/userAvatars/"), myfile);
					model.ImageUrl = "~/Areas/Admin/assets/userAvatars/" + myfile;

					image.SaveAs(path);
			    }
			    else
			    {
				    ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت png و jpg و jpeg انتخاب کنید");
			    }
		    }

		    var completed = await _users.CreateAsync(model);

		    if (completed)
		    {
			    return RedirectToAction("UserList");
		    }

		    return View(model: model);
	    }

	    // GET: Admin/User/Edit/username
	    [HttpGet]
	    [Route("Edit/{username}")]
	    [Authorize(Roles = "admin, editor, author")]
	    public async Task<ActionResult> Edit(string username)
	    {
		    var currentUser = User.Identity.Name;

		    if (!User.IsInRole("admin") &&
		        !string.Equals(currentUser, username, StringComparison.CurrentCultureIgnoreCase))
		    {
			    return new HttpUnauthorizedResult();
		    }

		    var user = await _users.GetUserByNameAsync(username);

		    if (user == null)
		    {
			    return HttpNotFound();
		    }

		    return View(model: user);
	    }

	    // product: Admin/User/Edit/username
	    [HttpPost]
	    [Route("Edit/{username}")]
	    [ValidateAntiForgeryToken]
	    [Authorize(Roles = "admin, editor, author")]
	    public async Task<ActionResult> Edit(UserViewModel model, string username, HttpPostedFileBase image)
	    {
		    var user = await _userRepository.GetUserByNameAsync(model.UserName);

		    if (user == null)
		    {
			    ModelState.AddModelError(string.Empty, "کاربر مدنظر شما وجود ندارد.");
				return View(model: model);
		    }

		    var allowedExtensionsImages = new[] {
			    ".Jpg", ".png", ".jpg", "jpeg"
		    };
		    if (image != null)
		    {
			    var fileName = Path.GetFileName(image.FileName);
			    var ext = Path.GetExtension(image.FileName); //getting the extension(ex-.jpg)
			    if (allowedExtensionsImages.Contains(ext)) //check what type of extension
			    {
				    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
				    string myfile = name + "_" + model.UserName + ext; //appending the name with id
				    // store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
				    var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/userAvatars/"), myfile);
				    model.ImageUrl = "~/Areas/Admin/assets/userAvatars/" + myfile;

				    image.SaveAs(path);
			    }
			    else
			    {
				    ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت png و jpg و jpeg انتخاب کنید");
			    }
		    }

			var currentUser = User.Identity.Name;
		    var isAdmin = User.IsInRole("admin");

		    if (!isAdmin &&
		        !string.Equals(currentUser, username, StringComparison.CurrentCultureIgnoreCase))
		    {
			    return new HttpUnauthorizedResult();
		    }

		    var userUpdated = await _users.UpdateUser(model);

		    if (userUpdated)
		    {
			    if (isAdmin)
			    {
				    return RedirectToAction("UserList");
			    }

			    return RedirectToAction("index", "Admin");
		    }

		    return View(model: model);
	    }


	    // product: Admin/User/Delete
	    [HttpPost]
	    [Route("Delete/{username}")]
	    [Authorize(Roles = "admin")]
	    public async Task<JsonResult> Delete(string username)
	    {
		    await _users.DeleteAsync(username);
		    return Json(RedirectToAction("UserList", "User"),JsonRequestBehavior.AllowGet);
	    }

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{

			if (!_isDisposed)
			{
				_userRepository.Dispose();
				_roleRepository.Dispose();
			}
			_isDisposed = true;

			base.Dispose(disposing);
		}

		#endregion
	}
}