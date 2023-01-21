using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class UserImageConfiguration : IEntityTypeConfiguration<UserImage>
    {
        public void Configure(EntityTypeBuilder<UserImage> builder)
        {
            builder.HasKey(e => e.UserId)
                    .HasName("PK_ImagineUser");

            builder.ToTable("UserImage");

            builder.Property(e => e.UserId).ValueGeneratedNever();

            builder.HasOne(d => d.User)
                .WithOne(p => p.UserImage)
                .HasForeignKey<UserImage>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagineUser_User");
        }
    }
}
