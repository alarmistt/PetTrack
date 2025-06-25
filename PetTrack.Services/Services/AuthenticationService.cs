using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetTrack.Contract.Repositories.Interfaces;
using PetTrack.Contract.Services.Interfaces;
using PetTrack.Core.Config;
using PetTrack.Core.Constants;
using PetTrack.Core.Enums;
using PetTrack.Core.Exceptions;
using PetTrack.Core.Helpers;
using PetTrack.Core.Models;
using PetTrack.Entity;
using PetTrack.ModelViews.AuthenticationModels;
using PetTrack.ModelViews.Mappers;
using PetTrack.ModelViews.UserModels;
using PetTrack.Services.Infrastructure;

namespace PetTrack.Services.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserContextService _userContextService;
        private readonly IWalletService _walletService;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly AppSettings _appSettings;

        public AuthenticationService(IUnitOfWork unitOfWork, JwtSettings jwtSettings, JwtTokenGenerator tokenGenerator, IUserContextService userContextService, IWalletService walletService, IPasswordHasher passwordHasher, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _tokenGenerator = tokenGenerator;
            _userContextService = userContextService;
            _walletService = walletService;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        public async Task<BasePaginatedList<UserResponseModel>> GetPagedUsers(UserQueryObject query)
        {
            var usersQuery = _unitOfWork.GetRepository<User>().Entities
                           .AsNoTracking()
                           .Where(u => u.DeletedTime == null);

            if (!string.IsNullOrWhiteSpace(query.Id))
                usersQuery = usersQuery.Where(u => u.Id == query.Id);

            if (!string.IsNullOrWhiteSpace(query.FullName))
                usersQuery = usersQuery.Where(u => u.FullName.Contains(query.FullName));

            if (!string.IsNullOrWhiteSpace(query.Email))
                usersQuery = usersQuery.Where(u => u.Email.Contains(query.Email));

            if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
                usersQuery = usersQuery.Where(u => u.PhoneNumber!.Contains(query.PhoneNumber));

            if (query.Role.HasValue)
                usersQuery = usersQuery.Where(u => u.Role == query.Role.Value.ToString());


            // Sorting logic
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                usersQuery = query.SortBy.ToLower() switch
                {
                    "fullname" => query.IsDescending
                        ? usersQuery.OrderByDescending(u => u.FullName)
                        : usersQuery.OrderBy(u => u.FullName),

                    "email" => query.IsDescending
                        ? usersQuery.OrderByDescending(u => u.Email)
                        : usersQuery.OrderBy(u => u.Email),

                    _ => usersQuery.OrderByDescending(u => u.CreatedTime)
                };
            }
            else
            {
                usersQuery = usersQuery.OrderByDescending(u => u.CreatedTime);
            }

            // Paging
            int totalCount = await usersQuery.CountAsync();

            var pagedUsers = await usersQuery
                .Skip((query.PageIndex - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var dto = pagedUsers.ToListUserDto();

            return new BasePaginatedList<UserResponseModel>(dto, totalCount, query.PageIndex, query.PageSize);
        }

        public async Task<UserResponseModel> GetUserById(string id)
        {
            User user = await _unitOfWork.GetRepository<User>().Entities.FirstOrDefaultAsync(u => u.Id == id && !u.DeletedTime.HasValue) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            return user.ToUserDto();
        }

        public async Task<UserResponseModel> GetUserInfo()
        {
            string userId = _userContextService.GetUserId();
            User user = await _unitOfWork.GetRepository<User>().Entities.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            return user.ToUserDto();
        }

        public async Task<AuthenticationModel> Login(GoogleLoginRequest request)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(request.IdToken);
            var googleUser = GoogleUserInfo.FromDecodedToken(decodedToken);

            var userRepo = _unitOfWork.GetRepository<User>();
            var user = await userRepo.Entities.FirstOrDefaultAsync(u => u.Email == googleUser.Email);

            if (user != null)
            {
                // Merge Google info
                user.FullName ??= googleUser.Name;
                user.AvatarUrl ??= googleUser.Picture;
                user.LastUpdatedTime = CoreHelper.SystemTimeNow;

                await userRepo.UpdateAsync(user);
            }
            else
            {
                user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = googleUser.Email,
                    FullName = googleUser.Name,
                    Role = UserRole.User.ToString(),
                    AvatarUrl = googleUser.Picture,
                    IsPasswordSet = false
                };

                await userRepo.InsertAsync(user);
            }

            await _unitOfWork.SaveAsync();
            await _walletService.CreateWalletIfNotExistsAsync(user.Id);

            return await _tokenGenerator.CreateToken(user, _jwtSettings);
        }

        public async Task<UserResponseModel> UpdateUserAsync(string id, UpdateUserRequest request)
        {
            User user = await _unitOfWork.GetRepository<User>().Entities
                .FirstOrDefaultAsync(u => u.Id == id && !u.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            user.FullName = request.FullName;
            user.Address = request.Address;
            user.PhoneNumber = request.PhoneNumber;
            user.AvatarUrl = request.AvatarUrl;
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return user.ToUserDto();
        }

        public async Task DeleteUserAsync(string id)
        {
            User user = await _unitOfWork.GetRepository<User>().Entities
                .FirstOrDefaultAsync(u => u.Id == id && !u.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            user.DeletedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserResponseModel> UpdateUserRoleAsync(string userId, UserRole newRole)
        {
            if (newRole == UserRole.Admin)
                throw new ErrorException(StatusCodes.Status403Forbidden, "FORBIDDEN", "You cannot assign Admin role via this API.");

            var user = await _unitOfWork.GetRepository<User>().Entities
                .FirstOrDefaultAsync(u => u.Id == userId && !u.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            user.Role = newRole.ToString();
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();

            return user.ToUserDto();
        }

        public async Task UpdateRoleClinicAsync(string userId)
        {
            var user = await _unitOfWork.GetRepository<User>().Entities
                .FirstOrDefaultAsync(u => u.Id == userId && !u.DeletedTime.HasValue)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            user.Role = UserRole.Clinic.ToString();
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserResponseModel> RegisterAsync(UserRegistrationRequest request)
        {
            var normalizedEmail = NormalizeEmail(request.Email);

            var userRepo = _unitOfWork.GetRepository<User>();
            var exists = await userRepo.Entities
                .AsNoTracking()
                .AnyAsync(u => u.Email == normalizedEmail && !u.DeletedTime.HasValue);

            if (exists)
                throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.DUPLICATE, "Email is already registered.");

            var user = new User
            {
                FullName = request.FullName,
                Email = normalizedEmail,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.User.ToString(),
                IsPasswordSet = true
            };

            await userRepo.InsertAsync(user);
            await _unitOfWork.SaveAsync();

            await _walletService.CreateWalletIfNotExistsAsync(user.Id);

            var confirmationLink = $"{_appSettings.BaseClientUrl}/verify-email?userId={user.Id}";
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "WelcomeEmailTemplate.html");

            string emailTemplate = await File.ReadAllTextAsync(templatePath);
            emailTemplate = emailTemplate
                .Replace("{{UserName}}", user.FullName)
                .Replace("{{AppLink}}", confirmationLink)
                .Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

            await _emailService.SendEmailAsync(user.Email, "Welcome to PetTrack", emailTemplate);

            return user.ToUserDto();
        }

        public async Task<AuthenticationModel> LoginWithEmailPasswordAsync(LoginRequest request)
        {
            var normalizedEmail = NormalizeEmail(request.Email);

            var user = await _unitOfWork.GetRepository<User>().Entities.FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null || !user.IsPasswordSet || !_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Invalid email or password.");
            }

            if (user.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status403Forbidden, ResponseCodeConstants.FORBIDDEN, "User account is inactive or deleted.");
            }

            return await _tokenGenerator.CreateToken(user, _jwtSettings);
        }

        public async Task SetPasswordAsync(string userId, SetPasswordRequest request)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId)
                ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found");

            if (user.IsPasswordSet)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "You have already set a password.");

            if (request.Password != request.ConfirmPassword)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Passwords do not match.");

            if (!PasswordHelper.IsStrongPassword(request.Password))
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Password must contain at least 1 uppercase, 1 lowercase, 1 digit, 1 special character and be at least 8 characters long.");

            user.PasswordHash = _passwordHasher.HashPassword(request.Password);
            user.IsPasswordSet = true;
            user.LastUpdatedTime = CoreHelper.SystemTimeNow;

            await _unitOfWork.GetRepository<User>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        // Shared privated methods
        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLowerInvariant();
        }
    }
}