using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;

namespace VovasKursach.ViewModel
{
    public class AddProductFormViewModel : ViewModelBase
    {
        private Product product;
        public Product Product
        {
            get
            {
                return product;
            }
            set
            {
                product = value;
                OnProperyChanged(nameof(Product));
            }
        }

        public ObservableCollection<ProductType> ProductTypes
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.ProductsTypes;

                    return new ObservableCollection<ProductType>(query);
                }
            }
        }

        public ObservableCollection<IngridientProduct> IngridientsProducts
        {
            get
            {
                return new ObservableCollection<IngridientProduct>(Product.IngredientsProducts);
            }
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    using (var context = new KursachDBContext())
                    {
                        AddProductForm form = obj as AddProductForm;

                        context.Products.Add(this.Product);

                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            form.Close();
                        }
                    }
                }, (obj) =>
                {
                    Product product = this.Product as Product;

                    return product != null &&
                            product.IngredientsProducts.Count != 0 &&
                            !string.IsNullOrEmpty(product.Name) &&
                            product.ProductType != null &&
                            !string.IsNullOrEmpty(product.RecipeText);
                });
            }
        }

        public ICommand SelectProductPicture
        {
            get
            {
                return new Command((obj) =>
                {
                    using (var dialog = new OpenFileDialog())
                    {
                        dialog.Filter = "jpg files (*.jpg)|*.jpg|png files (*.png)|*.png|bmp files (*.bmp)|*.bmp";
                        dialog.RestoreDirectory = true;

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            Product.ProductImagePath = dialog.FileName;

                            OnProperyChanged(nameof(Product));
                        }
                    }
                });
            }
        }

        public ICommand AddIngredient
        {
            get
            {
                return new Command((obj) =>
                {
                    AddIngredientForm form = new AddIngredientForm();
                    ((AddIngredientViewModel)form.DataContext).NewProduct = this.Product;
                    form.ShowDialog();

                    OnProperyChanged(nameof(IngridientsProducts));
                });
            }
        }

        public ICommand DeleteIngredients
        {
            get
            {
                return new Command((obj) =>
                {
                    System.Windows.Controls.ListView listView = obj as System.Windows.Controls.ListView;

                    foreach (var item in listView.SelectedItems)
                    {
                        Product.IngredientsProducts.Remove(item as IngridientProduct);
                    }

                    OnProperyChanged(nameof(IngridientsProducts));
                });
            }
        }

        public ICommand CreateProductType
        {
            get
            {
                return new Command((obj) =>
                {
                    CreateProductTypeForm form = new CreateProductTypeForm();
                    form.ShowDialog();

                    OnProperyChanged(nameof(ProductTypes));
                });
            }
        }

        public AddProductFormViewModel()
        {
            Product = new Product();
        }
    }
}
