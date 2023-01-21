using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.ToTable("Season");

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasOne(d => d.MovieSeries)
                .WithMany(p => p.Seasons)
                .HasForeignKey(d => d.MovieSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sezon_FilmSauSerial");
        }
    }
}
