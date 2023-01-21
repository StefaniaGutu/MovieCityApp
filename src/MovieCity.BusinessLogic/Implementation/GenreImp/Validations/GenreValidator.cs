using FluentValidation;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.GenreImp.Validations
{
    public class GenreValidator : AbstractValidator<CreateGenreModel>
    {
        private readonly UnitOfWork unitOfWork;
        public GenreValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Required field")
                .Must(NotAlreadyExist).WithMessage("This genre already exists");
        }

        private bool NotAlreadyExist(string genreName)
        {
            return !(unitOfWork.Genres.Get().Any(g => g.Name == genreName));
        }
    }
}
