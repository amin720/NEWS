using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace NEWS.Core.Entities
{
	public class UserIdentity : IdentityUser
	{
		public string DisplayName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Actived { get; set; }
		public string ImageUrl { get; set; }
		public DateTime DateCreated { get; set; }

	}
}
