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
		public IList<NEWSPost> PopularNEWS { get; set; }
		public IList<NEWSPost> BestAuthors { get; set; }
		public IList<NEWSPost> PoliticNEWS { get; set; }
		public IList<NEWSPost> SocietyNEWS { get; set; }
		public IList<NEWSPost> SportNEWS { get; set; }
		public IList<NEWSPost> CultureNEWS { get; set; }
		public IList<NEWSPost> ScienceNEWS { get; set; }
	}
}