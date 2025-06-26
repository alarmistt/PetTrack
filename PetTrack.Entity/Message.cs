using PetTrack.Core.Models;

namespace PetTrack.Entity
{
    public class Message : BaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; } // MessageStatus enum values as string (Sent, Received, Read)

        // Navigation properties
        public User? Sender { get; set; }
        public User? Receiver { get; set; }
    }
}
