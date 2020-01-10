using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VovasKursach.ViewModel;

namespace VovasKursach.View
{
    /// <summary>
    /// Interaction logic for ProductDetailsForm.xaml
    /// </summary>
    public partial class ProductDetailsForm : Window
    {
        public ProductDetailsForm(Product product)
        {
            InitializeComponent();

            ((ProductDetailsFormViewModel)this.DataContext).Product = product;
            ((ProductDetailsFormViewModel)this.DataContext).OnProductChanged();
        }
    }
}
