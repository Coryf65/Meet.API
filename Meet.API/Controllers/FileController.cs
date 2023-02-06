using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Meet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
	// this will allow the client to pull from the browser cache for the duration
	[ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "filename" })]
	[HttpGet]
	public ActionResult GetFile(string filename)
	{
		string rootPath = Directory.GetCurrentDirectory();

		string fullFilePath = rootPath + "/PrivateFiles" + filename;

		if (!System.IO.File.Exists(fullFilePath))
			return NotFound($"a file with the name: '{filename}' was not found.");

		byte[] file = System.IO.File.ReadAllBytes(fullFilePath);
		FileExtensionContentTypeProvider fileProvider = new();

		fileProvider.TryGetContentType(fullFilePath, out var contentType);

		return File(fileContents: file, contentType: contentType, fileDownloadName: filename);
	}
}
