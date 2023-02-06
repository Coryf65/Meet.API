using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Meet.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FileController : ControllerBase
{
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
