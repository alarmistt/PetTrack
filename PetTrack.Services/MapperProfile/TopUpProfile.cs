using AutoMapper;
using PetTrack.Entity;
using PetTrack.ModelViews.AuthenticationModels;
using PetTrack.ModelViews.TopUpModels;

namespace PetTrack.Services.MapperProfile
{
    public class TopUpProfile:Profile
    {
        public TopUpProfile() {
            CreateMap<TopUpResponse, TopUpTransaction>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();
        }
    }
}
