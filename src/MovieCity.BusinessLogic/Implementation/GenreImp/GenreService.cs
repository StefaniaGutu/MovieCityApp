using Microsoft.EntityFrameworkCore;
using MovieCity.BusinessLogic.Base;
using MovieCity.BusinessLogic.Implementation.GenreImp.Models;
using MovieCity.BusinessLogic.Implementation.GenreImp.Validations;
using MovieCity.Common.Exceptions;
using MovieCity.Common.Extensions;
using MovieCity.Entities;

namespace MovieCity.BusinessLogic.Implementation.GenreImp
{
    public class GenreService : BaseService
    {
        private readonly GenreValidator genreValidator;
        private readonly EditGenreValidator editGenreValidator;

        public GenreService(ServiceDependencies serviceDependencies)
            : base(serviceDependencies)
        {
            this.genreValidator = new GenreValidator(UnitOfWork);
            this.editGenreValidator = new EditGenreValidator(UnitOfWork);
        }

        public async Task<List<ListGenresModel>> GetGenres()
        {
            return await Mapper.ProjectTo<ListGenresModel>(UnitOfWork.Genres.Get()).ToListAsync();
        }

        public async Task CreateNewGenre(CreateGenreModel model)
        {
            genreValidator.Validate(model).ThenThrow(model);

            var newGenre = Mapper.Map<Genre>(model);

            UnitOfWork.Genres.Insert(newGenre);
            await UnitOfWork.SaveChangesAsync();
        }

        public async Task<List<string>> GetFavoriteGenres(Guid id)
        {
            var genresLists = await UnitOfWork.LikeMovie.Get()
                .Include(r => r.MovieSeries)
                    .ThenInclude(b => b.Genres)
                .Where(r => r.UserId == id)
                .Select(r => r.MovieSeries.Genres)
                .ToListAsync();

            var genres = new List<Genre>();

            genresLists.ForEach(genreList =>
            {
                genres.AddRange(genreList);
            });

            var favoriteGenres = genres.GroupBy(ab => (ab.Id, ab.Name)).OrderByDescending(g => g.Count()).Take(3).Select(a => a.Key.Name).ToList();
            return favoriteGenres;
        }

        public async Task<List<Guid>> GetFavoriteGenresIds(Guid id)
        {
            var genresLists = await UnitOfWork.LikeMovie.Get()
                .Include(r => r.MovieSeries)
                    .ThenInclude(b => b.Genres)
                .Where(r => r.UserId == id)
                .Select(r => r.MovieSeries.Genres)
                .ToListAsync();

            var genres = new List<Genre>();

            genresLists.ForEach(genreList =>
            {
                genres.AddRange(genreList);
            });

            var favoriteGenres = genres.GroupBy(ab => (ab.Id, ab.Name)).OrderByDescending(g => g.Count()).Take(3).Select(a => a.Key.Id).ToList();
            return favoriteGenres;
        }

        public async Task<EditGenreModel> GetEditGenreModelById(Guid id)
        {
            var genre = await UnitOfWork.Genres.Get().FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null)
                throw new NotFoundErrorException();

            var genreModel = Mapper.Map<EditGenreModel>(genre);

            return genreModel;
        }

        public async Task<bool> UpdateGenre(EditGenreModel model)
        {
            editGenreValidator.Validate(model).ThenThrow(model);

            var genreToUpdate = await UnitOfWork.Genres.Get().FirstOrDefaultAsync(g => g.Id == model.Id);

            if (genreToUpdate == null) return false;

            genreToUpdate.Name = model.Name;

            UnitOfWork.Genres.Update(genreToUpdate);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGenre(string id)
        {
            var GuidId = Guid.Parse(id);
            var genreToDelete = await UnitOfWork.Genres.Get().Include(e => e.Movies).FirstOrDefaultAsync(g => g.Id == GuidId);

            if(genreToDelete == null) return false;

            genreToDelete.Movies.Clear(); 

            UnitOfWork.Genres.Delete(genreToDelete);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
