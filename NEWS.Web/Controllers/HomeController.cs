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

	    public HomeController()
		:this(new UserRepository(), new PostRepository(),new CategoryRepository(),new TagRepository())
	    {}

	    public HomeController(IUserRepository userRepository,IPostRepository postRepository,ICategoryRepository categoryRepository,ITagRepository tagRepository)
	    {
		    _userRepository = userRepository;
		    _postRepository = postRepository;
		    _categoryRepository = categoryRepository;
		    _tagRepository = tagRepository;
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
			posts.Shuffle();

			var newsList = new List<NEWSPost>();

			foreach (var post in posts)
			{
				if (post.CategoryID != null)
					newsList.Add(new NEWSPost
					{
						PostID = post.ID,
						Title = post.Title,
						ShortDescription = post.ShortDescription,
						ImageUrl = post.MainImageUrl,
						CreatePost = post.CreateDate,
						IsGallery = post.IsGallery,
						CategoryID = post.CategoryID,
						CategoryName = _categoryRepository.GetByID((long) post.CategoryID).Name
					});
			}

			


			model.TrendNEWS = newsList
				.Skip(skip)
				.Take(5)
				.ToArray();
			model.LastNEWS = newsList
				.OrderByDescending(p => p.CreatePost)
				.Skip(skip)
				.Take(5)
				.ToArray();

			


			return View(model);
        }

	    // GET: Post
	    [HttpGet]
	    [Route("Post")]
	    [AllowAnonymous]
	    public ActionResult Post()
	    {

		    return View();
	    }

	    // GET: Category
	    [HttpGet]
	    [Route("Category")]
	    [AllowAnonymous]
	    public ActionResult Category()
	    {

		    return View();
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