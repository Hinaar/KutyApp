namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class UserFriendship
    {
        public virtual string User1Id { get; set; }
        public virtual string User2Id { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}