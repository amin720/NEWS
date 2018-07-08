using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;
using NEWS.Web.Models;
using NEWS.Web.Services;
using NEWS.Web.ViewModels;

namespace NEWS.Web.Controllers
{
	[RoutePrefix("")]
	public class HomeController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IPostRepository _postRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly ITagRepository _tagRepository;
		private readonly ICommentRepository _commentRepository;
		private readonly IGalleryRepository _galleryRepository;

		public HomeController()
		: this(new UserRepository(), new PostRepository()
			, new CategoryRepository(), new TagRepository()
			, new CommentRepository(), new GalleryRepository())
		{ }

		public HomeController(IUserRepository userRepository, IPostRepository postRepository
							, ICategoryRepository categoryRepository, ITagRepository tagRepository
							, ICommentRepository commentRepository, IGalleryRepository galleryRepository)
		{
			_userRepository = userRepository;
			_postRepository = postRepository;
			_categoryRepository = categoryRepository;
			_tagRepository = tagRepository;
			_commentRepository = commentRepository;
			_galleryRepository = galleryRepository;
		}

		// GET: Home
		[HttpGet]
		[Route("")]
		[AllowAnonymous]
		public ActionResult Index()
		{
			var skip = (1 - 1) * 5;

			var model = new MainPageViewModel();
			var posts = _postRepository.GetAll();
			var societyPosts = posts.Where(p => p.CategoryID == 6);
			var politicPosts = posts.Where(p => p.CategoryID == 1);
			var sportNEWS = posts.Where(p => p.CategoryID == 5);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var bestAuthors = new NEWSAuthor().Authors();
			var categories = _categoryRepository.GetAll();

			societyPosts.Shuffle();
			politicPosts.Shuffle();
			sportNEWS.Shuffle();
			posts.Shuffle();

			var newsList = (from post in posts
							where post.CategoryID != null
							select new NEWSPost
							{
								PostID = post.ID,
								Title = post.Title,
								ShortDescription = post.ShortDescription,
								ImageUrl = post.MainImageUrl,
								CreatePost = post.CreateDate,
								IsGallery = post.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = post.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							}).ToList();


			model.TrendNEWS = newsList
				.Skip(skip)
				.Take(5)
				.ToArray();

			model.RecomendedNEWS = newsList
				.Skip(5)
				.Take(5)
				.ToArray();

			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(5)
				.Take(5)
				.ToArray();

			model.PoliticNEWS = (from post in politicPosts
								 where post.CategoryID != null
								 select new NEWSPost
								 {
								 	PostID = post.ID,
								 	Title = post.Title,
								 	ShortDescription = post.ShortDescription,
								 	ImageUrl = post.MainImageUrl,
								 	CreatePost = post.CreateDate,
								 	IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								 	CategoryID = post.CategoryID,
								 	CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.SocietyNEWS = (from post in societyPosts
								 where post.CategoryID != null
								 select new NEWSPost
								 {
								 	PostID = post.ID,
								 	Title = post.Title,
								 	ShortDescription = post.ShortDescription,
								 	ImageUrl = post.MainImageUrl,
								 	CreatePost = post.CreateDate,
								 	IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								 	CategoryID = post.CategoryID,
								 	CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();
			model.SportNEWS = (from post in sportNEWS
							   where post.CategoryID != null
							   select new NEWSPost
							   {
								   PostID = post.ID,
								   Title = post.Title,
								   ShortDescription = post.ShortDescription,
								   ImageUrl = post.MainImageUrl,
								   CreatePost = post.CreateDate,
								   IsGallery = post.IsGallery,
								   CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								   CategoryID = post.CategoryID,
								   CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							   })
									.Skip(skip)
									.Take(5)
									.ToList();
			model.GalleryNEWS = (from post in galleryNEWS
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.BestAuthors = bestAuthors;
			model.CategoryNEWS = categories;

			return View(model);
		}

		// GET: Post
		[HttpGet]
		[Route("Post/{postID}")]
		[AllowAnonymous]
		public async Task<ActionResult> Post(long? ID)
		{
			var post = _postRepository.GetByID((long)ID);
			var posts = _postRepository.GetAll();
			var user = await _userRepository.GetUserByIdAsync(post.AuthorID);
			var categories = _categoryRepository.GetAll();
			var tags = new NEWSTags().GetPosTags(post.ID);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			
			var relatedPost = _postRepository.GetRelatedPost(post.ID, (long)post.CategoryID);
			var newsList = (from item in posts
							where item.CategoryID != null
							select new NEWSPost
							{
								PostID = item.ID,
								Title = item.Title,
								ShortDescription = item.ShortDescription,
								ImageUrl = item.MainImageUrl,
								CreatePost = item.CreateDate,
								IsGallery = item.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = item.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							}).ToList();



			var model = new PostViewModel
			{
				ID = post.ID,
				Title = post.Title,
				Image = post.MainImageUrl,
				ShortDescription = post.ShortDescription,
				LongDescription = post.LongDescription,
				CreateDate = (DateTime)post.CreateDate,
				IsGallery = post.IsGallery,
				AuthorID = post.AuthorID,
				AuthorName = user.FirstName + ' ' + user.LastName,
				AuthorImage = user.ImageUrl,
				CategoryID = (long)post.CategoryID,
				CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name,
				CategoryNEWS = categories,
				Tags = tags,
				CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
				Comments = _commentRepository.GetCommentsByPost((long)post.ID),
				GalleryNEWS = (from item in galleryNEWS
							   where item.CategoryID != null
							   select new NEWSPost
							   {
								   PostID = item.ID,
								   Title = item.Title,
								   ShortDescription = item.ShortDescription,
								   ImageUrl = item.MainImageUrl,
								   CreatePost = item.CreateDate,
								   IsGallery = item.IsGallery,
								   CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								   CategoryID = item.CategoryID,
								   CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							   })
					.Skip(0)
					.Take(5)
					.ToList(),
				RelatedPost = (from item in relatedPost
							   where item.CategoryID != null
							   select new NEWSPost
							   {
								   PostID = item.ID,
								   Title = item.Title,
								   ShortDescription = item.ShortDescription,
								   ImageUrl = item.MainImageUrl,
								   CreatePost = item.CreateDate,
								   IsGallery = item.IsGallery,
								   CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								   CategoryID = item.CategoryID,
								   CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							   })
					.Skip(0)
					.Take(5)
					.ToList(),
				TrendNEWS = newsList
					.Skip(0)
					.Take(5)
					.ToArray(),
				LastNEWS = newsList
					.OrderByDescending(p => p.CreatePost)
					.Skip(5)
					.Take(5)
					.ToArray(),
			};
			if (post.PostGalleryID.HasValue)
			{
				model.GalleryRelatedPost = _galleryRepository.GetGallerysByPostGallery((long)post.PostGalleryID);
			}

			return View(model.IsGallery == true ? "PostWithOutGallery" : "Post", model);
		}


		// GET: Post
		[HttpPost]
		[Route("Post")]
		[AllowAnonymous]
		public async Task<ActionResult> Post(PostViewModel model)
		{
			var post = _postRepository.GetByID(model.ID);
			var posts = _postRepository.GetAll();
			var user = await _userRepository.GetUserByIdAsync(post.AuthorID);
			var categories = _categoryRepository.GetAll();
			var tags = new NEWSTags().GetPosTags(post.ID);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var relatedPost = _postRepository.GetRelatedPost(post.ID, (long)post.CategoryID);
			var newsList = (from item in posts
							where item.CategoryID != null
							select new NEWSPost
							{
								PostID = item.ID,
								Title = item.Title,
								ShortDescription = item.ShortDescription,
								ImageUrl = item.MainImageUrl,
								CreatePost = item.CreateDate,
								IsGallery = item.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = item.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							}).ToList();



			var oldModel = new PostViewModel
			{
				ID = post.ID,
				Title = post.Title,
				Image = post.MainImageUrl,
				ShortDescription = post.ShortDescription,
				LongDescription = post.LongDescription,
				CreateDate = (DateTime)post.CreateDate,
				IsGallery = post.IsGallery,
				AuthorID = post.AuthorID,
				AuthorName = user.FirstName + ' ' + user.LastName,
				AuthorImage = user.ImageUrl,
				CategoryID = (long)post.CategoryID,
				CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name,
				CategoryNEWS = categories,
				Tags = tags,
				CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
				Comments = _commentRepository.GetCommentsByPost((long)post.ID),
				GalleryNEWS = (from item in galleryNEWS
							   where item.CategoryID != null
							   select new NEWSPost
							   {
								   PostID = item.ID,
								   Title = item.Title,
								   ShortDescription = item.ShortDescription,
								   ImageUrl = item.MainImageUrl,
								   CreatePost = item.CreateDate,
								   IsGallery = item.IsGallery,
								   CategoryID = item.CategoryID,
								   CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							   })
					.Skip(0)
					.Take(5)
					.ToList(),
				RelatedPost = (from item in relatedPost
							   where item.CategoryID != null
							   select new NEWSPost
							   {
								   PostID = item.ID,
								   Title = item.Title,
								   ShortDescription = item.ShortDescription,
								   ImageUrl = item.MainImageUrl,
								   CreatePost = item.CreateDate,
								   IsGallery = item.IsGallery,
								   CategoryID = item.CategoryID,
								   CategoryName = _categoryRepository.GetByID((long)item.CategoryID).Name
							   })
					.Skip(0)
					.Take(5)
					.ToList(),
				TrendNEWS = newsList
					.Skip(0)
					.Take(5)
					.ToArray(),
				LastNEWS = newsList
					.OrderByDescending(p => p.CreatePost)
					.Skip(5)
					.Take(5)
					.ToArray(),
			};

			if (post.PostGalleryID.HasValue)
			{
				model.GalleryRelatedPost = _galleryRepository.GetGallerysByPostGallery((long)post.PostGalleryID);
			}

			try
			{
				//oldModel.CommentID = model.CommentID;
				//oldModel.CommentName = model.CommentName;
				//oldModel.CommentEmail = model.CommentEmail;
				//oldModel.CommentWebsite = model.CommentWebsite;
				//oldModel.CommentReplyID = model.CommentReplyID;


				var comment = new Comment
				{
					ID = (model.CommentID != 0 ? model.CommentID : 0),
					PersonName = model.CommentName,
					Email = model.CommentEmail,
					Website = model.CommentWebsite,
					PostID = (long)model.ID,
					ReplyID = (model.CommentReplyID != 0 ? model.CommentReplyID : 0),
					Desciption = model.CommentContent,
					CreateDate = DateTime.Now
				};

				_commentRepository.Save(comment);




				return View(model.IsGallery == true ? "PostWithOutGallery" : "Post", oldModel);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(String.Empty, e.Message);
				return View(model.IsGallery == true ? "PostWithOutGallery" : "Post", oldModel);
			}

			
		}


		// GET: Category
		[HttpGet]
		[Route("Category")]
		[AllowAnonymous]
		public ActionResult Category(long? ID)
		{
			var skip = (1 - 1) * 5;

			var model = new MainPageViewModel();
			var otherPosts = _postRepository.GetAll();
			var posts = _postRepository.GetPageByCategory(1, 20, (long)ID);
			var societyPosts = posts.Where(p => p.CategoryID == 6);
			var politicPosts = posts.Where(p => p.CategoryID == 1);
			var sportNEWS = posts.Where(p => p.CategoryID == 5);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var bestAuthors = new NEWSAuthor().Authors();
			var categories = _categoryRepository.GetAll();

			societyPosts.Shuffle();
			politicPosts.Shuffle();
			sportNEWS.Shuffle();
			posts.Shuffle();
			otherPosts.Shuffle();


			var newsList = (from post in posts
							where post.CategoryID != null
							select new NEWSPost
							{
								PostID = post.ID,
								Title = post.Title,
								ShortDescription = post.ShortDescription,
								ImageUrl = post.MainImageUrl,
								CreatePost = post.CreateDate,
								IsGallery = post.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = post.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							}).ToList();

			var othernewsList = (from post in otherPosts
				where post.CategoryID != null
				select new NEWSPost
				{
					PostID = post.ID,
					Title = post.Title,
					ShortDescription = post.ShortDescription,
					ImageUrl = post.MainImageUrl,
					CreatePost = post.CreateDate,
					IsGallery = post.IsGallery,
					CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
					CategoryID = post.CategoryID,
					CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
				}).ToList();

			model.TrendNEWS = othernewsList
				.Skip(skip)
				.Take(5)
				.ToArray();

			model.RecomendedNEWS = othernewsList
				.Skip(5)
				.Take(5)
				.ToArray();

			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(0)
				.Take(20)
				.ToArray();

			
			model.GalleryNEWS = (from post in galleryNEWS
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.BestAuthors = bestAuthors;
			model.CategoryNEWS = categories;

			model.CategoryName = _categoryRepository.GetByID((long)ID).Name;

			return View(model);
		}

		// POST: Author
		[HttpGet]
		[Route("Author")]
		[AllowAnonymous]
		public async Task<ActionResult> Author(string ID)
		{
			var skip = (1 - 1) * 5;

			var model = new MainPageViewModel();
			var otherPosts = _postRepository.GetAll();
			var posts = _postRepository.GetPostsByAuthor(ID);
			var societyPosts = posts.Where(p => p.CategoryID == 6);
			var politicPosts = posts.Where(p => p.CategoryID == 1);
			var sportNEWS = posts.Where(p => p.CategoryID == 5);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var bestAuthors = new NEWSAuthor().Authors();
			var categories = _categoryRepository.GetAll();

			societyPosts.Shuffle();
			politicPosts.Shuffle();
			sportNEWS.Shuffle();
			posts.Shuffle();
			otherPosts.Shuffle();


			var newsList = (from post in posts
							where post.CategoryID != null
							select new NEWSPost
							{
								PostID = post.ID,
								Title = post.Title,
								ShortDescription = post.ShortDescription,
								ImageUrl = post.MainImageUrl,
								CreatePost = post.CreateDate,
								IsGallery = post.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = post.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							}).ToList();

			var othernewsList = (from post in otherPosts
								where post.CategoryID != null
								select new NEWSPost
								{
									PostID = post.ID,
									Title = post.Title,
									ShortDescription = post.ShortDescription,
									ImageUrl = post.MainImageUrl,
									CreatePost = post.CreateDate,
									IsGallery = post.IsGallery,
									CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									CategoryID = post.CategoryID,
									CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								}).ToList();

			model.TrendNEWS = othernewsList
				.Skip(skip)
				.Take(5)
				.ToArray();

			model.RecomendedNEWS = othernewsList
				.Skip(5)
				.Take(5)
				.ToArray();

			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(0)
				.Take(20)
				.ToArray();


			model.GalleryNEWS = (from post in galleryNEWS
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.BestAuthors = bestAuthors;
			model.CategoryNEWS = categories;

			var user = await _userRepository.GetUserByIdAsync(ID);
			model.CategoryName = user.FirstName + ' ' + user.LastName;

			return View(model);
		}

		// GET: Tag
		[HttpGet]
		[Route("Tag")]
		[AllowAnonymous]
		public ActionResult Tag(long? ID)
		{

			var skip = (1 - 1) * 5;

			var model = new MainPageViewModel();
			var otherPosts = _postRepository.GetAll();
			var posts = _postRepository.GetPostsByTag((long)ID);
			var societyPosts = posts.Where(p => p.CategoryID == 6);
			var politicPosts = posts.Where(p => p.CategoryID == 1);
			var sportNEWS = posts.Where(p => p.CategoryID == 5);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var bestAuthors = new NEWSAuthor().Authors();
			var categories = _categoryRepository.GetAll();

			societyPosts.Shuffle();
			politicPosts.Shuffle();
			sportNEWS.Shuffle();
			posts.Shuffle();
			otherPosts.Shuffle();

			var newsList = (from post in posts
							where post.CategoryID != null
							select new NEWSPost
							{
								PostID = post.ID,
								Title = post.Title,
								ShortDescription = post.ShortDescription,
								ImageUrl = post.MainImageUrl,
								CreatePost = post.CreateDate,
								IsGallery = post.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = post.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							}).ToList();

			var othernewsList = (from post in otherPosts
								 where post.CategoryID != null
									select new NEWSPost
									{
										PostID = post.ID,
										Title = post.Title,
										ShortDescription = post.ShortDescription,
										ImageUrl = post.MainImageUrl,
										CreatePost = post.CreateDate,
										IsGallery = post.IsGallery,
										CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
										CategoryID = post.CategoryID,
										CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
									}).ToList();

			model.TrendNEWS = othernewsList
				.Skip(skip)
				.Take(5)
				.ToArray();

			model.RecomendedNEWS = othernewsList
				.Skip(5)
				.Take(5)
				.ToArray();

			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(0)
				.Take(20)
				.ToArray();


			model.GalleryNEWS = (from post in galleryNEWS
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.BestAuthors = bestAuthors;
			model.CategoryNEWS = categories;

			
			model.CategoryName = _tagRepository.GetByID((long)ID).Name;

			return View(model);
		}


		// GET: Search
		[HttpGet]
		[Route("Search")]
		[AllowAnonymous]
		public ActionResult Search(string search)
		{

			var skip = (1 - 1) * 5;

			var model = new MainPageViewModel();
			var otherPosts = _postRepository.GetAll();
			var posts = SearchPost.Search(search);
			var societyPosts = posts.Where(p => p.CategoryID == 6);
			var politicPosts = posts.Where(p => p.CategoryID == 1);
			var sportNEWS = posts.Where(p => p.CategoryID == 5);
			var galleryNEWS = _postRepository.GetPostsHasGallery();
			var bestAuthors = new NEWSAuthor().Authors();
			var categories = _categoryRepository.GetAll();

			societyPosts.Shuffle();
			politicPosts.Shuffle();
			sportNEWS.Shuffle();
			posts.Shuffle();
			otherPosts.Shuffle();

			var newsList = (from post in posts
							where post.CategoryID != null
							select new NEWSPost
							{
								PostID = post.ID,
								Title = post.Title,
								ShortDescription = post.ShortDescription,
								ImageUrl = post.MainImageUrl,
								CreatePost = post.CreateDate,
								IsGallery = post.IsGallery,
								CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
								CategoryID = post.CategoryID,
								CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
							}).ToList();

			var othernewsList = (from post in otherPosts
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 }).ToList();

			model.TrendNEWS = othernewsList
				.Skip(skip)
				.Take(5)
				.ToArray();

			model.RecomendedNEWS = othernewsList
				.Skip(5)
				.Take(5)
				.ToArray();

			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(0)
				.Take(20)
				.ToArray();


			model.GalleryNEWS = (from post in galleryNEWS
								 where post.CategoryID != null
								 select new NEWSPost
								 {
									 PostID = post.ID,
									 Title = post.Title,
									 ShortDescription = post.ShortDescription,
									 ImageUrl = post.MainImageUrl,
									 CreatePost = post.CreateDate,
									 IsGallery = post.IsGallery,
									 CommentCount = _commentRepository.GetCommentsByPost((long)post.ID).Count(),
									 CategoryID = post.CategoryID,
									 CategoryName = _categoryRepository.GetByID((long)post.CategoryID).Name
								 })
									.Skip(skip)
									.Take(5)
									.ToList();

			model.BestAuthors = bestAuthors;
			model.CategoryNEWS = categories;


			model.CategoryName = search;

			return View(model);
		}

		#region Method

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_userRepository.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _userRepository.GetUserByNameAsync(User.Identity.Name);
		}

		#endregion
	}
}