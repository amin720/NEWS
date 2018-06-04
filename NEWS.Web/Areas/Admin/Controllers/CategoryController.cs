using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;

namespace NEWS.Web.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("Category")]
	[Authorize]
	public class CategoryController : Controller
    {
	    private readonly ICategoryRepository _categoryRepository;

	    public CategoryController()
			:this(new CategoryRepository())
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

			if (Request.AcceptTypes.Contains("application/json"))
			{
				return Json(categories, JsonRequestBehavior.AllowGet);
			}

			if (User.IsInRole("author"))
			{
				return new HttpUnauthorizedResult();
			}

			return View(model: categories, viewName: "Index");
		}

	    // product: Admin/Category/Create
	    [HttpPost]
	    [Route("Create")]
	    [Authorize(Roles = "admin, editor")]
	    public JsonResult Create(Category category)
	    {
		    if (!ModelState.IsValid)
		    {
			    return Json(HttpNotFound());
		    }

		    try
		    {
				_categoryRepository.Save(category);

			    return Json(_categoryRepository.GetAll(), JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception ex)
		    {
				ModelState.AddModelError(String.Empty, ex.Message);
			    return Json(_categoryRepository.GetAll(),JsonRequestBehavior.AllowGet);
			}
	    }

	    // Get: Admin/Category/Edit/category
	    [HttpGet]
	    [Route("edit/{categoryId}")]
	    [Authorize(Roles = "admin, editor")]
	    public JsonResult Edit(long categoryId)
	    {
		    try
		    {
			    Category category = _categoryRepository.GetByID(categoryId);



			    return Json(category, JsonRequestBehavior.AllowGet);

		    }
		    catch (Exception ex)
		    {
				ModelState.AddModelError(String.Empty, ex.Message);
			    return Json(_categoryRepository.GetAll(), JsonRequestBehavior.AllowGet);
		    }
	    }

	}
}