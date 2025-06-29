using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Services;

namespace SocialApp.Controllers;

public class FavoritesController : Controller
{
	private readonly IPostsService _postsService;

	public FavoritesController ( IPostsService postsService )
	{
		_postsService = postsService;
	}

	public async Task<IActionResult> Index ()
	{
		var loggedInUserId = 1;
		var favoritedPosts = await _postsService.GetAllFavoritedPostsAsync(loggedInUserId);

		return View(favoritedPosts);
	}
}
