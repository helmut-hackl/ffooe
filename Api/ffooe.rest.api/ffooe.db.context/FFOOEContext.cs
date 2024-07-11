using ffooe.db.entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ffooe.db.context
{
    public class FFOOEContext : DbContext
    {
        public DbSet<M_Client> M_Clients { get; set; }
        public DbSet<M_User> M_Users { get; set; }
        public FFOOEContext() { } // This one
        public FFOOEContext(DbContextOptions options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<M_Client>().ToTable("M_Client", "dbo"); //, tb => tb.HasTrigger("AnyTrigger")).HasKey(k => k.Eid);

            modelBuilder.Entity<M_User>().ToTable("M_User", "dbo").HasKey(k => k.UserName);
        }
    }
}
