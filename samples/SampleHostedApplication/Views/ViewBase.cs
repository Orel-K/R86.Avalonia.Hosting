using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using SampleHostedApplication.ViewModels;

namespace SampleHostedApplication.Views;

public abstract class ViewBase<T> : UserControl where T : ViewModelBase
{
    public T ViewModel => (T)DataContext!;

    protected ViewBase()
    {
        if (!Design.IsDesignMode)
        {
            DataContext = App.Current.Services.GetRequiredService<T>();
        }
    }
}
