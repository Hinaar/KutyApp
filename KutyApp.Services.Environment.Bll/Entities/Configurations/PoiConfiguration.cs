using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class PoiConfiguration : IEntityTypeConfiguration<Poi>
    {
        public void Configure(EntityTypeBuilder<Poi> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Location).IsRequired();
        }
    }
}
