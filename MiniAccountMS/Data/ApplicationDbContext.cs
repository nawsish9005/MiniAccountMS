using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniAccountMS.Models;

namespace MiniAccountMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherItem> VoucherItems { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Table names and relationships if needed
            builder.Entity<ChartOfAccount>().ToTable("ChartOfAccounts");

            builder.Entity<Voucher>().ToTable("Vouchers");
            builder.Entity<VoucherItem>().ToTable("VoucherItems");
            builder.Entity<VoucherItem>()
                   .HasOne<Voucher>()
                   .WithMany(v => v.Items)
                   .HasForeignKey(vi => vi.VoucherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>().ToTable("RolePermissions");
        }
    }
}
