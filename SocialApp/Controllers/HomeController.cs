using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Data;
using SocialApp.Data.Models;
using SocialApp.ViewModels.Home;

namespace SocialApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly AppDbContext _dbContext;

		public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<IActionResult> Index()
		{
			_logger.LogInformation("Index action called in HomeController.");

			var allPosts = await _dbContext.Posts
				.Include(p => p.User) // Include the User entity to get user details for each post
				.OrderByDescending(p => p.CreatedAt) // Order posts by creation date, newest first
				.ToListAsync();

			return View(allPosts);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePost(PostVM post)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var newPost = new Post
			{
				Content = post.Content,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				ImageUrl = null, // Assuming no image is uploaded; handle image upload separately if needed
				NrOfReports = 0, // Default value for new posts
				UserId = userId,
			};

			// Handle image upload if provided
			if (post.Image != null && post.Image.Length > 0)
			{
				string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
				if (post.Image.ContentType.Contains("image"))
				{
					string rootPathImage = Path.Combine(rootPath, "images");
					Directory.CreateDirectory(rootPathImage); // Ensure the directory exists

					string fileName = $"{Guid.NewGuid()}_{post.Image.FileName}";
					string filePath = Path.Combine(rootPathImage, fileName);

					using (var fileStream = new FileStream(filePath, FileMode.Create))
						await post.Image.CopyToAsync(fileStream);

					newPost.ImageUrl = $"/images/{fileName}"; // Set the image URL for the post
				}
			}

			_dbContext.Posts.Add(newPost);
			await _dbContext.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostLike(PostLikeVM postLikeVM)
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context

			var existingLike = await _dbContext.Likes
				.FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postLikeVM.PostId);

			if (existingLike != null)
			{
				// User has already liked the post, so we remove the like
				_dbContext.Likes.Remove(existingLike);
				await _dbContext.SaveChangesAsync();
			}
			else
			{
				// User has not liked the post, so we add a new like
				var newLike = new Like
				{
					UserId = userId,
					PostId = postLikeVM.PostId
				};
				await _dbContext.Likes.AddAsync(newLike);
				await _dbContext.SaveChangesAsync();
			}

			return RedirectToAction("Index");
		}
	}
}
