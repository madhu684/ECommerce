using ECommerce.Api.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Interfaces
{
    public interface IProductService
    {
        Task<(bool IsSuccess, IEnumerable<Product> products, string ErrorMessage)> GetProductAsync();
    }
}
