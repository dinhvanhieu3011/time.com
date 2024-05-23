using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.DB.Classes.DB
{
    public class AppDbContext : DbContext
    {
        const string DBName = "booking.db";

        public DbSet<Users> Users { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<ChannelYoutubes> ChannelYoutubes { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Settings> Settings { get; set; }

#if UnitTest
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source=booking.test.db");
#else
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DBName}");
#endif

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
              .HasKey(p => new { p.UserId });
            modelBuilder.Entity<Settings>()
              .HasKey(p => new { p.SettingsId });

        }

        public bool CleanDB()
        {
            try
            {
                using var db = new AppDbContext();


                foreach (var item in db.Settings)
                {
                    db.Remove(item);
                    db.SaveChanges();
                }


                foreach (var item in db.Users)
                {
                    db.Remove(item);
                    db.SaveChanges();
                }
                return true;
            }
            catch { return false; }
        }

        public static bool DefaultData()
        {
            try
            {
                using var db = new AppDbContext();

                if (!db.Users.Any())
                {
                    _ = db.Add(new Users() { Username = "admin", Role = 0, Email = "admin@noreply.com", Password = "admin", Registered = DateTime.Now });
                    _ = db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
    }
}
