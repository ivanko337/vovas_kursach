using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;

namespace VovasKursach.ViewModel
{
    public class AddProductFormViewModel : ViewModelBase
    {
        public Product Product { get; set; } = new Product();

        public List<ProductType> ProductTypes
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.ProductsTypes;

                    return query.ToList();
                }
            }
        }

        public ICommand AddProductCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    System.Windows.MessageBox.Show(Product.ProductType.TypeName);
                });
            }
        }
    }
}
