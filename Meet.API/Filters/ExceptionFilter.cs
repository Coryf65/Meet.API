using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Meet.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
	private readonly ILogger<ExceptionFilter> _logger;

	public ExceptionFilter(ILogger<ExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		// log the error
		_logger.LogCritical("Caught an Exception in ExceptionFilter {exception}", context.Exception);

		// we could also return info to the client
		JsonResult result = new("Something went wrong")
		{
			StatusCode = 500
		};

		context.Result = result;
	}
}
