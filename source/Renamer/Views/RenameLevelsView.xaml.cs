using System.Windows;
namespace Renamer.Views;
/// <summary>
/// Interaction logic for RenameLevelsView.xaml
/// </summary>
public partial class RenameLevelsView : Window
{
    private readonly ViewModels.RenameLevelsViewModel _viewModel;

    public RenameLevelsView()
    {
        InitializeComponent();
        //var viewModel = new MainViewModel();
        //this.DataContext = viewModel;

        //enable this line of code if XAML Behaviors is used
        //var _ = new Microsoft.Xaml.Behaviors.DefaultTriggerAttribute(typeof(Trigger), typeof(Microsoft.Xaml.Behaviors.TriggerBase), null);

        _viewModel = (ViewModels.RenameLevelsViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }
}
