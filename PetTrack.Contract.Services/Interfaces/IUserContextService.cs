namespace PetTrack.Contract.Services.Interfaces
{
    public interface IUserContextService
    {
        string GetUserId();
        string? GetUserRole();
    }
}
