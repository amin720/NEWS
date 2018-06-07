using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface IGalleryRepository
	{
		Gallery GetByID(long ID);
		IList<Gallery> GetAll();
		void Save(Gallery Instance);
		void Create(Gallery Instance);
		void Update(Gallery Instance);
		void Delete(long ID);
		IList<Gallery> GetPage(int pageNumber, int pageSize);
		IList<Gallery> GetGallerysByPostGallery(long? postGalleryID);
		void SetGalleryToPost(long postGalleryID, IList<long> galleryID);
	}
}
