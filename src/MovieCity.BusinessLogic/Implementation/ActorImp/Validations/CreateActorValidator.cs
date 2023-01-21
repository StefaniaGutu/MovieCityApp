using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.ActorImp.Validations
{
    public class CreateActorValidator : AbstractValidator<CreateActorModel>
    {
        private readonly UnitOfWork unitOfWork;
        public CreateActorValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field")
                .Must(ActorNotAlreadyExist).WithMessage("Already exists an actor with this name");
            RuleFor(r => r.Image)
                .Must(HasAcceptedFormat).WithMessage("Upload an .jpg/.png/.jpeg file");
        }

        private bool ActorNotAlreadyExist(string name)
        {
            return !unitOfWork.Actors.Get().Any(a => a.Name == name);
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
