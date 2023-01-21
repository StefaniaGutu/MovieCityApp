using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasIndex(e => new { e.UserId, e.ReviewId }, "IX_Postare")
                .IsUnique();

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasOne(d => d.Review)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postare_User");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Postare_User1");
        }
    }
}
