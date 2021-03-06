﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;

namespace NEWS.Infrastructure.Repository
{
	public class GalleryRepository: IGalleryRepository
	{
		public Gallery GetByID(long ID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Galleries.SingleOrDefault(d => d.ID == ID);
			}
		}
		public IList<Gallery> GetAll()
		{
			using (var db = new NEWSEntities())
			{
				return db.Galleries.OrderBy(d => d.PostGalleryID).ToList();
			}
		}
		public void Save(Gallery Instance)
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
		public void Create(Gallery Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					db.Galleries.Add(Instance);

					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public void Update(Gallery Instance)
		{
			using (var db = new NEWSEntities())
			{
				try
				{
					var existItem = db.Galleries.SingleOrDefault(d => d.ID == Instance.ID);

					existItem.Title = Instance.Title;
					existItem.Caption = Instance.Caption;
					existItem.FileName = Instance.FileName;
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
					var existItem = db.Galleries.SingleOrDefault(d => d.ID == ID);

					if (existItem == null)
					{
						throw new AggregateException("مورد یافت نشد.");
					}

					db.Galleries.Remove(existItem);
					db.SaveChanges();
				}
				catch (Exception ex)
				{
					throw new AggregateException(ex);
				}
			}
		}
		public IList<Gallery> GetPage(int pageNumber, int pageSize)
		{
			using (var db = new NEWSEntities())
			{
				var skip = (pageNumber - 1) * pageSize;

				return db.Galleries
								.OrderByDescending(p => p.Title)
								.Skip(skip)
								.Take(pageSize)
								.ToArray();
			}
		}
		public IList<Gallery> GetGallerysByPostGallery(long? postGalleryID)
		{
			using (var db = new NEWSEntities())
			{
				return db.Galleries
							.Include("PostGallery")
							.Where(p => p.PostGalleryID == postGalleryID)
							.OrderByDescending(post => post.PostGalleryID)
							.ToArray();
			}
		}

		public void SetGalleryToPost(long postGalleryID, IList<long> galleryID)
		{
			using (var db = new NEWSEntities())
			{
				foreach (var item in galleryID)
				{
					var gallery = db.Galleries.SingleOrDefault(g => g.ID == item);
					gallery.PostGalleryID = postGalleryID;
					db.SaveChanges();
				}
			}
		}
	}
}
