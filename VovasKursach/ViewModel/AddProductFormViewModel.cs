using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VovasKursach.ViewModel
{
    public class AddProductFormViewModel : ViewModelBase
    {
        public Product Product { get; set; }

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
    }
}
