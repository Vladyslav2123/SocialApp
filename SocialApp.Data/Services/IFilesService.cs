using Microsoft.AspNetCore.Http;
using SocialApp.Data.Helpers.Enums;

namespace SocialApp.Data.Services;

public interface IFilesService
{
	Task<string> UploadImageAsync ( IFormFile file, ImageFileType fileType );
}
