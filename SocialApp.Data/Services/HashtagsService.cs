using Microsoft.EntityFrameworkCore;
using SocialApp.Data.Helpers;
using SocialApp.Data.Models;

namespace SocialApp.Data.Services;

public class HashtagsService : IHashtagsService
{
	private readonly AppDbContext _context;

	public HashtagsService ( AppDbContext context )
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
	}

	public async Task ProcessHashtagsForNewPostAsync ( string content )
	{
		var postHashtags = HashtagHelper.GetHashtags(content);
		foreach (var hashTag in postHashtags)
		{
			var hashtag = await _context.Hashtags
				.FirstOrDefaultAsync(h => h.Name == hashTag);
			if (hashtag == null)
			{
				var newHashtag = new Hashtag
				{
					Name = hashTag,
					Count = 1,
					CreatedAt = DateTime.UtcNow,
					UpdatedAt = DateTime.UtcNow
				};
				await _context.Hashtags.AddAsync(newHashtag);
				await _context.SaveChangesAsync();
			}
			else
			{
				hashtag.Count++;
				hashtag.UpdatedAt = DateTime.UtcNow;
				_context.Hashtags.Update(hashtag);
				await _context.SaveChangesAsync();
			}
		}
	}

	public async Task ProcessHashtagsForRemovedPostAsync ( string content )
	{
		var postHashtags = HashtagHelper.GetHashtags(content);
		foreach (var hashtag in postHashtags)
		{
			var existingHashtag = await _context.Hashtags
				.FirstOrDefaultAsync(h => h.Name == hashtag);
			if (existingHashtag != null)
			{
				existingHashtag.Count--;
				if (existingHashtag.Count <= 0)
				{
					_context.Hashtags.Remove(existingHashtag); // Remove hashtag if count is zero
				}
				else
				{
					existingHashtag.UpdatedAt = DateTime.UtcNow;
					_context.Hashtags.Update(existingHashtag);
				}
				await _context.SaveChangesAsync();
			}
		}
	}
}
