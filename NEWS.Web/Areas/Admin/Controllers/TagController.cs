using System;
using System.Collections.Generic;
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
	[RoutePrefix("Tag")]
	[Authorize]
	public class TagController : Controller
    {
		private readonly ITagRepository _tagRepository;

		public TagController()
			: this(new TagRepository())
		{

		}
		public TagController(ITagRepository tagRepository)
		{
			_tagRepository= tagRepository;
		}


		// GET: Admin/Tag
		[Route("")]
		[HttpGet]
		public ActionResult Index()
		{
			var tags = _tagRepository.GetAll();

			if (Request.AcceptTypes.Contains("application/json"))
			{
				return Json(tags, JsonRequestBehavior.AllowGet);
			}

			if (User.IsInRole("author"))
			{
				return new HttpUnauthorizedResult();
			}

			return View(model: tags, viewName: "Index");
		}


		// product: Admin/Tag/Save
		[HttpPost]
		[Route("Save")]
		[Authorize(Roles = "admin, editor")]
		public JsonResult Save(Tag tag)
		{
			try
			{
				_tagRepository.Save(tag);

				return Json(_tagRepository.GetAll(), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return Json(_tagRepository.GetAll(), JsonRequestBehavior.AllowGet);
			}
		}

		// product: Admin/Tag/Delete
		[HttpPost]
		[Route("Delete")]
		[Authorize(Roles = "admin, editor")]
		public JsonResult Delete(int tagID)
		{
			try
			{
				_tagRepository.Delete((long)tagID);

				return Json(_tagRepository.GetAll(), JsonRequestBehavior.AllowGet);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return Json(_tagRepository.GetAll(), JsonRequestBehavior.AllowGet);
			}
		}
	}
}