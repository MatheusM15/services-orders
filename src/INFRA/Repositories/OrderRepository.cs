using DOMAIN;
using INFRA.Context;
using INFRA.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace INFRA.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(OrderContext context) : base(context) { }

    public async Task<TSelector?> GetOrderByIdAsync<TSelector>(Guid Id, Expression<Func<Order, TSelector>> selector)
        => await _dbSet.Include(x => x.Products).Where(x => x.Id == Id).Select(selector).FirstOrDefaultAsync();
}
