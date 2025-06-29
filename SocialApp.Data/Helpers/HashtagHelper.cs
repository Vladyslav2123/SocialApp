using System.Text.RegularExpressions;

namespace SocialApp.Data.Helpers;

public static class HashtagHelper
{
	public static List<string> GetHashtags ( string text )
	{
		var hashtagPattern = new Regex(@"#\w+");

		var matches = hashtagPattern.Matches(text)
			.Select(match => match.Value
			.TrimEnd('.', ',', '!', '?')
			.ToLower())
			.Distinct()
			.ToList();

		return matches;
	}
}
