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
        public ObservableCollection<Product> Products
        {
            get
            {
                using (var context = new DBContext())
                {
                    var query = from p in context.Products select p;

                    return new ObservableCollection<Product>(query);
                }
            }
        }
    }
}
