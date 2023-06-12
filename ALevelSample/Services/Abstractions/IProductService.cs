using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Data.Entities;
using ALevelSample.Models;

namespace ALevelSample.Services.Abstractions;

public interface IProductService
{
    Task<int> AddProductAsync(string name, double price);
    Task<Product> GetProductAsync(int id);
    Task<bool> UpdateProductAsync(int id, string newName, double newPrice);
    Task<bool> DeleteProductAsync(int id);
    IReadOnlyList<ProductEntity> PagingWithPriceFilter(int page, double minPrice, double maxPrice);
    IReadOnlyList<ProductEntity> PagingWithNameFilter(int page, string name);
    IReadOnlyList<ProductEntity> PagingWithNameAndPriceFilter(int page, double minPrice, double maxPrice, string name);
}