using AutoMapper;
using ECommerce.Api.Products.Db;
using ECommerce.Api.Products.Interfaces;
using ECommerce.Api.Products.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Providers
{
    public class ProductProvider : IProoductsProvider
    {
        private readonly ProductDbContext dbContext;
        private readonly ILogger<ProductProvider> logger;
        private readonly IMapper mapper;

        public ProductProvider(ProductDbContext dbContext, ILogger<ProductProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Products.Any())
            {
                dbContext.Products.Add(new Db.Product() { Id = 1, Name = "Keyboard", Price = 20, Inventory = 100 });
                dbContext.Products.Add(new Db.Product() { Id = 2, Name = "Mouse", Price = 10, Inventory = 50 });
                dbContext.Products.Add(new Db.Product() { Id = 3, Name = "Monitor", Price = 1500, Inventory = 30 });
                dbContext.Products.Add(new Db.Product() { Id = 4, Name = "CPU", Price = 3000, Inventory = 60 });
                dbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Product> products, string ErrorMessage)> GetProductsAsync()
        {
            try
            {
                var products = await dbContext.Products.ToListAsync();
                if (products != null && products.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Product>, IEnumerable<Models.Product>>(products);

                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Product product, string ErrorMessage)> GetProductAsync(int id)
        {
            try
            {
                var product = await dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    var result = mapper.Map<Db.Product, Models.Product>(product);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
