using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
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
        partial void OnCategoryChanged(Category value)
        {
            LoadProductCategoriesByCategory(value);
        }
    }
}
