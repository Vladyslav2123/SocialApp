using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;

namespace SocialApp.ViewComponents;

public class HashtagsViewComponent : ViewComponent
{
	private readonly AppDbContext _context;

	public HashtagsViewComponent ( AppDbContext dbContext )
	{
		_context = dbContext;
	}

	public async Task<IViewComponentResult> InvokeAsync ()
	{
		var oneWeekAgo = DateTime.UtcNow.AddDays(-7);

		var hashtags = await _context.Hashtags
			.Where(h => h.CreatedAt >= oneWeekAgo)
			.OrderByDescending(h => h.Count) // Order by usage count
			.Take(10) // Limit to top 10 hashtags
			.ToListAsync();

		return View(hashtags);
	}
}
