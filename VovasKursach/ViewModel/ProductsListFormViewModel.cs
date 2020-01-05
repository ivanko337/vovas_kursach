using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace VovasKursach.ViewModel
{
    public class ProductsListFormViewModel : ViewModelBase
    {
        public static ProductsListFormViewModel Instance;

        public ProductsListFormViewModel()
        {
            Instance = this;
        }

        public List<Product> Products
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.Products.Include("IngredientsProducts").Include("IngredientsProducts.Ingredient");

                    return query.ToList();
                }
            }
        }

        public void OnProductsUpdate()
        {
            OnProperyChanged(nameof(Products));
        }
    }
}
