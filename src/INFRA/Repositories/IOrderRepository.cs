using DOMAIN;
using INFRA.Repositories.Common;
using System.Linq.Expressions;

namespace INFRA.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    public Task<TSelector> GetOrderByIdAsync<TSelector>(Guid Id, Expression<Func<Order, TSelector>> selector);
}

