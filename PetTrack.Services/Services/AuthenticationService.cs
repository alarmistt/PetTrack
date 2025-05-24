using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Config;
using PetTrack.Core.Enums;
using PetTrack.Entity;
using PetTrack.ModelViews.AuthenticationModels;
using PetTrack.Services.Infrastructure;

namespace PetTrack.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthenticationService(IUnitOfWork unitOfWork, JwtSettings jwtSettings, JwtTokenGenerator tokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<AuthenticationModel> Login(GoogleLoginRequest request)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.IdToken);
            var googleUser = GoogleUserInfo.FromDecodedToken(decodedToken);

            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.Entities.FirstOrDefaultAsync(u => u.Email == googleUser.Email);

            if (user == null)
            {
                user = new User
                {
                    Email = googleUser.Email,
                    FullName = googleUser.Name,
                    Role = UserRole.User.ToString(),
                    AvatarUrl = googleUser.Picture
                };
                await userRepo.InsertAsync(user);
                await _unitOfWork.SaveAsync();
            }

            return await _tokenGenerator.CreateToken(user, _jwtSettings);
        }
    }
}