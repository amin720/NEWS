using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Areas.Admin.ViewModels
{
	public class PostViewModel
	{
		public long? ID { get; set; }
		public string UserID { get; set; }
		public long? CategoryID { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string LongDescription { get; set; }
		public string MainImageUrl { get; set; }
		public DateTime CreateDate { get; set; }
		public bool? Actived { get; set; }
		public bool? IsGallery { get; set; }
		public int ViewerCount { get; set; }
		public string SelectedTags { get; set; }
		public string CategoryName { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public long? PostGalleryID { get; set; }


		public IList<Category> Categories { get; set; }
		public IList<PostGallery> PostGalleries{ get; set; }
	}
}