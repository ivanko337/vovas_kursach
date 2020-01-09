using System;
using System.Windows;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;

namespace VovasKursach.ViewModel
{
    public class CreateIngredientTypeFormViewModel : ViewModelBase
    {
        public IngredientType NewType { get; set; }

        public ICommand CreateTypeCommand
        {
            get
            {
                return new Command(CreateType);
            }
        }

        private void CreateType(object parameter)
        {
            if (string.IsNullOrEmpty(NewType.TypeName))
            {
                return;
            }

            using (var context = new KursachDBContext())
            {
                context.IngredientsTypes.Add(NewType);

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
    }
}
