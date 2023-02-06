using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Meet.API.Filters;

/// <summary>
/// Can track how long a method took to run and 
/// what aciton is being preformed at the time for debug info.
/// Used as a Attribute to a method or class [TimeTrackFilter]
/// </summary>
public class TimeTrackFilter : Attribute, IActionFilter
{
	private Stopwatch _stopwatch;

	public void OnActionExecuted(ActionExecutedContext context)
	{
		_stopwatch.Stop();

		var milliseconds = _stopwatch.ElapsedMilliseconds;
		var action = context.ActionDescriptor.DisplayName;

		Debug.WriteLine($"Action [{action}], executed in: {milliseconds} milliseconds");
	}

	public void OnActionExecuting(ActionExecutingContext context)
	{
		_stopwatch = new Stopwatch();
		_stopwatch.Start();
	}
}