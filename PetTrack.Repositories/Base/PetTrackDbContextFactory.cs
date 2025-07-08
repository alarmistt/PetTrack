using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PetTrack.Repositories.Base
{
    public class PetTrackDbContextFactory : IDesignTimeDbContextFactory<PetTrackDbContext>
    {
        public PetTrackDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PetTrack"))
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();


            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<PetTrackDbContext>();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            return new PetTrackDbContext(optionsBuilder.Options);
        }
    }
}
