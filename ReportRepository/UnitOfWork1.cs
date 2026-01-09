namespace CenterReport.Repository
{
    public class UnitOfWork1 : IUnitOfWork1
    {
        private readonly CenterReportDbContext _context;
        public UnitOfWork1(CenterReportDbContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
