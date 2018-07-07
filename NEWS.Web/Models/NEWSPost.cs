using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NEWS.Web.Models
{
	public class NEWSPost
	{
		public long PostID { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string CategoryName { get; set; }
		public string ImageUrl { get; set; }
		public DateTime? CreatePost { get; set; }
		public bool? IsGallery { get; set; }
		public long? CategoryID { get; set; }
		public int? CommentCount { get; set; }

		public string AuthorName { get; set; }
		public long? AuthorPostCount { get; set; }
	}
}