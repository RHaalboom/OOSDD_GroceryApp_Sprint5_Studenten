using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    [QueryProperty(nameof(Category), nameof(Category))]
    public partial class ProductCategoriesViewModel : BaseViewModel
    {
        private readonly IProductCategoryService _productCategoryService;
        public ObservableCollection<ProductCategory> ProductCategories { get; set; } = [];
        public ObservableCollection<Product> AvailableProducts { get; set; } = [];

        private string searchText = "";

        [ObservableProperty]
        private Category category = new(0, "None");

        public ProductCategoriesViewModel(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public void LoadProductCategoriesByCategory(Category category)
        {
            ProductCategories.Clear();

            var productCategories = _productCategoryService.GetByAllOnCategoryId(category.Id);
            foreach (var pc in productCategories)
            {
                ProductCategories.Add(pc);
            }
        }

        // Populate AvailableProducts with products that are NOT listed under the selected category.
        // Deduplicates by Product.Id and applies the current searchText filter.
        private void GetAvailableProducts()
        {
            AvailableProducts.Clear();

            // Get all product-category mappings (Product is populated by the service)
            var allProductCategories = _productCategoryService.GetAll();

            var seenProductIds = new HashSet<int>();

            foreach (var pc in allProductCategories)
            {
                if (pc == null || pc.Product == null) continue;

                // Skip products that belong to the selected category
                if (pc.CategoryId == Category.Id) continue;

                var product = pc.Product;

                // Apply search filter (case-insensitive). If searchText is empty, include all.
                if (!string.IsNullOrWhiteSpace(searchText) &&
                    !product.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (seenProductIds.Add(product.Id))
                {
                    AvailableProducts.Add(product);
                }
            }
        }

        partial void OnCategoryChanged(Category value)
        {
            LoadProductCategoriesByCategory(value);
            GetAvailableProducts();
        }

        [RelayCommand]
        public void PerformSearch(string searchText)
        {
            this.searchText = searchText ?? "";
            GetAvailableProducts();
        }

        [RelayCommand]
        public void AddProductToCategory(Product product)
        {
            if (product == null) return;
            if (Category == null) return;

            var mapping = new ProductCategory(0, Category.Name, product.Id, Category.Id);
            _productCategoryService.Add(mapping);

            LoadProductCategoriesByCategory(Category);
            GetAvailableProducts();
        }
    }
}
