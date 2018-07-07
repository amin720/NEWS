using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Models
{
	public class NEWSAuthor
	{
		public string ID { get; set; }
		public string FullName { get; set; }
		public int? PostCount { get; set; }
		public string Avatar { get; set; }

		public IEnumerable<NEWSAuthor> Authors()
		{
			using (var db = new NEWSEntities())
			{
				var model = db.Database.SqlQuery<NEWSAuthor>(
					"SELECT AuthorID AS ID, FirstName + \' \' + LastName AS FullName, COUNT(Post.ID) AS PostCount,ImageUrl AS Avatar\r\nFROM dbo.Post\r\nLEFT OUTER JOIN dbo.Users ON Users.Id = Post.AuthorID\r\nLEFT OUTER JOIN dbo.UserRole ON UserRole.IdentityUser_Id = Users.Id\r\nGROUP BY AuthorID,FirstName + \' \' + LastName,ImageUrl").ToList();

				return model;
			}
		}
	}
}