using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productRepository = productRepository;
        }

        public List<ProductCategory> GetAll()
        {
            var productCategories = _productCategoryRepository.GetAll();
            FillService(productCategories);
            return productCategories;
        }

        public List<ProductCategory> GetByAllOnCategoryId(int id)
        {
            List<ProductCategory> productCategories = _productCategoryRepository.GetAll().Where(c => c.CategoryId == id).ToList();
            FillService(productCategories);
            return productCategories;
        }

        public ProductCategory Add(ProductCategory item)
        {
            var added = _productCategoryRepository.Add(item);
            added.Product = _productRepository.Get(added.ProductId) ?? new Product(0, "", 0, 0);
            return added;
        }

        private void FillService(List<ProductCategory> productCategory)
        {
            foreach (ProductCategory pc in productCategory)
            {
                pc.Product = _productRepository.Get(pc.ProductId) ?? new(0, "", 0, 0);
            }
        }
    }
}