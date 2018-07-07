using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;

namespace NEWS.Infrastructure.Repository
{
	public class CommentRepository: ICommentRepository
	{
		public Comment GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Comments.SingleOrDefault(d => d.ID == ID);
			}
		}
		public IList<Comment> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.Comments.OrderBy(d => d.PostID).ToList();
			}
		}
		public void Save(Comment Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Comments.SingleOrDefault(d => d.ID == Instance.ID);

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
		public void Create(Comment Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.Comments.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(Comment Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Comments.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.Title = Instance.Title;
					existItem.PersonName = Instance.PersonName;
					existItem.Email = Instance.Email;
					existItem.Title = Instance.Title;
					existItem.Desciption = Instance.Desciption;
					existItem.Website = Instance.Website;
					existItem.PostID = Instance.PostID;
					
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
					var existItem = db.Comments.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}

					db.Comments.Remove(existItem);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public IList<Comment> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Comments
								.OrderByDescending(p => p.Title)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}
		public IList<Comment> GetCommentsByPost(long postID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Comments
							.Include("Post")
							.Where(p => p.PostID == postID)
							.OrderByDescending(post => post.PostID)
							.ToArray();
			}
		}
	}
}
