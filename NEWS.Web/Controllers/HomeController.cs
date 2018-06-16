using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NEWS.Core.Entities;
using NEWS.Core.Interfaces;
using NEWS.Infrastructure.Repository;

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
        public ActionResult Index()
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