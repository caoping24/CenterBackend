namespace CenterReport.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CenterReportDbContext _context;
        public UnitOfWork(CenterReportDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
