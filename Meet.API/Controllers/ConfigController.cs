using Microsoft.AspNetCore.Mvc;

namespace Meet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
	private readonly IConfiguration _config;
	private readonly ILogger<ConfigController> _logger;

	public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger)
	{
		_config = configuration;
		_logger = logger;
	}

	[HttpOptions("reload")]
	public ActionResult Reload()
	{
		try
		{
			((IConfigurationRoot)_config).Reload();
			return Ok();
		}
		catch (Exception error)
		{
			_logger.LogError("Failed to reload the Config file, 'Meet.API.Controllers.ConfigController.Reload()' error: { error }", error.Message);
			return StatusCode(500, "Failed to reload the Configuration File.");
		}
	}
}
