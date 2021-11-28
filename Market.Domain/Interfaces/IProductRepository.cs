using Market.Domain.Entities;
using System.Collections.Generic;

namespace Market.Domain.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
    }
}
