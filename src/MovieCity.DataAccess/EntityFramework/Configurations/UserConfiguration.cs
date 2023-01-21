using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.BirthDate)
                .HasMaxLength(10)
                .IsFixedLength();

            builder.Property(e => e.Email).HasMaxLength(255);

            builder.Property(e => e.FirstName).HasMaxLength(30);

            builder.Property(e => e.FullName)
                .HasMaxLength(61)
                .HasComputedColumnSql("(([FirstName]+' ')+[LastName])", true);

            builder.Property(e => e.LastName).HasMaxLength(30);

            builder.Property(e => e.Password).HasMaxLength(500);

            builder.Property(e => e.Username).HasMaxLength(30);

            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);
        }
    }
}
