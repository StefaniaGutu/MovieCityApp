using FluentValidation;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp.Validations
{
    public class EditSeasonValidator : AbstractValidator<EditSeasonModel>
    {
        private readonly UnitOfWork unitOfWork;
        public EditSeasonValidator(UnitOfWork UnitOfWork)
        {
            this.unitOfWork = UnitOfWork;
            RuleFor(s => s.ReleaseDate)
               .NotEmpty()
               .WithMessage("Required field");
            RuleFor(r => r.ReleaseDate)
                .Must(d => d < DateTime.Now).WithMessage("Release date can't be in the future");
            RuleFor(s => s.Name)
               .NotEmpty()
               .WithMessage("Required field");
            RuleFor(s => s.Name)
               .Must((name) => false)
               .When(s => NotAlreadyExist(s.Name, s.MovieSeriesId, s.Id))
               .WithMessage("Already exists an season with this name");
        }

        private bool NotAlreadyExist(string seasonName, Guid seriesId, Guid seasonId)
        {
            return unitOfWork.Seasons.Get().Any(u => u.Name == seasonName && seriesId == u.MovieSeriesId && u.Id != seasonId);
        }
    }
}
