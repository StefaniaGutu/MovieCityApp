using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Models;
using MovieCity.BusinessLogic.Implementation.SeasonImp.Validations;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.SeasonImp
{
    public class SeasonService : BaseService
    {
        private readonly SeasonValidator seasonValidator;
        private readonly EditSeasonValidator editSeasonValidator;
        public SeasonService(ServiceDependencies serviceDependencies) 
            : base(serviceDependencies)
        {
            this.seasonValidator = new SeasonValidator(UnitOfWork);
            this.editSeasonValidator = new EditSeasonValidator(UnitOfWork);
        }

        public async Task CreateNewSeason(SeasonModel model)
        {
            seasonValidator.Validate(model).ThenThrow(model);

            var newSeason = Mapper.Map<Season>(model);

            UnitOfWork.Seasons.Insert(newSeason);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<EditSeasonModel> GetEditSeasonModelById(Guid id)
        {
            var season = await UnitOfWork.Seasons.Get().FirstOrDefaultAsync(a => a.Id == id);

            if (season == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<EditSeasonModel>(season);

            return model;
        }

        public async Task<List<ListSeasonModel>> GetSeasonsForSeries(Guid id)
        {
            return await Mapper.ProjectTo<ListSeasonModel>(UnitOfWork.Seasons.Get().Include(s => s.Episodes)
                .Where(s => s.MovieSeriesId == id))
                .ToListAsync();
        }

        public async Task<bool> UpdateSeason(EditSeasonModel model)
        {
            editSeasonValidator.Validate(model).ThenThrow(model);

            var seasonToUpdate = await UnitOfWork.Seasons.Get()
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if(seasonToUpdate == null) return false;

            seasonToUpdate.Name = model.Name;
            seasonToUpdate.ReleaseDate = model.ReleaseDate;

            UnitOfWork.Seasons.Update(seasonToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSeason(Guid id)
        {
            var episodesToDelete = await UnitOfWork.Episodes.Get()
                .Where(g => g.SeasonId == id)
                .ToListAsync();

            UnitOfWork.Episodes.DeleteRange(episodesToDelete);
            
            var seasonToDelete = await UnitOfWork.Seasons.Get()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (seasonToDelete == null) return false;

            UnitOfWork.Seasons.Delete(seasonToDelete);
            await UnitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
