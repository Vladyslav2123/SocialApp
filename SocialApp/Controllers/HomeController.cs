using Microsoft.AspNetCore.Mvc;
using SocialApp.Data.Helpers.Enums;
using SocialApp.Data.Models;
using SocialApp.Data.Services;
using SocialApp.ViewModels.Home;

namespace SocialApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IPostsService _postsService = null!;
		private readonly IHashtagsService _hashtagsService = null!;
		private readonly IFilesService _filesService = null!;

		public HomeController (
			ILogger<HomeController> logger,
			IPostsService postsService,
			IHashtagsService hashtagsService,
			IFilesService filesService )
		{
			_logger = logger;
			_postsService = postsService;
			_hashtagsService = hashtagsService;
			_filesService = filesService;
		}

		public async Task<IActionResult> Index ()
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			var allPosts = await _postsService.GetAllPostsAsync(userId);
			return View(allPosts);
		}

		[HttpPost]
		public async Task<IActionResult> CreatePost ( PostVM post )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			var imageUploadPath = await _filesService.UploadImageAsync(post.Image, ImageFileType.PostImage);

			var newPost = new Post
			{
				Content = post.Content,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow,
				ImageUrl = imageUploadPath,
				NrOfReports = 0,
				UserId = userId,
			};

			await _postsService.CreatePostAsync(newPost);
			await _hashtagsService.ProcessHashtagsForNewPostAsync(post.Content); // Add hashtags if any

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostLike ( PostLikeVM postLikeVM )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			await _postsService.TogglePostLikeAsynk(postLikeVM.PostId, userId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddPostComment ( PostCommentVM postCommentVM )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			await _postsService.AddPostCommentAsync(new Comment
			{
				Content = postCommentVM.Content,
				UserId = userId,
				PostId = postCommentVM.PostId,
				CreateAt = DateTime.UtcNow,
				UpdateAt = DateTime.UtcNow
			});

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> RemovePostComment ( RemoveCommentVM removeCommentVM )
		{
			await _postsService.RemovePostCommentAsync(removeCommentVM.CommentId);
			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostFavorite ( PostFavoriteVM postFavoriteVM )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			await _postsService.TogglePostFavoriteAsync(postFavoriteVM.PostId, userId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> TogglePostVisibility ( PostVisibilityVM postVisibilityVM )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			await _postsService.TogglePostVisibilityAsync(postVisibilityVM.PostId, userId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> AddPostReport ( PostReportVM postReportVM )
		{
			int userId = 1; // This should be replaced with the actual user ID from the authenticated user context
			await _postsService.ReportPostAsync(postReportVM.PostId, userId);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> PostRemove ( RemovePostVM removePostVM )
		{
			var postRemove = await _postsService.RemovePostAsync(removePostVM.PostId);
			await _hashtagsService.ProcessHashtagsForRemovedPostAsync(postRemove.Content); // Remove hashtags if any
			return RedirectToAction("Index");
		}
	}
}