using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ALevelSample.Data.Entities;

namespace ALevelSample.Repositories.Abstractions;

public interface IProductRepository
{
    Task<int> AddProductAsync(string name, double price);
    Task<ProductEntity?> GetProductAsync(int id);
    Task<bool> UpdateProductAsync(int id, string name, double price);
    Task<bool> DeleteProductAsync(int id);
    IReadOnlyList<ProductEntity> Filter<TKey>(Expression<Func<ProductEntity, bool>> filter, Expression<Func<ProductEntity, TKey>> order, int skip, int take);
}