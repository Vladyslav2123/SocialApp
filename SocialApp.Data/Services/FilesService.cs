using Microsoft.AspNetCore.Http;
using SocialApp.Data.Helpers.Enums;

namespace SocialApp.Data.Services;

public class FilesService : IFilesService
{
	public async Task<string> UploadImageAsync ( IFormFile file, ImageFileType fileType )
	{
		string filePathUpload = fileType switch
		{
			ImageFileType.PostImage => Path.Combine("images", "posts"),
			ImageFileType.StoryImage => Path.Combine("images", "stories"),
			ImageFileType.ProfilePicture => Path.Combine("images", "profilePictures"),
			ImageFileType.CoverImage => Path.Combine("images", "covers"),
			_ => throw new ArgumentException("Invalid file type", nameof(fileType))
		};

		if (file != null && file.Length > 0)
		{
			string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
			if (file.ContentType.Contains("image"))
			{
				string rootPathImage = Path.Combine(rootPath, filePathUpload);
				Directory.CreateDirectory(rootPathImage);

				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				string filePath = Path.Combine(rootPathImage, fileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
					await file.CopyToAsync(fileStream);

				return $"{filePathUpload}\\{fileName}";
			}
		}
		return string.Empty; // Return an empty string if the file is null or not an image
	}
}