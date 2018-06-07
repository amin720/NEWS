using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;

namespace NEWS.Core.Interfaces
{
	public interface IPostRepository
	{
		Post GetByID(long ID);
		Post GetByTitleDateAuthor(string Title, DateTime CreateDate, string AuthorID);
		IList<Post> GetAll();
		void Save(Post Instance);
		void Create(Post Instance);
		void Update(Post Instance);
		void Delete(long ID);
		IList<string> GetTagsPost(long postID);
		IList<Post> GetPage(int pageNumber, int pageSize);
		IEnumerable<Post> GetPostsByAuthor(string authorId);
		IList<Post> GetPageByCategory(int pageNumber, int pageSize, long categoryID);
		IEnumerable<Post> GetPublishedPosts();
		Post GetPostByPostGallery(long postgalleryID);
		void SetTagToPost(long postID, long tagID);
	}
}
