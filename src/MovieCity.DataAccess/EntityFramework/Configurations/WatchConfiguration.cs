using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class WatchConfiguration : IEntityTypeConfiguration<Watch>
    {
        public void Configure(EntityTypeBuilder<Watch> builder)
        {
            builder.HasKey(e => new { e.UserId, e.MovieSeriesId, e.IsAlreadyWatched })
                    .HasName("PK_Vazut");

            builder.ToTable("Watch");

            builder.Property(e => e.IsAlreadyWatched).HasColumnName("isAlreadyWatched");

            builder.HasOne(d => d.MovieSeries)
                .WithMany(p => p.Watches)
                .HasForeignKey(d => d.MovieSeriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vazut_Film");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Watches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vazut_User");
        }
    }
}
