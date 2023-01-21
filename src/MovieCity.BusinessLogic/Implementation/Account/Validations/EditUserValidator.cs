using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.Account.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.Account.Validations
{
    public class EditUserValidator : AbstractValidator<ViewUserProfileModel>
    {
        private readonly UnitOfWork unitOfWork;
        public EditUserValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Required field");
            RuleFor(r => r.Email)
                .Must((email) => false)
                .When(r => EmailNotAlreadyExist(r.Email, r.Id))
                .WithMessage("Already exists an account with this email");
            RuleFor(r => r.Email)
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("Not a valid email address");
            RuleFor(r => r.FirstName)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.LastName)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Username)
                .NotEmpty()
                .WithMessage("Required field");
            RuleFor(r => r.Username)
                .Must((username) => false)
                .When(r => UsernameNotAlreadyExist(r.Username, r.Id))
                .WithMessage("Already exists an account with this username");
            RuleFor(r => r.NewImage)
                .Must(HasAcceptedFormat).WithMessage("Upload an .jpg/.png/.jpeg file");
            RuleFor(r => r.BirthDate)
                .NotEmpty().WithMessage("Required field")
                .Must(IsAValidDateOfBirth).WithMessage("You must be between 14 and 100 years old");
        }

        private bool EmailNotAlreadyExist(string email, Guid id)
        {
            return unitOfWork.Users.Get().Any(u => u.Email == email && u.Id != id);
        }

        private bool UsernameNotAlreadyExist(string username, Guid id)
        {
            return unitOfWork.Users.Get().Any(u => u.Username == username && u.Id != id);
        }

        private bool HasAcceptedFormat(IFormFile image)
        {
            if(image == null)
            {
                return true;
            }

            var acceptedTypes = new List<string>()
            {
                "image/jpeg",
                "image/jpg",
                "image/png"
            };

            return acceptedTypes.Contains(image.ContentType);
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
