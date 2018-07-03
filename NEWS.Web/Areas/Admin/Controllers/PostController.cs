using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;
using NEWS.Web.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using NEWS.Web.Services;

namespace NEWS.Web.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("Post")]
	[Authorize]
	public class PostController : Controller
	{
		private readonly IPostRepository _postRepository;
		private readonly IUserRepository _userRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly ITagRepository _tagRepository;
		private readonly IPostGalleryRepository _postGalleryRepository;
		private readonly IGalleryRepository _galleryRepository;

		public PostController()
		: this(new PostRepository(), new UserRepository(), new CategoryRepository(), new TagRepository(), new PostGalleryRepository(), new GalleryRepository())
		{

		}

		public PostController(IPostRepository postRepository, IUserRepository userRepository, ICategoryRepository categoryRepository,
								ITagRepository tagRepository, IPostGalleryRepository postGalleryRepository, IGalleryRepository galleryRepository)
		{
			_postRepository = postRepository;
			_userRepository = userRepository;
			_categoryRepository = categoryRepository;
			_tagRepository = tagRepository;
			_postGalleryRepository = postGalleryRepository;
			_galleryRepository = galleryRepository;
		}

		// GET: Admin/Post
		[HttpGet]
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var model = new List<PostViewModel>();

			if (!User.IsInRole("author"))
			{
				var posts = _postRepository.GetAll();
				foreach (var post in posts)
				{
					var user = await _userRepository.GetUserByIdAsync(post.AuthorID);


					model.Add(new PostViewModel
					{
						ID = post.ID,
						Title = post.Title,
						CategoryID = post.CategoryID,
						CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name,
						MainImageUrl = post.MainImageUrl,
						IsGallery = post.IsGallery ?? false,
						Actived = post.Actived,
						UserName = user.UserName,
						FullName = user.FirstName + " " + user.LastName
					});
				}

				return View(model);
			}

			var userLogged = await GetloggedInUser();
			var postForUser = _postRepository.GetPostsByAuthor(userLogged.Id);

			model.AddRange(postForUser.Select(post => new PostViewModel
			{
				ID = post.ID,
				Title = post.Title,
				CategoryID = post.CategoryID,
				CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name,
				MainImageUrl = post.MainImageUrl,
				IsGallery = post.IsGallery,
				Actived = post.Actived,
				UserName = userLogged.UserName,
				FullName = userLogged.FirstName + " " + userLogged.LastName
			}));

			return View(model);
		}

		// GET: Admin/Post/Save
		[HttpGet]
		[Route("Save")]
		public ActionResult Save(long? postID)
		{
			var model = new PostViewModel
			{
				CreateDate = DateTime.Now,
				Categories = _categoryRepository.GetAll(),
				PostGalleries = _postGalleryRepository.GetAll()
			};

			if (!postID.HasValue) return View(model: model);

			var post = _postRepository.GetByID((long)postID);

			var tags = _postRepository.GetTagsPost((long)postID);

			model.ID = postID;
			model.Title = post.Title;
			model.ShortDescription = post.ShortDescription;
			model.LongDescription = post.LongDescription;
			model.CategoryID = post.CategoryID;
			model.Actived = post.Actived;
			model.IsGallery = post.IsGallery;
			model.SelectedTags = tags == null ? null : string.Join(",", tags);
			model.MainImageUrl = post.MainImageUrl;
			model.UserID = post.AuthorID;
			model.PostGalleryID = post.PostGalleryID;


			return View(model: model);
		}

		// POST: Admin/Post/Save
		[HttpPost]
		[Route("Save")]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public async Task<ActionResult> Save(PostViewModel model, HttpPostedFileBase image)
		{

			model.Categories = _categoryRepository.GetAll();
			model.PostGalleries = _postGalleryRepository.GetAll();

			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, "یک مشکلی در ثبت اطلاعات رخ داده است.");
				return View(model);
			}

			try
			{
				var user = await GetloggedInUser();

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
						string myfile = name + "_" + Extensions.RandomString(5) + ext; //appending the name with id
																		// store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
						var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/post/"), myfile);
						model.MainImageUrl = "~/Areas/Admin/assets/post/" + myfile;

						image.SaveAs(path);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت png و jpg و jpeg انتخاب کنید");
					}
				}

				if (model.ID == null) model.ID = 0;

				var post = new Post
				{
					ID = (long)model.ID,
					Title = model.Title,
					ShortDescription = model.ShortDescription,
					LongDescription = model.LongDescription,
					CreateDate = model.CreateDate,
					Actived = model.Actived,
					IsGallery = model.IsGallery,
					MainImageUrl = model.MainImageUrl,
					AuthorID = user.Id,
					CategoryID = model.CategoryID,
					PostGalleryID = model.PostGalleryID
				};

				_postRepository.Save(post);

				var tags = model.SelectedTags.Split(',').ToList();

				foreach (var tag in tags)
				{
					var modelTag = _tagRepository.GetByName(tag);
					var modelPost = _postRepository.GetByTitleDateAuthor(post.Title, (DateTime)post.CreateDate, post.AuthorID);

					if (modelTag == null)
					{
						_tagRepository.Save(new Tag
						{
							Name = tag
						});

						modelTag = _tagRepository.GetByName(tag);

						_postRepository.SetTagToPost(modelPost.ID, modelTag.ID);
					}
					else
					{
						_postRepository.SetTagToPost(modelPost.ID, modelTag.ID);
					}

				}

				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return View(model);
			}
		}

		// GET: Admin/Post/Delete
		[HttpGet]
		[Route("Delete")]
		//public async Task<JsonResult> Delete(long? postID)
		public async Task<ActionResult> Delete(long? postID)
		{
			try
			{
				_postRepository.Delete((long)postID);

				return RedirectToAction("Index");
				//return Json(model, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				var model = new List<PostViewModel>();
				var posts = _postRepository.GetAll();
				foreach (var post in posts)
				{
					var user = await _userRepository.GetUserByIdAsync(post.AuthorID);


					model.Add(new PostViewModel
					{
						ID = post.ID,
						Title = post.Title,
						CategoryID = post.CategoryID,
						CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name,
						MainImageUrl = post.MainImageUrl,
						IsGallery = post.IsGallery,
						Actived = post.Actived,
						UserName = user.UserName,
						FullName = user.FirstName + " " + user.LastName
					});
				}
				ModelState.AddModelError(String.Empty, e.Message);
				//return Json(RedirectToAction("Index"), JsonRequestBehavior.AllowGet);
				return Json(model, JsonRequestBehavior.AllowGet);
			}
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

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _userRepository.GetUserByNameAsync(User.Identity.Name);
		}

		#endregion
	}
}