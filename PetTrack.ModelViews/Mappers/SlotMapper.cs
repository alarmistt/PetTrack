using PetTrack.Entity;
using PetTrack.ModelViews.SlotModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class SlotMapper
    {
        public static SlotResponse ToSlotDto(this Slot slot)
        {
            return new SlotResponse
            {
                Id = slot.Id,
                DayOfWeek = slot.DayOfWeek,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime
            };
        }
        public static List<SlotResponse> ToSlotDtoList(this IEnumerable<Slot> slots)
        {
            return slots.Select(s => s.ToSlotDto()).ToList();
        }
    }
}
