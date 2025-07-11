﻿using Microsoft.AspNetCore.Identity;

namespace SocialApp.Data.Models;

public class User : IdentityUser<int>
{
	public string FullName { get; set; } = string.Empty;
	public string? ProfilePictureUrl { get; set; } = null;
	public string? Bio { get; set; } = string.Empty;
	public bool IsDeleted { get; set; } = false;

	// Navigation properties
	public ICollection<Post> Posts { get; set; } = new List<Post>();
	public ICollection<Story> Stories { get; set; } = new List<Story>();
	public ICollection<Like> Likes { get; set; } = new List<Like>();
	public ICollection<Comment> Comments { get; set; } = new List<Comment>();
	public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
	public ICollection<Report> Reports { get; set; } = new List<Report>();
}
