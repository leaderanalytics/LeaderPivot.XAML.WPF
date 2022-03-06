using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LeaderAnalytics.LeaderPivot.XAML.WPF;
public class RelayCommand : ICommand
{
    private Action execute;
    private Func<object, bool> canExecute;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action execute, Func<object, bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return this.canExecute == null || this.canExecute(parameter);
    }

    public void Execute(object? o = null)
    {
        this.execute();
    }
}

public class RelayCommand<T> : ICommand
{
    private Action<T> execute;
    private Func<object?, bool> canExecute;

    public event EventHandler CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<T> execute, Func<object?, bool> canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return this.canExecute == null || this.canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        if (parameter == null)
            throw new ArgumentNullException(nameof(parameter));

        this.execute((T)parameter);
    }
}