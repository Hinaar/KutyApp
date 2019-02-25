using KutyApp.Services.Environment.Bll.Entities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KutyApp.Services.Environment.Bll.Entities.Configurations
{
    public class PetConfiguration : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Ignore(p => p.Age);
            builder.HasMany(p => p.Habits).WithOne(h => h.Pet);
            builder.HasMany(p => p.MedicalTreatments).WithOne(m => m.Pet);
            builder.HasOne(p => p.Owner).WithMany(u => u.Pets);
        }
    }
}
