﻿using System;
using System.Collections.Generic;
using System.IO;
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
	[RoutePrefix("Gallery")]
	[Authorize]
	public class GalleryController : Controller
    {
	    private readonly IPostGalleryRepository _postGalleryRepository;

	    private readonly IGalleryRepository _galleryRepository;

	    public GalleryController()
		    : this(new PostGalleryRepository(), new GalleryRepository())
	    {
		    
	    }
	    public GalleryController(IPostGalleryRepository postGalleryRepository,IGalleryRepository galleryRepository)
	    {
		    _postGalleryRepository = postGalleryRepository;
		    _galleryRepository = galleryRepository;
	    }

        // GET: Admin/PostGallery
		[HttpGet]
		[Route("")]
        public ActionResult ListPostGallery()
		{
			var model = new GalleryViewModel
			{
				PostGalleries = _postGalleryRepository.GetAll()
			};

			return View(model);
		}

	    // GET: Admin/PostGallery/Create
	    [HttpGet]
	    public JsonResult SavePostGallery(GalleryViewModel model)
	    {
		    model.PostGalleries = _postGalleryRepository.GetAll();

		    try
		    {
				var postgallery = new PostGallery
				{
					ID = model.PostGalleryID,
					Name = model.PostGalleryName,
				};

				_postGalleryRepository.Save(postgallery);

			    return Json(model, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError(String.Empty, e.Message);
			    return Json(model, JsonRequestBehavior.AllowGet);
		    }
	    }

	    // GET: Admin/PostGallery/Delete
	    [HttpGet]
	    public JsonResult DeletePostGallery(long postgalleryID)
	    {
			var model = new GalleryViewModel
			{
				PostGalleries = _postGalleryRepository.GetAll()
			};

		    try
		    {
			    _postGalleryRepository.Delete(postgalleryID);

			    return Json(model, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError(String.Empty, e.Message);
			    return Json(model, JsonRequestBehavior.AllowGet);
		    }
	    }

	    // GET: Admin/Gallery
	    [HttpGet]
	    [Route("Gallery")]
	    public ActionResult ListGallery(long postgalleryID)
	    {
		    var model = new GalleryViewModel
		    {
				PostGalleryID = postgalleryID,
			    Galleries = _galleryRepository.GetGallerysByPostGallery(postgalleryID)
		    };

		    return View(model);
	    }

	    // POST: Admin/PostGallery/Create
	    [HttpPost]
		[ValidateAntiForgeryToken]
	    public ActionResult SaveGallery(GalleryViewModel model, HttpPostedFileBase file)
	    {
		    model.Galleries = _galleryRepository.GetGallerysByPostGallery(model.PostGalleryPostID);

		    if (!ModelState.IsValid)
		    {
			    ModelState.AddModelError(string.Empty, "یک مشکلی در ثبت اطلاعات رخ داده است.");
				return RedirectToAction("ListGallery", new { postgalleryID = model.PostGalleryID });
			}

			try
		    {
			    var allowedExtensionsImages = new[] {
				    ".Jpg", ".png", ".jpg", "jpeg"
			    };
			    if (file != null)
			    {
				    var fileName = Path.GetFileName(file.FileName);
				    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)
				    if (allowedExtensionsImages.Contains(ext)) //check what type of extension
				    {
					    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extensi
					    string myfile = name + "_" + model.GalleryTitle + ext; //appending the name with id
					    // store the file inside ~/project folder(Img)E:\Project-Work\Zahra.Project\Restaurant\Restaurant.Web\assets\images\products\1.png
					    var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/gallery/"), myfile);
					    model.GalleryFileName = "~/Areas/Admin/assets/gallery/" + myfile;

					    file.SaveAs(path);
				    }
				    else
				    {
					    ModelState.AddModelError(string.Empty, "لطفا یک فایل با فرمت png و jpg و jpeg انتخاب کنید");
				    }
			    }

				var gallery = new Gallery
			    {
				    ID = model.GalleryID,
				    Title = model.GalleryTitle,
					Caption = model.GalleryCaption,
				    PostGalleryID = model.PostGalleryPostID
			    };

			    _galleryRepository.Save(gallery);

			    return RedirectToAction("ListGallery",new { postgalleryID = model.PostGalleryID});
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError(String.Empty, e.Message);
				return RedirectToAction("ListGallery", new { postgalleryID = model.PostGalleryID });
			}
	    }

	    // GET: Admin/PostGallery/Delete
	    [HttpGet]
	    public JsonResult DeleteGallery(long galleryID, long postgalleryID)
	    {
		    var model = new GalleryViewModel
		    {
			    Galleries = _galleryRepository.GetGallerysByPostGallery(postgalleryID)
		    };

		    try
		    {
			    _galleryRepository.Delete(galleryID);

			    return Json(RedirectToAction("ListGallery", new { postgalleryID = model.PostGalleryID }), JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception e)
		    {
			    ModelState.AddModelError(String.Empty, e.Message);
			    return Json(model, JsonRequestBehavior.AllowGet);
		    }
	    }

	}
}