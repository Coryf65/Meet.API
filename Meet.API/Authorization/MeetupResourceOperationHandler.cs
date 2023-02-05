using Meet.API.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Meet.API.Authorization;

public class MeetupResourceOperationHandler : AuthorizationHandler<ResourceOperationRequirement, Meetup>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Meetup resource)
	{
		if (requirement.OperationType is OperationType.Create or OperationType.Read)
		{
			context.Succeed(requirement);
		}

		// they want to edit or delete the meetup
		var userId = context.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

		// check if they created the meetup
		if (resource.CreatedById == int.Parse(userId))
			context.Succeed(requirement);
		
		return Task.CompletedTask;
	}
}
