using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GNB.Data
{
    public class GNBDbContext : IdentityDbContext
    {
        public GNBDbContext()
        {
        }

        public GNBDbContext(DbContextOptions<GNBDbContext> options)
            : base(options)
        {
        }
    }
}
