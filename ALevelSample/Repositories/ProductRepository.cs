using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALevelSample.Data;
using ALevelSample.Data.Entities;
using ALevelSample.Repositories.Abstractions;
using ALevelSample.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ALevelSample.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.DbContext;
    }

    public async Task<int> AddProductAsync(string name, double price)
    {
        var product = new ProductEntity()
        {
            Name = name,
            Price = price
        };

        var result = await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<ProductEntity?> GetProductAsync(int id)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> UpdateProductAsync(int id, string newName, double newPrice)
    {
        var product = await GetProductAsync(id);

        if (product == null)
        {
            return false;
        }

        product.Price = newPrice;
        product.Name = newName;

        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await GetProductAsync(id);

        if (product == null)
        {
            return false;
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public IReadOnlyList<ProductEntity> Filter<TKey>(Expression<Func<ProductEntity, bool>> filter, Expression<Func<ProductEntity, TKey>> order, int skip, int take)
    {
        return _dbContext.Products.Where(filter).Skip(skip).Take(take).OrderBy(order).ToList();
    }
}