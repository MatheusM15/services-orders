using DOMAIN.Base;
using INFRA.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace INFRA.Repositories.Common;

public class Repository<T> : IRepository<T> where T : AggregateRoot
{
    public DbSet<T> _dbSet { get; private set; }
    public OrderContext _context { get; set; }

    public Repository(OrderContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
        => await _dbSet.Where(predicate).AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void SaveChangesAsync()
        => _context.SaveChanges();

    public async Task<T> GetByIdAsync(Guid id)
        => await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();

    public void Update(T entity)
        => _dbSet.Update(entity);
}
