using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasKey(e => new { e.User1Id, e.User2Id })
                    .HasName("PK_Prieten");

            builder.ToTable("Friend");

            builder.HasOne(d => d.User1)
                .WithMany(p => p.User2Ids)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prieten_User1");

            builder.HasOne(d => d.User2)
                .WithMany(p => p.User1Ids)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prieten_User");
        }
    }
}
