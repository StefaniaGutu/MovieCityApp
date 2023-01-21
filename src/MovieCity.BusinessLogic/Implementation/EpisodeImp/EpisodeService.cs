using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.EpisodeImp.Models;
using MovieCity.BusinessLogic.Implementation.EpisodeImp.Validations;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.EpisodeImp
{
    public class EpisodeService : BaseService
    {
        private readonly EpisodeValidator episodeValidator;

        public EpisodeService(ServiceDependencies serviceDependencies) 
            : base(serviceDependencies)
        {
            this.episodeValidator = new EpisodeValidator(UnitOfWork);
        }

        public async Task CreateNewEpisode(EpisodeModel model)
        {
            episodeValidator.Validate(model).ThenThrow(model);

            var newEpisode = Mapper.Map<Episode>(model);

            UnitOfWork.Episodes.Insert(newEpisode);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<EpisodeModel> GetEpisodeModelById(Guid id)
        {
            var episode = await UnitOfWork.Episodes.Get().Include(e => e.Season).FirstOrDefaultAsync(e => e.Id == id);

            if (episode == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<EpisodeModel>(episode);
            model.SeriesId = episode.Season.MovieSeriesId;

            return model;
        }

        public async Task<bool> UpdateEpisode(EpisodeModel model)
        {
            episodeValidator.Validate(model).ThenThrow(model);

            var episodeToUpdate = await UnitOfWork.Episodes
                .Get()
                .FirstOrDefaultAsync(m => m.Id == model.Id);

            if(episodeToUpdate == null) return false;

            episodeToUpdate.Name = model.Name;
            episodeToUpdate.EpisodeNo = model.EpisodeNo;
            episodeToUpdate.Duration = model.Duration;

            UnitOfWork.Episodes.Update(episodeToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEpisode(string id)
        {
            var guidId = Guid.Parse(id);
            var episodeToDelete = await UnitOfWork.Episodes
                .Get()
                .FirstOrDefaultAsync(g => g.Id == guidId);

            if (episodeToDelete == null) return false;

            UnitOfWork.Episodes.Delete(episodeToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
