using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IProductCategoryService
    {
        List<ProductCategory> GetAll();
        List<ProductCategory> GetByAllOnCategoryId(int id);
    }
}
