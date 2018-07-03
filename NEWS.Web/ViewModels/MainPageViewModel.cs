using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NEWS.Core.Entities;
using NEWS.Web.Models;

namespace NEWS.Web.ViewModels
{
	public class MainPageViewModel
	{
		public IList<NEWSPost> TrendNEWS { get; set; }
		public IList<NEWSPost> LastNEWS { get; set; }
	}
}