using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Meet.API.Filters;

/// <summary>
/// Can track how long a method took to run and 
/// what aciton is being preformed at the time for debug info.
/// Used as a Attribute to a method or class [TimeTrackFilter]
/// </summary>
public class TimeTrackFilter : IActionFilter
{
	private readonly ILogger<TimeTrackFilter> _logger;
	private Stopwatch _stopwatch;

	public TimeTrackFilter(ILogger<TimeTrackFilter> logger)
	{
		_logger = logger;
	}

	public void OnActionExecuted(ActionExecutedContext context)
	{
		_stopwatch.Stop();

		var milliseconds = _stopwatch.ElapsedMilliseconds;
		var action = context.ActionDescriptor.DisplayName;

		_logger.LogInformation($"Action [{action}], executed in: {milliseconds} milliseconds");
	}

	public void OnActionExecuting(ActionExecutingContext context)
	{
		_stopwatch = new Stopwatch();
		_stopwatch.Start();
	}
}