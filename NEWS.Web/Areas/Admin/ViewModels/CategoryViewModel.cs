using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;

namespace NEWS.Web.Areas.Admin.ViewModels
{
	public class CategoryViewModel
	{
		public long ID { get; set; }
		public long CategoryID { get; set; }
		public string Name { get; set; }
		public long? ParentID { get; set; }
		public string ParentName { get; set; }
	}
}