using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class MovieOrSeriesImageConfiguration : IEntityTypeConfiguration<MovieOrSeriesImage>
    {
        public void Configure(EntityTypeBuilder<MovieOrSeriesImage> builder)
        {
            builder.HasKey(e => e.MovieSeriesId)
                   .HasName("PK_ImagineFilmSauSerial");

            builder.ToTable("MovieOrSeriesImage");

            builder.Property(e => e.MovieSeriesId).ValueGeneratedNever();

            builder.HasOne(d => d.MovieSeries)
                .WithOne(p => p.MovieOrSeriesImage)
                .HasForeignKey<MovieOrSeriesImage>(d => d.MovieSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieOrSeriesImage_MoviesAndSeries");
        }
    }
}
