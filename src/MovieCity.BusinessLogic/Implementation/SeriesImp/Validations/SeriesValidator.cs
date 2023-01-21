using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.SeriesImp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.SeriesImp.Validations
{
    public class SeriesValidator : AbstractValidator<CreateSeriesModel>
    {
        public SeriesValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Image)
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
