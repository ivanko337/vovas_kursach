using System.Collections.ObjectModel;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;

namespace VovasKursach.ViewModel
{
    public class ProductsListFormViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.Products.Include("IngredientsProducts").Include("IngredientsProducts.Ingredient");

                    return new ObservableCollection<Product>(query);
                }
            }
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new Command(AddProduct);
            }
        }

        public ICommand EditProductCommand
        {
            get
            {
                return new Command(EditProduct);
            }
        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return new Command(DeleteProduct);
            }
        }

        private void AddProduct(object parameter)
        {
            AddProductForm form = new AddProductForm();
            form.ShowDialog();

            OnProperyChanged(nameof(Products));
        }

        private void EditProduct(object parameter)
        {

        }

        private void DeleteProduct(object parameter)
        {

        }
    }
}
