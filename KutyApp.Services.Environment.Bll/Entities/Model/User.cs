using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace KutyApp.Services.Environment.Bll.Entities.Model
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Advert> Adverts { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public ICollection<UserFriendship> Friendships { get; set; }
        public ICollection<UserPoi> FavoritePlaces { get; set; }
        public ICollection<PetSitting> PetSittings { get; set; }

        public User()
        {
            Adverts = new HashSet<Advert>();
            Pets = new HashSet<Pet>();
            Friendships = new HashSet<UserFriendship>();
            FavoritePlaces = new HashSet<UserPoi>();
            PetSittings = new HashSet<PetSitting>();
        }
    }
}
