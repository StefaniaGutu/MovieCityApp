using FluentValidation;
using MovieCity.BusinessLogic.Implementation.EpisodeImp.Models;
using MovieCity.DataAccess;

namespace MovieCity.BusinessLogic.Implementation.EpisodeImp.Validations
{
    public class EpisodeValidator : AbstractValidator<EpisodeModel>
    {
        private readonly UnitOfWork unitOfWork;
        public EpisodeValidator(UnitOfWork UnitOfWork)
        {
            this.unitOfWork = UnitOfWork;
            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Duration)
                .NotEmpty().WithMessage("Required field");
            RuleFor(r => r.Duration)
                .Must(d => d > 0).WithMessage("Duration must be a positive number");
            RuleFor(r => r.EpisodeNo)
                .NotEmpty().WithMessage("Required field")
                .Must(e => e > 0).WithMessage("EpisodeNo must be a positive number");
            RuleFor(s => s.EpisodeNo)
                .Must((episodeNo) => false)
                .When(s => NotAlreadyExist(s.EpisodeNo, s.SeasonId, s.Id))
                .WithMessage("Already exists an episode with this EpisodeNo");
        }

        private bool NotAlreadyExist(int episodeNo, Guid seasonId, Guid episodeId)
        {
            return unitOfWork.Episodes.Get().Any(g => g.EpisodeNo == episodeNo && g.SeasonId == seasonId && g.Id != episodeId);
        }
    }
}
