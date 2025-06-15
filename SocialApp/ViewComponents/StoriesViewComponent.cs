
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;

namespace SocialApp.ViewComponents;

public class StoriesViewComponent : ViewComponent
{

	private readonly AppDbContext _dbContext;

	public StoriesViewComponent ( AppDbContext dbContext )
	{
		_dbContext = dbContext;
	}

	public async Task<IViewComponentResult> InvokeAsync ()
	{
		var stories = await _dbContext.Stories
			.Where(s => s.CreatedAt >= DateTime.UtcNow.AddDays(-1)) // Filter stories created in the last 24 hours
			.Where(s => !s.IsDeleted) // Filter out deleted stories
			.Include(s => s.User) // Include the User entity to get user details for each story
			.ToListAsync();

		return View(stories);
	}
}
