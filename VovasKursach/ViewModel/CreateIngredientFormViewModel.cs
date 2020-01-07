using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VovasKursach.Infrastructure.Commands;
using System.Windows.Input;
using VovasKursach.View;
using System.Windows;

namespace VovasKursach.ViewModel
{
    public class CreateIngredientFormViewModel : ViewModelBase
    {
        public Ingredient NewIngredient { get; set; }

        public ObservableCollection<IngredientType> IngredientsTypes
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.IngredientsTypes;

                    return new ObservableCollection<IngredientType>(query);
                }
            }
        }

        public ObservableCollection<Unit> Units
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.Units;

                    return new ObservableCollection<Unit>(query);
                }
            }
        }

        public ICommand CreateIngredientTypeCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    CreateIngredientTypeForm form = new CreateIngredientTypeForm();
                    form.ShowDialog();

                    OnProperyChanged(nameof(IngredientsTypes));
                });
            }
        }

        public ICommand CreateUnitCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    CreateUnitForm form = new CreateUnitForm();
                    form.ShowDialog();

                    OnProperyChanged(nameof(Units));
                });
            }
        }

        public ICommand CreateIngredientCommand
        {
            get
            {
                return new Command(CreateIngredient, CanCreateIngredient);
            }
        }

        private void CreateIngredient(object parameter)
        {
            using (var context = new KursachDBContext())
            {
                context.Ingredients.Add(NewIngredient);

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

        private bool CanCreateIngredient(object parameter)
        {
            return NewIngredient.IngredientType != null &&
                !string.IsNullOrEmpty(NewIngredient.Name) &&
                NewIngredient.Unit != null;
        }
    }
}
