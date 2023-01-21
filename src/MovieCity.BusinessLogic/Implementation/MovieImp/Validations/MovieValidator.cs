using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieCity.BusinessLogic.Implementation.MovieImp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCity.BusinessLogic.Implementation.MovieImp.Validations
{
    public class MovieValidator : AbstractValidator<CreateMovieModel>
    {
        public MovieValidator()
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Duration)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Duration)
                .Must(d => d > 0).WithMessage("Duration must be a positive number");
            RuleFor(r => r.ReleaseDate)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.ReleaseDate)
                .Must(d => d < DateTime.Now).WithMessage("Release date can't be in the future");
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
