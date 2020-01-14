using System;
//using System.Data.Entity;
using System.Data.Entity.Include;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;
using VovasKursach.Infrastructure.Services;

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

        public ICommand ProductDetailsCommand
        {
            get
            {
                return new Command(ProductDetails);
            }
        }

        public ICommand CreateReportCommand
        {
            get
            {
                return new Command(CreateReport);
            }
        }

        public ICommand EditProductCommand
        {
            get
            {
                return new Command(EditProduct);
            }
        }

        private void ProductDetails(object parameter)
        {
            Product p = parameter as Product;

            if (p == null)
            {
                return;
            }

            ProductDetailsForm form = new ProductDetailsForm(p);
            form.ShowDialog();
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

            if (product == null)
            {
                System.Windows.MessageBox.Show("Выберите элемент для удаления!", "WARNING", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

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

        private void EditProduct(object parameter)
        {
            Product productToEdit = parameter as Product;

            EditProductForm form = new EditProductForm();

            using (var context = new KursachDBContext())
            {
                ((EditProductFormViewModel)form.DataContext).Product = 
                    context.Products
                    .Include("IngredientsProducts")
                    .Include("IngredientsProducts.Ingredient")
                    .Include("IngredientsProducts.Ingredient.Unit")
                    .Include(p => p.ProductType)
                    .FirstOrDefault(p => p.Id == productToEdit.Id);
            }

            if (form.ShowDialog().Value)
            {
                OnProperyChanged(nameof(Products));
            }
        }

        private void CreateReport(object parameter)
        {
            var dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            dialog.RestoreDirectory = true;
            string path = "";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.FileName;
            }
            else
            {
                return;
            }

            using (var context = new KursachDBContext())
            {
                var service = new ExcelService();
                var query = from p in context.Products
                            select p;

                try
                {
                    service.WriteToExcel(path, query);

                    System.Windows.MessageBox.Show("All done!");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
