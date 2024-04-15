using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BoostifySolution.Entities;
using boostifysolution1.Entities;

namespace BoostifySolution.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser<int>>().ToTable("UserLogins").Property(p => p.Id).HasColumnName("UserId");
        builder.Entity<IdentityRole<int>>().ToTable("UserRoleTypes").Property(rt => rt.Id).HasColumnName("UserRoleTypeId");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles").Property(p => p.UserId).HasColumnName("UserId");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("UserClaimId");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("UserRoleClaims").Property(p => p.Id).HasColumnName("UserRoleClaimId");
        builder.Entity<IdentityUserLogin<int>>().ToTable("User3rdPartyLogin").Property(p => p.UserId).HasColumnName("UserId");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens").Property(p => p.UserId).HasColumnName("UserId");

        builder.Entity<LeaderAdminStaffs>()
        .HasKey(x => new { x.AdminStaffId, x.AssociatedAdminStaffId });
    }

    public DbSet<AdminStaffs> AdminStaffs { get; set; }

    public DbSet<ProductReviews> ProductReviews { get; set; }

    public DbSet<WalletTransactions> WalletTransactions { get; set; }

    public DbSet<Users> Users { get; set; }

    public DbSet<Tasks> Tasks { get; set; }

    public DbSet<UserTasks> UserTasks { get; set; }

    public DbSet<UserLeads> UserLeads { get; set; }

    public DbSet<LeaderAdminStaffs> LeaderAdminStaffs { get; set; }

    public DbSet<Exceptions> Exceptions { get; set; }

    public DbSet<SupportItems> SupportItems { get; set; }
}