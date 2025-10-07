namespace Grocery.App.Views;

public partial class ProductCategoriesView : ContentPage
{
	public ProductCategoriesView(ProductCategoriesView viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}