using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Numarataj.Entity.Entities;

namespace Numarataj.DataAccess.Context
{
    public class NumaratajDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public NumaratajDbContext(DbContextOptions<NumaratajDbContext> options) : base(options)
        {
        }

        public DbSet<AdresTespit> AdresTespit { get; set; }
        public DbSet<OzelIsyeri> OzelIsyeri { get; set; }
        public DbSet<ResmiKurum> ResmiKurum { get; set; }
        public DbSet<SahaCalismasi> SahaCalismasi { get; set; }
        public DbSet<YeniBina> YeniBina { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Diğer özel konfigürasyonlar burada
        }
    }

}
