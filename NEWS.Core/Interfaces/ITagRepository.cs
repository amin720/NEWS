using NEWS.Core.Entities;
using System.Collections.Generic;

namespace NEWS.Core.Interfaces
{
	public interface ITagRepository
	{
		Tag GetByID(long ID);
		Tag GetByName(string Name);
		IList<Tag> GetAll();
		void Save(Tag Instance);
		void Create(Tag Instance);
		void Update(Tag Instance);
		void Delete(long ID);
		IList<Tag> GetPage(int pageNumber, int pageSize);
	}
}
