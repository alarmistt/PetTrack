using AutoMapper;
using PetTrack.Entity;
using PetTrack.ModelViews.Booking;

namespace PetTrack.Services.Mapper
{
    public class BookingMapperProfile : Profile
    {
        public BookingMapperProfile()
        {
            CreateMap<BookingRequestModel, Booking>().ReverseMap();
            CreateMap<Booking, BookingResponseModel>()
                .ForMember(dest => dest.ClinicName, opt => opt.MapFrom(src => src.Clinic != null ? src.Clinic.Name : null))
                .ForMember(dest => dest.ServicePackageName, opt => opt.MapFrom(src => src.ServicePackage != null ? src.ServicePackage.Name : null)).ReverseMap();
        }
    }
}
