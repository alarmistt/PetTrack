using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Wallet : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Balance { get; set; }

        public ICollection<WalletTransaction> Transactions { get; set; }
    }
}
