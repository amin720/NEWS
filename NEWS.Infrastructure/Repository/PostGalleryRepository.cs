using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NEWS.Infrastructure.Repository
{
	public class PostGalleryRepository: IPostGalleryRepository
	{
		public PostGallery GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.PostGalleries.SingleOrDefault(d => d.ID == ID);
			}
		}
		public IList<PostGallery> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.PostGalleries.OrderBy(d => d.ID).ToList();
			}
		}
		public void Save(PostGallery Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.PostGalleries.SingleOrDefault(d => d.ID == Instance.ID);

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
		public void Create(PostGallery Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.PostGalleries.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(PostGallery Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.PostGalleries.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.ID = Instance.ID;
					existItem.Name = Instance.Name;

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
					var existItem = db.PostGalleries.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}

					db.PostGalleries.Remove(existItem);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}

		
		public IList<PostGallery> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.PostGalleries
								.OrderByDescending(p => p.Name)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}
	}
}
