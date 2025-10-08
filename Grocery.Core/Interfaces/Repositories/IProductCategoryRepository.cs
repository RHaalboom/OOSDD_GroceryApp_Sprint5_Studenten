using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Repositories
{
    public interface IProductCategoryRepository
    {
        List<ProductCategory> GetAll();
        List<ProductCategory> GetByAllOnCaterogyId(int id);
        ProductCategory Add(ProductCategory item);
    }
}
