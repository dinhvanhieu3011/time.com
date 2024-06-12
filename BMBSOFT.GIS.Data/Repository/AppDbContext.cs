using BASE.Entity.DexTrack;
using BASE.Entity.IdentityAccess;
using BASE.Entity.LogHistory;
using BASE.Entity.SecurityMatrix;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BASE.Data.Repository
{
    public class AppDbContext : IdentityDbContext<User, Role, string, UserClaim
        , UserRole, IdentityUserLogin<string>, RoleClaim, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public override DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserTokens> UserTokens { get; set; }
        public DbSet<BASE.Entity.SecurityMatrix.SecurityMatrix> SecurityMatrix { get; set; }
        public DbSet<Entity.SecurityMatrix.Action> Action { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<LogHistoryEntity> LogHistoryEntities { get; set; }
        public DbSet<ChannelYoutubes> ChannelYoutubes { get; set; }
        public DbSet<BASE.Entity.DexTrack.Users> Userss { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<UserAction> UserActions { get; set; }
        public DbSet<Videos> Videos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<UserClaim>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserClaims).HasForeignKey(e => e.UserId);

            modelBuilder.Entity<RoleClaim>()
                .HasOne(e => e.Role)
                .WithMany(e => e.RoleClaims).HasForeignKey(e => e.RoleId);

            modelBuilder.Entity<BASE.Entity.SecurityMatrix.SecurityMatrix>()
                .HasOne(e => e.Action)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.ActionId);
            modelBuilder.Entity<BASE.Entity.SecurityMatrix.SecurityMatrix>()
                .HasOne(e => e.Screen)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.ScreenId);
            modelBuilder.Entity<BASE.Entity.SecurityMatrix.SecurityMatrix>()
                .HasOne(e => e.Role)
                .WithMany(e => e.SecurityMatrices)
                .HasForeignKey(e => e.RoleId);

            modelBuilder.Seed();
        }
    }
}


