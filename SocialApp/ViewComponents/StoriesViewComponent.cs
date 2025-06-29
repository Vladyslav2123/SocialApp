
using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Services;

namespace SocialApp.ViewComponents;

public class StoriesViewComponent : ViewComponent
{

	private readonly IStoriesService _storiesServices;

	public StoriesViewComponent ( IStoriesService storiesService )
	{
		_storiesServices = storiesService;
	}

	public async Task<IViewComponentResult> InvokeAsync ()
	{
		var stories = await _storiesServices.GetAllStoriesAsync();
		return View(stories);
	}
}
