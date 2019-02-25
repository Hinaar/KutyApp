using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class UserFriendshipConfiguration : IEntityTypeConfiguration<UserFriendship>
    {
        public void Configure(EntityTypeBuilder<UserFriendship> builder)
        {
            builder.HasKey(f => new { f.User1Id, f.User2Id });
            builder.HasOne(f => f.User1).WithMany().HasForeignKey(f => f.User1Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.User2).WithMany().HasForeignKey(f => f.User2Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
