using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VovasKursach.ViewModel
{
    public class SelectedIngridientProduct
    {
        public string Name { get; set; }
        public int IngCount { get; set; }
        public string UnitName { get; set; }
    }

    public class ProductDetailsFormViewModel : ViewModelBase
    {
        public Product Product { get; set; }

        public ObservableCollection<SelectedIngridientProduct> IngridientsProducts
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = from ip in context.IngridientsProducts
                                where ip.ProductId == Product.Id
                                select new SelectedIngridientProduct
                                {
                                    Name = ip.Ingredient.Name,
                                    IngCount = ip.IngCount,
                                    UnitName = ip.Ingredient.Unit.Name
                                };

                    return new ObservableCollection<SelectedIngridientProduct>(query);
                }
            }
        }

        public string ProductType
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = from p in context.Products
                                where p.Id == Product.Id
                                select p.ProductType.TypeName;

                    return query.FirstOrDefault();
                }
            }
        }

        public void OnProductChanged()
        {
            OnProperyChanged(nameof(Product));
            OnProperyChanged(nameof(IngridientsProducts));
            OnProperyChanged(nameof(ProductType));
        }
    }
}
