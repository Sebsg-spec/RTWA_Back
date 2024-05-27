using Microsoft.EntityFrameworkCore;
using RTWA_Back.Controllers;
using RTWA_Back.Models;

namespace RTWA_Back.Data
{
    public class DataContext : DbContext
    {
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Package> Package => Set<Package>();

        public DbSet<PackageDetails> PackageDetails => Set<PackageDetails>();

        public DbSet<PackageHistory> PackageHistory => Set<PackageHistory>();

        public DbSet<PackageDetailsHistory> PackageDetailsHistory => Set<PackageDetailsHistory>();

        public DbSet<ACCOUNTS>ACCOUNTS => Set<ACCOUNTS>();

        public DbSet<ROLES> ROLES => Set<ROLES>();

        public DbSet<RELATIONS> RELATIONS => Set<RELATIONS>();

        public DbSet<FormControlls> FormControlls => Set<FormControlls>();


        public DbSet<EMailTable> EMailTable => Set<EMailTable>();
        public DbSet<RoleUpgrade> RoleUpgrade => Set<RoleUpgrade>();

    }
}
