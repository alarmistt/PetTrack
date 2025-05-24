using PetTrack.ModelViews.AuthenticationModels;

namespace PetTrack.Contract.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationModel> Login(GoogleLoginRequest request);
    }
}
