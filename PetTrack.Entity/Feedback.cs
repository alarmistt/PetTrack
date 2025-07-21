using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Feedback : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string? Comment { get; set; }
    }
}
