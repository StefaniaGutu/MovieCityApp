using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class ActorImageConfiguration : IEntityTypeConfiguration<ActorImage>
    {
        public void Configure(EntityTypeBuilder<ActorImage> builder)
        {
            builder.HasKey(e => e.ActorId)
                    .HasName("PK_ImagineActor");

            builder.ToTable("ActorImage");

            builder.Property(e => e.ActorId).ValueGeneratedNever();

            builder.HasOne(d => d.Actor)
                .WithOne(p => p.ActorImage)
                .HasForeignKey<ActorImage>(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagineActor_Actor");
        }
    }
}
