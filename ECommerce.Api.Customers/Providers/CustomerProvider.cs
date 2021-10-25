using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomerProvider : ICustomerProvider
    {
        private readonly CustomerDbContext dbContext;
        private readonly ILogger<CustomerProvider> logger;
        private readonly IMapper mapper;

        public CustomerProvider(CustomerDbContext dbContext, ILogger<CustomerProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Daneesha Nilmini", Address = "Lion street, Hambantota" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Tharun Sandunil", Address = "Koondeniya, Dickwella" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Sachini Niwarthana", Address = "Central junction, Kalutara" });
                dbContext.SaveChanges();

            }
        }

        public async Task<(bool IsSuccess, Models.Customer customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                logger?.LogInformation("Querying Customers");
                var customer = await dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
                if (customer != null)
                {
                    logger?.LogInformation("Customer found");
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
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

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                logger?.LogInformation("Querying Customers");
                var customers = await dbContext.Customers.ToListAsync();
                if (customers != null && customers.Any())
                {
                    logger?.LogInformation($"{customers.Count} customers found");
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);

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
