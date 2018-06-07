using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface IPostGalleryRepository
	{
		PostGallery GetByID(long ID);
		IList<PostGallery> GetAll();
		void Save(PostGallery Instance);
		void Create(PostGallery Instance);
		void Update(PostGallery Instance);
		void Delete(long ID);
		IList<PostGallery> GetPage(int pageNumber, int pageSize);
	}
}
