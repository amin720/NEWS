using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;

namespace NEWS.Infrastructure.Repository
{
	public class PostRepository: IPostRepository
	{
		public Post GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Posts.SingleOrDefault(d => d.ID == ID);
			}
		}

		public Post GetByTitleDateAuthor(string Title, DateTime CreateDate, string AuthorID)
		{
			using (var db =new NEWSEntities())
			{
				return db.Posts.SingleOrDefault(p => p.Title == Title
				                                  && p.CreateDate == CreateDate
				                                  && p.AuthorID == AuthorID);
			}
		}
		public IList<Post> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.Posts.OrderBy(d => d.CreateDate).ToList();
			}
		}
		public void Save(Post Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Posts.SingleOrDefault(d => d.ID == Instance.ID);

					if (existItem != null)
					{
						Update(Instance);
					}
					else
					{
						Create(Instance);
					}
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Create(Post Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.Posts.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(Post Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Posts.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.Title = Instance.Title;
					existItem.ShortDescription = Instance.ShortDescription;
					existItem.LongDescription = Instance.LongDescription;
					existItem.MainImageUrl = Instance.MainImageUrl;
					existItem.CreateDate = Instance.CreateDate;
					existItem.Actived = Instance.Actived;
					existItem.ViewerCount = Instance.ViewerCount;
					existItem.IsGallery = Instance.IsGallery;
					existItem.AuthorID = Instance.AuthorID;
					existItem.CategoryID = Instance.CategoryID;
					existItem.PostGalleryID = Instance.PostGalleryID;

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Delete(long ID)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Posts.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}

					db.Posts.Remove(existItem);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public IList<string> GetTagsPost(long postID)
		{
			using (var db = new NEWSEntities())
			{
				var postTag = db.Posts.SqlQuery("SELECT *\r\nFROM dbo.PostTag\r\nWHERE PostID = {0}",postID);

				return null;
			}
		}
		public IList<Post> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Posts
								.OrderByDescending(p => p.Title)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}
		public IList<Post> GetPageByCategory(int pageNumber, int pageSize,long categoryID)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Posts
					.Include("Category")
					.Where(c => c.CategoryID == categoryID)
					.OrderByDescending(p => p.Title)
					.Skip(skip)
					.Take(pageSize)
					.ToArray();
			}
		}
		public IEnumerable<Post> GetPostsByAuthor(string authorId)
		{
			using (var db = new NEWSEntities())
			{
				return db.Posts
							.Include("Users")
							.Where(p => p.AuthorID == authorId)
							.OrderByDescending(post => post.CreateDate)
							.ToArray();
			}
		}
		public IEnumerable<Post> GetPublishedPosts()
		{
			using (var db = new NEWSEntities())
			{
				return db.Posts
								.Include("Users")
								.Where(p => p.CreateDate < DateTime.Now)
								.OrderByDescending(p => p.CreateDate)
								.ToArray();
			}
		}
		public Post GetPostByPostGallery(long postgalleryID)
		{
			using (var db = new NEWSEntities())
			{
				var post = db.Posts.SingleOrDefault(pg => pg.PostGalleryID== postgalleryID);

				return post;
			}
		}
		public void SetTagToPost(long postID,long tagID)
		{
			using (var db = new NEWSEntities())
			{
				var post = db.Posts.SingleOrDefault(p => p.ID == postID);
				var tag = db.Tags.SingleOrDefault(t => t.ID == tagID);

				post.Tags.Add(tag);
				db.SaveChanges();
			}
		}
	}
}
