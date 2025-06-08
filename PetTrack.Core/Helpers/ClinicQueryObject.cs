using PetTrack.Core.Enums;

namespace PetTrack.Core.Helpers
{
    public class ClinicQueryObject
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public ClinicStatus? Status { get; set; }
        public string? OwnerUserId { get; set; }
        public bool IncludedDeleted { get; set; } = false;

        // Paging & Sorting
        public string? SortBy { get; set; } = "CreatedTime";
        public bool IsDescending { get; set; } = true;

        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
