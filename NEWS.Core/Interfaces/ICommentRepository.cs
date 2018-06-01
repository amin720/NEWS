using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface ICommentRepository
	{
		Comment GetByID(long ID);
		IList<Comment> GetAll();
		void Save(Comment Instance);
		void Create(Comment Instance);
		void Update(Comment Instance);
		void Delete(long ID);
		IList<Comment> GetPage(int pageNumber, int pageSize);
		IEnumerable<Comment> GetCommentsByPost(long postID);
	}
}
