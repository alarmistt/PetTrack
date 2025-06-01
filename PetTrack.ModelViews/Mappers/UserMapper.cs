using PetTrack.Entity;
using PetTrack.ModelViews.AuthenticationModels;

namespace PetTrack.ModelViews.Mappers
{
    public static class UserMapper
    {
        public static UserResponseModel ToUserDto(this User userModel)
        {
            return new UserResponseModel
            {
                Id = userModel.Id,
                FullName = userModel.FullName,
                Email = userModel.Email,
                Address = userModel.Address,
                PhoneNumber = userModel.PhoneNumber,
                Role = userModel.Role,
                AvatarUrl = userModel.AvatarUrl,
                CreatedTime = userModel.CreatedTime,
            };
        }

        public static List<UserResponseModel> ToListUserDto(this IEnumerable<User> userList)
        {
            return userList.Select(u => new UserResponseModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Address = u.Address,
                PhoneNumber = u.PhoneNumber,
                Role = u.Role,
                AvatarUrl = u.AvatarUrl,
                CreatedTime = u.CreatedTime,
            }).ToList();
        }
    }
}