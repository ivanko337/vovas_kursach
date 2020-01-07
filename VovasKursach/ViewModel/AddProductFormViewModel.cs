using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;

namespace VovasKursach.ViewModel
{
    public class AddProductFormViewModel : ViewModelBase
    {
        public Product Product { get; set; }

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
                        context.Products.Add(this.Product);

                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    }
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

                    OnProperyChanged(nameof(Product));
                });
            }
        }

        public ICommand CreateProductType
        {
            get
            {
                return new Command((obj) =>
                {
                    // надо допилякать форму
                });
            }
        }

        public AddProductFormViewModel()
        {
            Product = new Product();
        }
    }
}
