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
	public class CategoryRepository : ICategoryRepository
	{
		public Category GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Categories.SingleOrDefault(d => d.ID == ID);
			}
		}
		public IList<Category> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.Categories.OrderBy(d => d.Name).ToList();
			}
		}
		public void Save(Category Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Categories.SingleOrDefault(d => d.ID == Instance.ID);

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
		public void Create(Category Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.Categories.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(Category Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Categories.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.Name = Instance.Name;
					existItem.CategoryType = Instance.CategoryType;
					existItem.ParentId = Instance.ParentId;

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
					var existItem = db.Categories.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public IList<Category> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Categories
								.OrderByDescending(p => p.Name)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}
	}
}
