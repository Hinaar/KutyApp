using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class DbVersionConfiguration : IEntityTypeConfiguration<DbVersion>
    {
        public void Configure(EntityTypeBuilder<DbVersion> builder)
        {
            builder.HasKey(v => v.Id);
            builder.Property(v => v.Value).IsRequired();
        }
    }
}
