using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProoductsProvider prooductsProvider;

        public ProductsController(IProoductsProvider prooductsProvider)
        {
            this.prooductsProvider = prooductsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductAsync()
        {
            var result = await prooductsProvider.GetProductsAsync();

            if (result.IsSuccess)
            {
                return Ok(result.products);
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var result = await prooductsProvider.GetProductAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.product);
            }
            return NotFound();
        }
    }
}
