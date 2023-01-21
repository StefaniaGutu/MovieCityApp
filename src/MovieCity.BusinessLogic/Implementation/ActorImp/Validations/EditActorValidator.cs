using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Validations
{
    public class EditActorValidator : AbstractValidator<EditActorModel>
    {
        private readonly UnitOfWork unitOfWork;
        public EditActorValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Name)
                .Must((name) => false)
                .When(s => ActorNotAlreadyExist(s.Name, s.Id))
                .WithMessage("Already exists an actor with this name");
            RuleFor(r => r.NewImage)
                .Must(HasAcceptedFormat).WithMessage("Upload an .jpg/.png/.jpeg file");
        }

        private bool ActorNotAlreadyExist(string name, Guid id)
        {
            return unitOfWork.Actors.Get().Any(a => a.Name == name && id != a.Id);
        }

        private bool HasAcceptedFormat(IFormFile image)
        {
            if (image == null)
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
    }
}
