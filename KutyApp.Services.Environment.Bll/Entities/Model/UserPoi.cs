namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class UserPoi
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int PoiId { get; set; }
        public Poi Poi { get; set; }
    }
}