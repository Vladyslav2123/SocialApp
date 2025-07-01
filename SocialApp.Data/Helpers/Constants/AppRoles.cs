namespace SocialApp.Data.Helpers.Constants;

public static class AppRoles
{
	public const string Admin = "Admin";
	public const string User = "User";
	public const string Moderator = "Moderator";
	public const string Guest = "Guest";
	public static readonly IReadOnlyList<string> All = new []
	{
		Admin,
		User,
		Moderator,
		Guest
	};
}
