using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.ActorImp.Models;
using MovieCity.BusinessLogic.Implementation.ActorImp.Validations;
using MovieCity.BusinessLogic.Implementation.MovieImp;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;
using X.PagedList;

namespace MovieCity.BusinessLogic.Implementation.ActorImp
{
    public class ActorService : BaseService
    {
        private readonly CreateActorValidator createActorValidator;
        private readonly EditActorValidator editActorValidator;
        private readonly MovieService movieService;

        public ActorService(ServiceDependencies serviceDependencies, MovieService movieService) 
            : base(serviceDependencies)
        {
            this.createActorValidator = new CreateActorValidator(UnitOfWork);
            this.editActorValidator = new EditActorValidator(UnitOfWork);
            this.movieService = movieService;
        }

        public async Task<List<ListActorsModel>> GetAllActors()
        {
            return await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToListAsync();
        }

        public async Task<IPagedList<ListActorsModel>> GetActors(int pageNumber, int pageSize)
        {
            return await Mapper.ProjectTo<ListActorsModel>(UnitOfWork.Actors.Get()).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IPagedList<ViewActorDetailsModel>> GetActorsWithDetails(string searchString, int pageNumber, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
                return await Mapper.ProjectTo<ViewActorDetailsModel>(UnitOfWork.Actors.Get().Include(a => a.ActorImage))
                    .Where(m => m.Name.Contains(searchString))
                    .OrderBy(a => a.Name)
                    .ToPagedListAsync(pageNumber, pageSize);
            else
                return await Mapper.ProjectTo<ViewActorDetailsModel>(UnitOfWork.Actors.Get().Include(a => a.ActorImage))
                    .OrderBy(a => a.Name)
                    .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<ViewActorDetailsModel> GetActorDetailsModelById(Guid id)
        {
            var actor = await UnitOfWork.Actors.Get().Include(a => a.ActorImage).FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                throw new NotFoundErrorException();
            }
                
            var model = Mapper.Map<ViewActorDetailsModel>(actor);
            model.MoviesForActor = await movieService.GetMoviesForActor(id);
            return model;
        }

        public async Task<EditActorModel> GetEditActorModelById(Guid id)
        {
            var actor = await UnitOfWork.Actors.Get().Include(a => a.ActorImage).FirstOrDefaultAsync(a => a.Id == id);

            if (actor == null)
            {
                throw new NotFoundErrorException();
            }

            var model = Mapper.Map<EditActorModel>(actor);

            return model;
        }

        public async Task CreateActor(CreateActorModel model)
        {
            createActorValidator.Validate(model).ThenThrow(model);

            var actor = Mapper.Map<Actor>(model);

            if (model.Image != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.Image.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    actor.ActorImage = new ActorImage
                    {
                        Image = fileBytes
                    };
                }
            }

            UnitOfWork.Actors.Insert(actor);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateActor(EditActorModel model)
        {
            editActorValidator.Validate(model).ThenThrow(model);

            var actorToUpdate = await UnitOfWork.Actors.Get().Include(a => a.ActorImage).FirstOrDefaultAsync(g => g.Id == model.Id);

            if (actorToUpdate == null) return false;

            actorToUpdate.Name = model.Name;
            actorToUpdate.Description = model.Description;
            if (model.NewImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.NewImage.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    if (actorToUpdate.ActorImage != null)
                    {
                        actorToUpdate.ActorImage.Image = fileBytes;
                    }
                    else
                    {
                        actorToUpdate.ActorImage = new ActorImage
                        {
                            Image = fileBytes
                        };
                    }
                } 
            }
            else
            {
                if (model.DeleteActualImage)
                {
                    actorToUpdate.ActorImage = null;
                }
            }

            UnitOfWork.Actors.Update(actorToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteActor(string id)
        {
            var GuidId = Guid.Parse(id);
            var actorToDelete = await UnitOfWork.Actors.Get()
                .Include(a => a.Movies)
                .Include(a => a.ActorImage)
                .FirstOrDefaultAsync(g => g.Id == GuidId);

            if (actorToDelete == null) return false;

            actorToDelete.Movies.Clear();
            actorToDelete.ActorImage = null;

            UnitOfWork.Actors.Delete(actorToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
