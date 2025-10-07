using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.App.Views;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class CategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }
        private readonly ICategoryService _categoryService;

        public CategoriesViewModel(ICategoryService categoryService, GlobalViewModel global)
        {
            Title = "Categories";
            _categoryService = categoryService;
            Categories = new(_categoryService.GetAll());
        }

        [RelayCommand]
        public async Task SelectCategory(Category category)
        {
            Dictionary<string, object> paramater = new() { { nameof(Category), category } };
            await Shell.Current.GoToAsync($"{nameof(Views.ProductCategoriesView)}?Title={category.Name}", true, paramater);
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            Categories = new(_categoryService.GetAll());
        }

        public override void OnDisappearing() {
            base.OnDisappearing();
            Categories.Clear();
        }
    }
}
