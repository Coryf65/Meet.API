using Microsoft.AspNetCore.Authorization;

namespace Meet.API.Authorization;

public enum OperationType
{
	Create,
	Read,
	Update,
	Delete
}

public class ResourceOperationRequirement : IAuthorizationRequirement
{
	public OperationType OperationType { get; }

	public ResourceOperationRequirement(OperationType operationType)
	{
		OperationType = operationType;
	}

}
