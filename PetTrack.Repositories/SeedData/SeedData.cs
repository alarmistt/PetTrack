using Microsoft.EntityFrameworkCore;
using PetTrack.Repositories.Base;

namespace PetTrack.Repositories.SeedData
{
    public class SeedData
    {
        private readonly PetTrackDbContext _context;

        public SeedData(PetTrackDbContext context)
        {
            _context = context;
        }

        public async Task Initialise()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    bool dbExists = _context.Database.CanConnect();
                    if (!dbExists)
                    {
                        _context.Database.Migrate();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _context.Dispose();
            }
        }
    }
}
