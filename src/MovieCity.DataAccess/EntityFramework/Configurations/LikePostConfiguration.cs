using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class LikePostConfiguration : IEntityTypeConfiguration<LikePost>
    {
        public void Configure(EntityTypeBuilder<LikePost> builder)
        {
            builder.HasKey(e => new { e.UserId, e.PostId })
                    .HasName("PK_LikePostare");

            builder.ToTable("LikePost");

            builder.Property(e => e.IsLiked).HasColumnName("isLiked");

            builder.HasOne(d => d.Post)
                .WithMany(p => p.LikePosts)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikePostare_Postare");

            builder.HasOne(d => d.User)
                .WithMany(p => p.LikePosts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LikePostare_User");
        }
    }
}
