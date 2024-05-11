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
        public DbSet<Books> Books { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Channels> Channels { get; set; }
        public DbSet<ReservedBook> ReservedBook { get; set; }
        public DbSet<BooksCopies> BooksCopies { get; set; }

        public DbSet<CookaAccounts> CookaAccounts { get; set; }
        public DbSet<ChannelYoutubes> ChannelYoutubes { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<News> News { get; set; }

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
            modelBuilder.Entity<Channels>()
              .HasKey(p => new { p.ChannelId });
            modelBuilder.Entity<Books>()
              .HasKey(p => new { p.BookId });
            modelBuilder.Entity<ReservedBook>()
              .HasKey(p => new { p.ReservedBookId });
            modelBuilder.Entity<BooksCopies>()
              .HasKey(p => new { p.BooksCopiesId });
            modelBuilder.Entity<Settings>()
              .HasKey(p => new { p.SettingsId });

        }

        public bool CleanDB()
        {
            try
            {
                using var db = new AppDbContext();

                foreach (var item in db.ReservedBook)
                {
                    db.Remove(item);
                    db.SaveChanges();
                }

                foreach (var item in db.BooksCopies)
                {
                    db.Remove(item);
                    db.SaveChanges();
                }

                foreach (var item in db.Settings)
                {
                    db.Remove(item);
                    db.SaveChanges();
                }

                foreach (var item in db.Books)
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

                    _ = db.Add(new Books() { Name = "Harry Potter 1", Author = "J.K. Rowling", PublicationYear = 1997, Registered = DateTime.Now });
                    _ = db.SaveChanges();
                    _ = db.Add(new Books() { Name = "Harry Potter 2", Author = "J.K. Rowling", PublicationYear = 1998, Registered = DateTime.Now });
                    _ = db.SaveChanges();
                    _ = db.Add(new Books() { Name = "Harry Potter 3", Author = "J.K. Rowling", PublicationYear = 1999, Registered = DateTime.Now });
                    _ = db.SaveChanges();
                    _ = db.Add(new Books() { Name = "Harry Potter 4", Author = "J.K. Rowling", PublicationYear = 2000, Registered = DateTime.Now });
                    _ = db.SaveChanges();
                    _ = db.Add(new Books() { Name = "Harry Potter 5", Author = "J.K. Rowling", PublicationYear = 2003, Registered = DateTime.Now });
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
