using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class BankAccount : BaseEntity
    {
        public string BankName { get; set; }
        public string BankNumber { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
