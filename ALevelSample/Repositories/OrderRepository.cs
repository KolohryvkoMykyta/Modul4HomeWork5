using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALevelSample.Data;
using ALevelSample.Data.Entities;
using ALevelSample.Models;
using ALevelSample.Repositories.Abstractions;
using ALevelSample.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ALevelSample.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<int> AddOrderAsync(string user, List<OrderItem> items)
    {
        var result = await _dbContext.Orders.AddAsync(new OrderEntity()
        {
            UserId = user,
            OrderItems = items.Select(s => new OrderItemEntity()
            {
                Count = s.Count,
                ProductId = s.ProductId
            }).ToList()
        });

        await _dbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<OrderEntity?> GetOrderAsync(int id)
    {
        return await _dbContext.Orders.Include(i => i.OrderItems).FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<IEnumerable<OrderEntity>?> GetOrderByUserIdAsync(string id)
    {
        return await _dbContext.Orders.Include(i => i.OrderItems).Where(f => f.UserId == id).ToListAsync();
    }

    public async Task<bool> UpdateOrderAsync(int id, string userId, List<OrderItem> newItems)
    {
        var product = _dbContext.Orders.Where(p => p.Id == id).FirstOrDefault();

        if (product == null)
        {
            return false;
        }

        product.UserId = userId;
        product.OrderItems = newItems.Select(s => new OrderItemEntity()
        {
            Count = s.Count,
            OrderId = id,
            ProductId = s.ProductId
        }).ToList();

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await GetOrderAsync(id);

        if (order == null)
        {
            return false;
        }

        _dbContext.Remove(order);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}