using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NEWS.Infrastructure.Repository
{
	public class TagRepository : ITagRepository
	{
		public Tag GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Tags.SingleOrDefault(d => d.ID == ID);
			}
		}
		public IList<Tag> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.Tags.OrderBy(d => d.Name).ToList();
			}
		}
		public void Save(Tag Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Tags.SingleOrDefault(d => d.ID == Instance.ID);

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
		public void Create(Tag Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.Tags.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(Tag Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Tags.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.Name = Instance.Name;
					existItem.Posts = Instance.Posts;
					
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
					var existItem = db.Tags.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}

					db.Tags.Remove(existItem);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public IList<Tag> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Tags
								.OrderByDescending(p => p.Name)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}

		//public IEnumerable<Tag> GetTagsByPost(long postID)
		//{
		//	using (var db = new NEWSEntities())
		//	{
				
		//	}
		//}
	}
}
