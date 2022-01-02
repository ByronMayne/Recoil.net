using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskList.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Predicate<object?>? m_canExecute;
        private readonly Action<object?> m_execute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute) : this(null, execute)
        { }

        public RelayCommand(Predicate<object?>? canExecute, Action<object?> execute)
        {
            ArgumentNullException.ThrowIfNull(execute);


            m_canExecute = canExecute;
            m_execute = execute;
        }

        public bool CanExecute(object? parameter)
            => m_canExecute != null 
                ? m_canExecute(parameter) 
                : true;


        public void Execute(object? parameter)
            => m_execute(parameter);
    }
}
