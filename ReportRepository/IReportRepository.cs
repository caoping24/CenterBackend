using Microsoft.EntityFrameworkCore;

namespace CenterReport.Repository
{
    public interface IReportRepository<T> where T : class
    {
        IQueryable<T> db { get; }
        Task<List<T>> GetByDataTimeAsync(DateTime DateTime);
        Task<List<T>> GetByDataTimeAsync(DateTime start, DateTime end);
        Task<T?> GetByIdAsync(long id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteByIdAsync(long id);
    }
}
