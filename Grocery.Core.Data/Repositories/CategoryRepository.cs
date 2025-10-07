using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery.Core.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly List<Category> categories;

        public CategoryRepository() 
        { 
            categories = [
                new Category(1, "Zuivel"),
                new Category(2, "Granen" ),
                ];
        }

        public Category? Get(int id)
        {
            Category? category = categories.FirstOrDefault(c => c.Id == id);
            return category;
        }
        public List<Category> GetAll()
        {
            return categories;
        }

    }


}
