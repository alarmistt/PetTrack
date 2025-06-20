using PetTrack.Core.Enums;
using PetTrack.Core.Models;

namespace PetTrack.Core.Helpers
{
    public class ClinicQueryObject : BaseQueryObject
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public ClinicStatus? Status { get; set; }
        public string? OwnerUserId { get; set; }
        public bool IncludedDeleted { get; set; } = false;
    }
}
