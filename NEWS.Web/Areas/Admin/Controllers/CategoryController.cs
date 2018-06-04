using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;
using NEWS.Web.Areas.Admin.ViewModels;

namespace NEWS.Web.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("Category")]
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;

		public CategoryController()
			: this(new CategoryRepository())
		{

		}
		public CategoryController(ICategoryRepository categoryRepository)
		{
			_categoryRepository = categoryRepository;
		}


		// GET: Admin/Category
		[Route("")]
		[HttpGet]
		public ActionResult Index()
		{
			var categories = _categoryRepository.GetAll();

			var model = categories.Select(category => new CategoryViewModel
			{
				ID = category.ID,
				CategoryID = category.ID,
				Name = category.Name,
				ParentID = category.ParentId,
				ParentName = (category.ParentId == null || category.ParentId == 0 ? string.Empty: _categoryRepository.GetByID((long)category.ParentId).Name)
			})
				.ToList();

			if (Request.AcceptTypes.Contains("application/json"))
			{
				return Json(categories, JsonRequestBehavior.AllowGet);
			}

			if (User.IsInRole("author"))
			{
				return new HttpUnauthorizedResult();
			}

			return View(model: model, viewName: "Index");
		}


		// product: Admin/Category/Save
		[HttpPost]
		[Route("Save")]
		[Authorize(Roles = "admin, editor")]
		public JsonResult Save(CategoryViewModel model)
		{
			var categories = _categoryRepository.GetAll();

			var newModel = categories.Select(category => new CategoryViewModel
				{
					ID = category.ID,
					CategoryID = category.ID,
					Name = category.Name,
					ParentID = category.ParentId,
					ParentName = (category.ParentId == null || category.ParentId == 0 ? string.Empty : _categoryRepository.GetByID((long)category.ParentId).Name)
				})
				.ToList();

			try
			{
				var category = new Category
				{
					ID = model.ID,
					Name = model.Name,
					ParentId = model.ParentID
				};
				_categoryRepository.Save(category);

				return Json(newModel, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return Json(newModel, JsonRequestBehavior.AllowGet);
			}
		}

		// product: Admin/Category/Delete
		[HttpPost]
		[Route("Delete")]
		[Authorize(Roles = "admin, editor")]
		public JsonResult Delete(int categoryID)
		{
			var categories = _categoryRepository.GetAll();
			var newModel = categories.Select(category => new CategoryViewModel
				{
					ID = category.ID,
					CategoryID = category.ID,
					Name = category.Name,
					ParentID = category.ParentId,
					ParentName = (category.ParentId == null || category.ParentId == 0 ? string.Empty : _categoryRepository.GetByID((long)category.ParentId).Name)
				})
				.ToList();
			try
			{
				_categoryRepository.Delete((long)categoryID);

				return Json(newModel, JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return Json(newModel, JsonRequestBehavior.AllowGet);
			}
		}

	}
}