namespace CenterReport.Repository
{
    public interface IUnitOfWork1
    {
        Task<int> SaveChangesAsync();
    }
}
