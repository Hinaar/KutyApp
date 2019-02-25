using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class PetSittingsConfiguration : IEntityTypeConfiguration<PetSitting>
    {
        public void Configure(EntityTypeBuilder<PetSitting> builder)
        {
            builder.HasKey(p => new { p.SitterId, p.PetId });
            builder.HasOne(p => p.Sitter).WithMany(u => u.PetSittings).HasForeignKey(p => p.SitterId);
            builder.HasOne(p => p.Pet).WithMany(p => p.PetSittings).HasForeignKey(p => p.PetId);
        }
    }
}
