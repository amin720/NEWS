using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Models
{
	public class NEWSTags
	{
		public long ID { get; set; }
		public long PostID { get; set; }
		public string Name { get; set; }


		public IList<NEWSTags> GetPosTags(long PostID)
		{
			using (var db = new NEWSEntities())
			{
				var model = db.Database
					.SqlQuery<NEWSTags>(
						"SELECT ID , PostID , Name\r\nFROM dbo.PostTag\r\nINNER JOIN dbo.Tag ON Tag.ID = PostTag.TagID\r\nWHERE PostID = " +
						PostID).ToList();
				return model;
			}
		}
	}
}