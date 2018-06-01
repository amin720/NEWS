using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface ICategoryRepository
	{
		Category GetByID(long ID);
		IList<Category> GetAll();
		void Save(Category Instance);
		void Create(Category Instance);
		void Update(Category Instance);
		void Delete(long ID);
		IList<Category> GetPage(int pageNumber, int pageSize);

	}
}
