using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class MoviesAndSeriesConfiguration : IEntityTypeConfiguration<MoviesAndSeries>
    {
        public void Configure(EntityTypeBuilder<MoviesAndSeries> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Title).HasMaxLength(50);

            builder.HasMany(d => d.Actors)
                .WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "MovieActor",
                    l => l.HasOne<Actor>().WithMany().HasForeignKey("ActorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FilmActor_Actor"),
                    r => r.HasOne<MoviesAndSeries>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_FilmActor_Film"),
                    j =>
                    {
                        j.HasKey("MovieId", "ActorId").HasName("PK_FilmActor");

                        j.ToTable("MovieActor");
                    });

            builder.HasMany(d => d.Genres)
                .WithMany(p => p.Movies)
                .UsingEntity<Dictionary<string, object>>(
                    "GenreMovie",
                    l => l.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_GenFilm_Gen"),
                    r => r.HasOne<MoviesAndSeries>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_GenFilm_Film"),
                    j =>
                    {
                        j.HasKey("MovieId", "GenreId").HasName("PK_GenFilm");

                        j.ToTable("GenreMovie");
                    });
        }
    }
}
