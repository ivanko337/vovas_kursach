using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VovasKursach.ViewModel
{
    public class SelectedProduct
    {
        public string Name { get; set; }
        public List<IngridientProduct> IngredientsProducts { get; set; }
        public string ProductImagePath { get; set; }
    }

    public class ProductsListFormViewModel : ViewModelBase
    {
        public static ProductsListFormViewModel Instance;

        public ProductsListFormViewModel()
        {
            Instance = this;
        }

        public ObservableCollection<SelectedProduct> Products
        {
            get
            {
                using (var context = new DatabaseContext())
                {
                    var query = from p in context.Products
                                select new SelectedProduct
                                {
                                    Name = p.Name,
                                    IngredientsProducts = p.IngridientsProducts.ToList(),
                                    ProductImagePath = p.ProductImagePath
                                };

                    return new ObservableCollection<SelectedProduct>(query);
                }
            }
        }

        public void OnProductsUpdate()
        {
            OnProperyChanged(nameof(Products));
        }
    }
}
