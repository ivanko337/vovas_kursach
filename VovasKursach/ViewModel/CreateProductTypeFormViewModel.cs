using System;
using System.Windows;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;

namespace VovasKursach.ViewModel
{
    public class CreateProductTypeFormViewModel : ViewModelBase
    {
        public ProductType NewType { get; set; }

        public ICommand CreateTypeCommand
        {
            get
            {
                return new Command(CreateType, CanCreateType);
            }
        }

        public CreateProductTypeFormViewModel()
        {
            NewType = new ProductType();
        }

        private void CreateType(object parameter)
        {
            using (var context = new KursachDBContext())
            {
                context.ProductsTypes.Add(NewType);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ((Window)parameter).Close();
                }
            }
        }

        private bool CanCreateType(object parameter)
        {
            return NewType != null && !string.IsNullOrEmpty(NewType.TypeName);
        }
    }
}
