using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerControlller : ControllerBase
    {
        private readonly ICustomerProvider customerProvider;

        public CustomerControlller(ICustomerProvider customerProvider)
        {
            this.customerProvider = customerProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var result = await customerProvider.GetCustomersAsync();

            if (result.IsSuccess)
            {
                return Ok(result.customers);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(int id)
        {
            var result = await customerProvider.GetCustomerAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.customer);
            }
            return NotFound();
        }
    }
}
