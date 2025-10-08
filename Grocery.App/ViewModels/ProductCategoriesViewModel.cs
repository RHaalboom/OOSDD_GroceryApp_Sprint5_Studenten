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

            if (Category == null) return;

            // Collect product ids that are already assigned to the selected category
            var inCategory = _productCategoryService
                .GetByAllOnCategoryId(Category.Id)
                .Where(pc => pc?.Product != null)
                .Select(pc => pc.ProductId)
                .ToHashSet();

            // Get all products from all mappings (service fills Product)
            var allProducts = _productCategoryService
                .GetAll()
                .Where(pc => pc?.Product != null)
                .Select(pc => pc.Product)
                .GroupBy(p => p.Id)
                .Select(g => g.First());

            foreach (var product in allProducts)
            {
                if (product == null) continue;

                // Exclude products that are present in the selected category
                if (inCategory.Contains(product.Id)) continue;

                // Apply search filter (case-insensitive). If searchText is empty, include all.
                if (!string.IsNullOrWhiteSpace(searchText) &&
                    !product.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                AvailableProducts.Add(product);
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

            // Prevent adding the same product twice to the currently selected category
            if (ProductCategories.Any(pc => pc.ProductId == product.Id && pc.CategoryId == Category.Id))
                return;

            var mapping = new ProductCategory(0, Category.Name, product.Id, Category.Id);
            var added = _productCategoryService.Add(mapping);

            if (added != null)
            {
                ProductCategories.Add(added);

                var toRemove = AvailableProducts.FirstOrDefault(p => p.Id == product.Id);
                if (toRemove != null)
                    AvailableProducts.Remove(toRemove);
            }
        }
    }
}
