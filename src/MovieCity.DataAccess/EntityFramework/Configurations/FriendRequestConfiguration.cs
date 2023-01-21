using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieCity.Entities;

namespace MovieCity.DataAccess.EntityFramework.Configurations
{
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(e => new { e.SenderId, e.ReceiverId })
                    .HasName("PK_Cerere");

            builder.ToTable("FriendRequest");

            builder.HasOne(d => d.Receiver)
                .WithMany(p => p.FriendRequestReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cerere_User1");

            builder.HasOne(d => d.Sender)
                .WithMany(p => p.FriendRequestSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cerere_User");
        }
    }
}
