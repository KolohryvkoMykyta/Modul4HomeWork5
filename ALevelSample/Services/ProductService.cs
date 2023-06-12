using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Data;
using ALevelSample.Data.Entities;
using ALevelSample.Models;
using ALevelSample.Repositories.Abstractions;
using ALevelSample.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace ALevelSample.Services;

public class ProductService : BaseDataService<ApplicationDbContext>, IProductService
{
    private const int _pageSize = 20;

    private readonly IProductRepository _productRepository;
    private readonly ILogger<UserService> _loggerService;

    public ProductService(
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IProductRepository productRepository,
        ILogger<UserService> loggerService)
        : base(dbContextWrapper, logger)
    {
        _productRepository = productRepository;
        _loggerService = loggerService;
    }

    public async Task<int> AddProductAsync(string name, double price)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var id = await _productRepository.AddProductAsync(name, price);
            _loggerService.LogInformation($"Created product with Id = {id}");
            return id;
        });
    }

    public async Task<Product> GetProductAsync(int id)
    {
        var result = await _productRepository.GetProductAsync(id);

        if (result == null)
        {
            _loggerService.LogWarning($"Not founded product with Id = {id}");
            return null!;
        }

        return new Product()
        {
            Id = result.Id,
            Name = result.Name,
            Price = result.Price
        };
    }

    public async Task<bool> UpdateProductAsync(int id, string newName, double newPrice)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _productRepository.UpdateProductAsync(id, newName, newPrice);

            if (result)
            {
                _loggerService.LogInformation($"Updated product with Id = {id}");
            }

            return result;
        });
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _productRepository.DeleteProductAsync(id);

            if (result)
            {
                _loggerService.LogInformation($"Deleted product with Id = {id}");
            }

            return result;
        });
    }

    public IReadOnlyList<ProductEntity> PagingWithPriceFilter(int page, double minPrice, double maxPrice)
    {
        return _productRepository.Filter(p => p.Price > minPrice && p.Price < maxPrice, p => p.Price, page * _pageSize, _pageSize);
    }

    public IReadOnlyList<ProductEntity> PagingWithNameFilter(int page, string name)
    {
        return _productRepository.Filter(p => p.Name.Contains(name), p => p.Name, page * _pageSize, _pageSize);
    }

    public IReadOnlyList<ProductEntity> PagingWithNameAndPriceFilter(int page, double minPrice, double maxPrice, string name)
    {
        return _productRepository.Filter(p => p.Name.Contains(name) && p.Price > minPrice && p.Price < maxPrice, p => p.Price, page * _pageSize, _pageSize);
    }
}