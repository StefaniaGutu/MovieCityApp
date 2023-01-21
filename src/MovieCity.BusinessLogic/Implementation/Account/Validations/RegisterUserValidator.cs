using FluentValidation;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.DataAccess;
using System.Text.RegularExpressions;

namespace MovieCity.BusinessLogic.Implementation.Account.Validations
{
    public class RegisterUserValidator : AbstractValidator<RegisterModel>
    {
        private readonly UnitOfWork unitOfWork;
        public RegisterUserValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Required field")
                .Must(EmailNotAlreadyExist)
                .WithMessage("Already exists an account with this email")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("Not a valid email address");
            RuleFor(r => r.Password)
                .NotEmpty().WithMessage("Required field")
                .Must(IsAValidPassword).WithMessage("Password must contain at least a number and one uppercase letter and must have minimum 8 characters length");
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.ConfirmPassword)
                .NotEmpty().WithMessage("Required field");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match");
            RuleFor(r=> r.Username)
                .NotEmpty()
                .WithMessage("Required field")
                .Must(UsernameNotAlreadyExist)
                .WithMessage("Already exists an account with this username");
            RuleFor(r => r.BirthDate)
                .NotEmpty().WithMessage("Required field")
                .Must(IsAValidDateOfBirth).WithMessage("You must be between 14 and 100 years old");
        }

        private bool EmailNotAlreadyExist(string? email)
        {
            if (email == null)
                return true;

            return !unitOfWork.Users.Get().Any(u => u.Email == email && u.IsDeleted == false);
        }

        private bool UsernameNotAlreadyExist(string? username)
        {
            if (username == null)
                return true;

            return !unitOfWork.Users.Get().Any(u => u.Username == username && u.IsDeleted == false);
        }

        private bool IsAValidPassword(string? password)
        {
            if(password == null)
                return true;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");

            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
        }

        private bool IsAValidDateOfBirth(DateTime? date)
        {
            if (date == null)
                return true;

            var dateOfBirth = (DateTime)date;

            if (dateOfBirth.AddYears(14) > DateTime.Now)
                return false;

            return (dateOfBirth.AddYears(100) > DateTime.Now);
        }
    }
}
