using FluentValidation;
using Meet.API.Data;
using Meet.API.Models;

namespace Meet.API.Validators;

/// <summary>
/// Fluent Validators, validations for the Register User DTO model
/// </summary>
public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
{
	public RegisterUserValidator(MeetupContext meetupContext)
	{	
		// no emptys
		RuleFor(x => x.Email).NotEmpty();
		RuleFor(x => x.Password).MinimumLength(6).NotEmpty();

		// password and confirm are equal
		RuleFor(x => x.Password).Equal(x => x.ConfirmPassword);

		// Custom Email Validator
		RuleFor(x => x.Email).Custom((value, context) =>
		{
			var userExists = meetupContext.Users.Any(
				user => user.Email == value
			);

			if (userExists)
				context.AddFailure("Email", "This Email is already taken.");
		});
	}
}
