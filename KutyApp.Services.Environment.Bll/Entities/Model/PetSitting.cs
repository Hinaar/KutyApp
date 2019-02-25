namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class PetSitting
    {
        public string SitterId { get; set; }
        public virtual User Sitter { get; set; }
        public int PetId { get; set; }
        public virtual Pet Pet { get; set; }
    }
}