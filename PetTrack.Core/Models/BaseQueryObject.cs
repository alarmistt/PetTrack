namespace PetTrack.Core.Models
{
    public class BaseQueryObject
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = true;
    }
}
