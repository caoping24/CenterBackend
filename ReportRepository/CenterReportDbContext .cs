using CenterReport.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace CenterReport.Repository
{
    public class CenterReportDbContext : DbContext
    {
        public DbSet<SourceData> SourceDatas => Set<SourceData>();
        public DbSet<HourlyDataStatistic> HourlyDataStatistics => Set<HourlyDataStatistic>();
        public CenterReportDbContext(DbContextOptions<CenterReportDbContext> options)
               : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// todo：目前只用对单个表实体进行软删除过滤
            //modelBuilder.Entity<Report>().HasQueryFilter(p => !p.IsDelete);
        }
    }
}
