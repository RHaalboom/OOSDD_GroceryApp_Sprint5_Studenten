using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Data.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {

        private readonly List<ProductCategory> productCategoryList;

        public ProductCategoryRepository()
        {
            productCategoryList = [
                new ProductCategory(1, "Zuivel", 1, 1),
                new ProductCategory(2, "Zuivel", 2, 1),
                new ProductCategory(3, "Granen", 3, 2),
                new ProductCategory(4, "Granen", 4, 2),
                new ProductCategory(5, "Bier", 5, 3)

            ];
        }
        public List<ProductCategory> GetAll()
        {
            return productCategoryList;
        }

        public List<ProductCategory> GetByAllOnCaterogyId(int id)
        {
            return productCategoryList.Where(c => c.CategoryId == id).ToList();
        }

        public ProductCategory Add(ProductCategory item)
        {
            productCategoryList.Add(item);
            return item;
        }
    }
}