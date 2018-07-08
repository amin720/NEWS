using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Services
{
	public static class SearchPost
	{
		public static IEnumerable<Post> Search(string searchItem)
		{
			using (var db = new NEWSEntities())
			{
				var searched = db.Posts.Where(t => t.Title.Contains(searchItem)).ToList();

				return searched;
			}
		}
	}
}