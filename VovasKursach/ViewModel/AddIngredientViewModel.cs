using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;
using VovasKursach.View;
using System.Linq;

namespace VovasKursach.ViewModel
{
    public class AddIngredientViewModel : ViewModelBase
    {
        public Product NewProduct { get; set; }

        public IngridientProduct CurrIngredientProduct { get; set; }

        public Ingredient CurrIngredient
        {
            get
            {
                return CurrIngredientProduct.Ingredient;
            }
            set
            {
                using (var context = new KursachDBContext())
                {
                    CurrIngredientProduct.Ingredient = context.Ingredients.Include("Unit").FirstOrDefault(i => i.Id == value.Id);
                }
                
                OnProperyChanged(nameof(CurrIngredient));
                OnProperyChanged(nameof(CurrIngredientProduct));
            }
        }

        public ObservableCollection<Ingredient> Ingredients
        {
            get
            {
                using (var context = new KursachDBContext())
                {
                    var query = context.Ingredients.Include("Unit");

                    return new ObservableCollection<Ingredient>(query);
                }
            }
        }

        public ICommand AddIngredientCommand
        {
            get
            {
                return new Command(AddIngredient, CanAddIngredient);
            }
        }

        public ICommand CreateIngredientCommand
        {
            get
            {
                return new Command(CreateIngredient);
            }
        }

        public AddIngredientViewModel()
        {
            using (var context = new KursachDBContext())
            {
                CurrIngredientProduct = context.IngridientsProducts.Create(); // FirstOrDefault();
            }
        }

        private void CreateIngredient(object parameter)
        {
            try
            {
                CreateIngredientForm form = new CreateIngredientForm();
                form.ShowDialog();

                OnProperyChanged(nameof(Ingredients));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddIngredient(object parameter)
        {
            return CurrIngredientProduct.Ingredient != null && CurrIngredientProduct.IngCount != 0;
        }

        private void AddIngredient(object param)
        {
            try
            {
                ((Window)param).DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
