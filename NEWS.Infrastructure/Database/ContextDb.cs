using Microsoft.AspNet.Identity.EntityFramework;
using NEWS.Core.Entities;
using System.Data.Entity;

namespace NEWS.Infrastructure.Database
{
	public class ContextDb : IdentityDbContext<UserIdentity>
	{
		public ContextDb()
			: base("name=NEWS")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<IdentityUser>()
				.ToTable("Users", "dbo");
			modelBuilder.Entity<UserIdentity>()
				.ToTable("Users", "dbo");
			modelBuilder.Entity<IdentityRole>()
				.ToTable("Roles", "dbo");
			modelBuilder.Entity<IdentityUserRole>()
				.ToTable("UserRole", "dbo");
		}
	}
}
