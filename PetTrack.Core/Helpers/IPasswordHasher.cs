﻿namespace PetTrack.Core.Helpers
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string inputPassword);
    }
}
