using ffooe.db.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ffooe.db.context
{
    public class FFOOEContext : DbContext
    {
        public DbSet<M_Client> M_Clients { get; set; }
        public FFOOEContext() { } // This one
        public FFOOEContext(DbContextOptions options) : base(options)
        {
            //DatabaseState = options.DatabaseState;
            //DatabaseInfo = options.DatabaseInfo;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=FFOOE;Integrated Security=SSPI;TrustServerCertificate=true");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Client>().ToTable("M_Client", "dbo"); //, tb => tb.HasTrigger("AnyTrigger")).HasKey(k => k.Eid);
        }
    }
}
