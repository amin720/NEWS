using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Areas.Admin.ViewModels
{
	public class GalleryViewModel
	{
		public long? PostGalleryID { get; set; }
		public string PostGalleryName { get; set; }
		public long? PostGalleryPostID { get; set; }

		public long? GalleryID { get; set; }
		public string GalleryTitle { get; set; }
		public string GalleryCaption { get; set; }
		public string GalleryFileName { get; set; }
		public long? GalleryPostGalleryID { get; set; }

		public IList<PostGallery> PostGalleries { get; set; }
		public IList<Gallery> Galleries { get; set; }
	}
}