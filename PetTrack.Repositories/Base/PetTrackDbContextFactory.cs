using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PetTrack.Repositories.Base
{
    public class KidProgramingDbContextFactory : IDesignTimeDbContextFactory<PetTrackDbContext>
    {
        public PetTrackDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PetTrack"))
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();


            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PetTrackDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection")
                    , new MySqlServerVersion(new Version(9, 3, 0)));

            return new PetTrackDbContext(optionsBuilder.Options);
        }
    }
}
