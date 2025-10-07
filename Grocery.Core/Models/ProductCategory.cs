using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Models
{
    public class ProductCategory
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        public ProductCategory(int id, string name, int productId, int categoryId)
        {
            ProductId = productId;
            CategoryId = categoryId;
        }

        public Product Product { get; set; } = new (0, "None", 0);
    }
}