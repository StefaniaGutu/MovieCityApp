using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class LikeMovieConfiguration : IEntityTypeConfiguration<LikeMovie>
    {
        public void Configure(EntityTypeBuilder<LikeMovie> builder)
        {
            builder.HasKey(e => new { e.UserId, e.MovieSeriesId })
                    .HasName("PK_LikeFilm");

            builder.ToTable("LikeMovie");

            builder.Property(e => e.IsLiked).HasColumnName("isLiked");

            builder.HasOne(d => d.MovieSeries)
                .WithMany(p => p.LikeMovies)
                .HasForeignKey(d => d.MovieSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikeFilm_Film");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikeMovies)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikeFilm_User");
        }
    }
}
