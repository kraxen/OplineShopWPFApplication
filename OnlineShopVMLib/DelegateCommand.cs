using System.Windows.Input;

namespace OnlineShopVMLib;
public class DelegateCommand : ICommand
{
    private readonly Action<object> execute;
    private readonly Func<object, bool> canExecute;
    public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return canExecute.Invoke(parameter);
    }

    public void Execute(object parameter)
    {
        execute.Invoke(parameter);
    }
    public void OnCanExecuteChanged()
    {
        CanExecuteChanged.Invoke(null, null);
    }
}