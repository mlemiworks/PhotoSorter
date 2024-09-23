using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoSorter.Commands
{
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> _commands;

        public CompositeCommand(params ICommand[] commands)
        {
            _commands = commands.ToList();
        }

        public bool CanExecute(object parameter)
        {
            return _commands.All(c => c.CanExecute(parameter));
        }

        public void Execute(object parameter)
        {
            foreach (var command in _commands)
            {
                command.Execute(parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
