using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class UserPoiConfiguration : IEntityTypeConfiguration<UserPoi>
    {
        public void Configure(EntityTypeBuilder<UserPoi> builder)
        {
            builder.HasKey(up => new { up.UserId, up.PoiId });
            builder.HasOne(up => up.User).WithMany(u => u.FavoritePlaces).HasForeignKey(up => up.UserId);
            builder.HasOne(up => up.Poi).WithMany(p => p.FavouredByUsers).HasForeignKey(up => up.PoiId);
        }
    }
}
