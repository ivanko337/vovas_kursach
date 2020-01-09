using System;
using System.Linq;
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

        public ICommand CreateProductCommand
        {
            get
            {
                return new Command(CreateProduct);
            }
        }

        public ICommand DeleteProductCommand
        {
            get
            {
                return new Command(DeleteProduct);
            }
        }

        private void CreateProduct(object parameter)
        {
            try
            {
                AddProductForm form = new AddProductForm();
                form.ShowDialog();

                OnProperyChanged(nameof(Products));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void DeleteProduct(object parameter)
        {
            Product product = parameter as Product;

            using (var context = new KursachDBContext())
            {
                var deleteEntity = context.Products.FirstOrDefault((p) => p.Id == product.Id);
                context.Products.Remove(deleteEntity);

                try
                {
                    context.SaveChanges();

                    OnProperyChanged(nameof(Products));
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
