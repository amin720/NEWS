using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using NEWS.Core.Entities;

namespace NEWS.Web.Areas.Admin.ViewModels
{
	public class UserViewModel
	{
		[Required]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }

		[Display(Name = "Display Name")]
		public string DisplayName { get; set; }

		[Display(Name = "Current Password")]
		public string CurrentPassword { get; set; }

		[Display(Name = "New Password")]
		[System.ComponentModel.DataAnnotations.Compare("ConfirmPassword", ErrorMessage = "کلمه عبور جدید با کلمه عبور وارد شده تطابق ندارد !!!")]
		public string NewPassword { get; set; }

		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ImageUrl { get; set; }
		[DefaultValue(false)]
		public bool Actived { get; set; }

		[Display(Name = "Role")]
		public string SelectedRole { get; set; }

		private readonly List<string> _roles = new List<string>();
		public IEnumerable<SelectListItem> Roles
		{
			get { return new SelectList(_roles); }
		}

		public IEnumerable<UserIdentity> Users { get; set; }

		public void LoadUserRoles(IEnumerable<IdentityRole> roles)
		{
			_roles.AddRange(roles.Select(r => r.Name));
		}


	}
}