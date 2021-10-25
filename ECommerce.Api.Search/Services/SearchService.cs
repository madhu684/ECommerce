using ECommerce.Api.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResult)> SearchAsync(int customerId)
        {
            var ordersResult = await orderService.GetOrdersAsync(customerId);
            var productsResult = await productService.GetProductAsync();
            var customersResult = await customerService.GetCustomersAsync();

            foreach (var order in ordersResult.orders)
            {
                order.CustomerName = customersResult.IsSucces ?
                    customersResult.customers.FirstOrDefault(p => p.Id == order.CustomerId)?.Name :
                    "Customer Name Not Available";

                order.Address = customersResult.IsSucces ?
                    customersResult.customers.FirstOrDefault(p => p.Id == order.CustomerId)?.Address :
                    "Customer Address Not Available";

                foreach (var item in order.Items)
                {
                    item.ProductName = productsResult.IsSuccess ?
                        productsResult.products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                        "Product Name Not Available";
                }
            }
            if(ordersResult.IsSucces)
            {
                var result = new
                {
                    Orders = ordersResult.orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
