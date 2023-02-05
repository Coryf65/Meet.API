using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Meet.API.Authorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
	private readonly ILogger _logger;

	public MinimumAgeHandler(ILogger logger)
	{
		_logger = logger;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
	{
		var userEmail = context.User.FindFirst(claim => claim.Type == ClaimTypes.Name).Value;
		// get users DOB from the Claim (jwt)
		var dateOfBirth = DateTime.Parse(context.User.FindFirst(claim => claim.Type == "DateOfBirth").Value);

		_logger.LogInformation("Checking Age requirement for {userEmail}", userEmail);

		if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
		{		
			// user meets requirement grant access
			context.Succeed(requirement);
		} else
		{
			_logger.LogInformation("Access Denied to {userEmail} for not meeting Age requirements", userEmail);
		}

		return Task.CompletedTask;
	}
}
