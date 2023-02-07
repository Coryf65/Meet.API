using FluentValidation;
using Meet.API.Entities;
using Meet.API.Models;

namespace Meet.API.Validators;

public class MeetupQueryValidator : AbstractValidator<MeetupQuery>
{
	private int[] pageSizesAllowed = new[] { 5, 15, 50 };
	private string[] columnNamesToSortBy = { nameof(Meetup.Date), nameof(Meetup.Organizer), nameof(Meetup.Name) };

	// rules
	public MeetupQueryValidator()
	{
		RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
		RuleFor(q => q.PageSize).Custom((value, context) =>
		{
			if (!pageSizesAllowed.Contains(value))
				context.AddFailure("PageSize", $"Page size must be set to one of the following '{string.Join(",", pageSizesAllowed)}'");			
		});

		RuleFor(q => q.SortBy)
			.Must(value => string.IsNullOrEmpty(value) || columnNamesToSortBy.Contains(value))
			.WithMessage($"Sort By is optional, if used must be one of the following: '{string.Join(", ", columnNamesToSortBy)}'");
	}
}
