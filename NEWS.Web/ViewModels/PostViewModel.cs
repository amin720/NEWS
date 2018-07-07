using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;
using NEWS.Web.Models;

namespace NEWS.Web.ViewModels
{
	public class PostViewModel
	{
		public long ID { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public string LongDescription { get; set; }
		public string Image { get; set; }
		public DateTime CreateDate { get; set; }
		public bool? IsGallery { get; set; }
		public string AuthorID { get; set; }
		public string AuthorName { get; set; }
		public string AuthorImage { get; set; }
		public int Viewer { get; set; }
		public int CommentCount { get; set; }


		public int CommentID { get; set; }
		public string CommentName { get; set; }
		public string CommentEmail { get; set; }
		public string CommentWebsite { get; set; }
		public string CommentContent { get; set; }
		public int CommentReplyID { get; set; }


		public long CategoryID { get; set; }
		public string CategoryName { get; set; }
		public IList<NEWSTags> Tags { get; set; }
		public IList<Category> CategoryNEWS { get; set; }
		public IList<NEWSPost> TrendNEWS { get; set; }
		public IList<NEWSPost> LastNEWS { get; set; }
		public IList<NEWSPost> GalleryNEWS { get; set; }
		public IList<NEWSPost> RelatedPost { get; set; }
		public IList<Comment> Comments { get; set; }
		public IList<Gallery> GalleryRelatedPost { get; set; }
	}
}