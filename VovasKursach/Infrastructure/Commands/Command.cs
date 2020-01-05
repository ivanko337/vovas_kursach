﻿using System;
using System.Windows.Input;

namespace VovasKursach.Infrastructure.Commands
{
    public class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Func<object, bool> canExecute;
        private Action<object> execute;

        public Command(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            if (execute != null)
            {
                execute(parameter);
            }
        }
    }
}
