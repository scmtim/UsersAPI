using FluentValidation;
using System.Text.Json.Serialization;
using UsersAPI.Utility;

namespace UsersAPI.Domain
{
    public class User 
    {
        public int Id { get; set; }
        public required string FirstName {  get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string PhoneNumber {  get; set; }

        public int Age { 
            get {
                var today = DateTime.Today;
                return YearCalculator.YearDifference(DateOfBirth, today);
            } 
        }

    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(128);
            RuleFor(x => x.LastName).MaximumLength(128);
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.PhoneNumber).Matches(@"^\d{10}$");
            RuleFor(x => x.DateOfBirth).Must(dob => YearCalculator.YearDifference(dob, DateTime.Now) >= 18);
        }
    }
}
