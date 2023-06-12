using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ALevelSample.Models;
using ALevelSample.Services.Abstractions;

namespace ALevelSample;

public class App
{
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;

    public App(
        IUserService userService,
        IOrderService orderService,
        IProductService productService)
    {
        _userService = userService;
        _orderService = orderService;
        _productService = productService;
    }

    public async Task Start()
    {
        var firstName = "first name";
        var lastName = "last name";

        var userId = await _userService.AddUserAsync(firstName, lastName);
        var userId2 = await _userService.AddUserAsync(firstName, lastName);
        var userId3 = await _userService.AddUserAsync(firstName, lastName);

        var user = await _userService.GetUserAsync(userId2);

        var newFirstName = "New first name";
        var newLastName = "New last name";
        var isChangedName = await _userService.UpdateUserAsync(user.Id, newFirstName, newLastName);

        var isDeletedName = await _userService.DeleteUserAsync(userId3);

        List<int> productId = new List<int>();

        for (int i = 0; i < 100; i++)
        {
            productId.Add(await _productService.AddProductAsync($"product{i}", new Random().Next(15, 115)));
        }

        var isUpdatedProduct = await _productService.UpdateProductAsync(productId[3], "Changed product", 13);

        var isDeletedProduct = await _productService.DeleteProductAsync(productId[4]);

        var order1 = await _orderService.AddOrderAsync(userId, new List<OrderItem>()
        {
            new OrderItem()
            {
                Count = 3,
                ProductId = productId[1]
            },

            new OrderItem()
            {
                Count = 6,
                ProductId = productId[2]
            },
        });

        var order2 = await _orderService.AddOrderAsync(userId, new List<OrderItem>()
        {
            new OrderItem()
            {
                Count = 1,
                ProductId = productId[1]
            },

            new OrderItem()
            {
                Count = 9,
                ProductId = productId[2]
            },
        });

        var userOrder = await _orderService.GetOrderByUserIdAsync(userId);

        var isDeleteSucces = await _orderService.DeleteOrderAsync(order2);

        var productsFilterByName = _productService.PagingWithNameFilter(4, "product");
        var productsFilterByPrice = _productService.PagingWithPriceFilter(2, 20, 80);
        var productsFilterByPriceAndName = _productService.PagingWithNameAndPriceFilter(0, 30, 60, "product");
    }
}