using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookingSystem.Infrastructure.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BookingSystemDb;User Id=sa;Password=MyStrongPass123!;TrustServerCertificate=True;");

        return new ApplicationDbContext(optionsBuilder.Options)
        {
            Properties = null!,
            Bookings = null!
        };
    }
}
