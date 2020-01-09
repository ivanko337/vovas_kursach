using System;
using System.Windows;
using System.Windows.Input;
using VovasKursach.Infrastructure.Commands;

namespace VovasKursach.ViewModel
{
    public class CreateUnitFormViewModel : ViewModelBase
    {
        public Unit NewUnit { get; set; }

        public ICommand CreateUnitCommand
        {
            get
            {
                return new Command(CreateUnit);
            }
        }

        private void CreateUnit(object parameter)
        {
            if (string.IsNullOrEmpty(NewUnit.Name))
            {
                return;
            }

            using (var context = new KursachDBContext())
            {
                context.Units.Add(NewUnit);

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

        public CreateUnitFormViewModel()
        {
            NewUnit = new Unit();
        }
    }
}
