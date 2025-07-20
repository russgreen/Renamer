using System.Windows;

namespace Renamer.Views;
/// <summary>
/// Interaction logic for RenameFiltersView.xaml
/// </summary>
public partial class RenameFiltersView : Window
{
    private readonly ViewModels.RenameFiltersViewModel _viewModel;

    public RenameFiltersView()
    {
        InitializeComponent();
        //var viewModel = new MainViewModel();
        //this.DataContext = viewModel;

        //enable this line of code if XAML Behaviors is used
        //var _ = new Microsoft.Xaml.Behaviors.DefaultTriggerAttribute(typeof(Trigger), typeof(Microsoft.Xaml.Behaviors.TriggerBase), null);

        _viewModel = (ViewModels.RenameFiltersViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }
}
