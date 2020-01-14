using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;
using System.Windows.Forms;

namespace VovasKursach.ViewModel
{
    public class EditProductFormViewModel : ViewModelBase
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
                OnProperyChanged(nameof(IngridientsProducts));
                OnProperyChanged(nameof(ProductType));

            }
        }

        public ProductType ProductType
        {
            get
            {
                return Product.ProductType;
            }
            set
            {
                Product.ProductType = value;

                OnProperyChanged(nameof(ProductType));
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

        public ICommand SaveProductCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    using (var context = new KursachDBContext())
                    {
                        EditProductForm form = obj as EditProductForm;

                        Product.ProductType = context.ProductsTypes.FirstOrDefault(pt => pt.Id == Product.ProductType.Id);

                        foreach (var item in Product.IngredientsProducts)
                        {
                            // ох, костыли - моя жизнь
                            // сие дело нужно для того, чтобы ингредиенты в бд не дублировались
                            context.Entry(item.Ingredient).State = System.Data.Entity.EntityState.Modified;
                        }

                        context.Products.Add(this.Product);

                        try
                        {
                            context.SaveChanges();
                            form.DialogResult = true;
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                            form.DialogResult = false;
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

                    if (form.ShowDialog().Value)
                    {
                        IngridientProduct ip = ((AddIngredientViewModel)form.DataContext).CurrIngredientProduct;
                        ip.Product = this.Product;

                        Product.IngredientsProducts.Add(ip);

                        OnProperyChanged(nameof(IngridientsProducts));
                    }
                });
            }
        }

        public ICommand DeleteIngredient
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
    }
}
