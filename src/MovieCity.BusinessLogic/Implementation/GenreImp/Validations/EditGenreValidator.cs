using FluentValidation;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.GenreImp.Validations
{
    public class EditGenreValidator : AbstractValidator<EditGenreModel>
    {
        private readonly UnitOfWork unitOfWork;
        public EditGenreValidator(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Required field");
            RuleFor(g => g.Name)
                .Must((name) => false)
                .When(s => NotAlreadyExist(s.Name, s.Id))
                .WithMessage("This genre already exists");
        }

        private bool NotAlreadyExist(string genreName, Guid genreId)
        {
            return unitOfWork.Genres.Get().Any(g => g.Name == genreName && g.Id != genreId);
        }
    }
}
