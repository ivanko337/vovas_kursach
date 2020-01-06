using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using VovasKursach.Infrastructure.Commands;
using System.Windows.Input;

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

                });
            }
        }

        public ICommand CreateUnitCommand
        {
            get
            {
                return null;
            }
        }


    }
}
