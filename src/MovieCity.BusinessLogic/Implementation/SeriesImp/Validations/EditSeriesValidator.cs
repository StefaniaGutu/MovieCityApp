using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.SeriesImp.Models;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Validations
{
    public class EditSeriesValidator : AbstractValidator<EditSeriesModel>
    {
        public EditSeriesValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.NewImage)
                .Must(HasAcceptedFormat).WithMessage("Upload an .jpg/.png/.jpeg file");
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
