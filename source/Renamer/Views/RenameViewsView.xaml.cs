using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Renamer.Views;
/// <summary>
/// Interaction logic for RenameViewsModel.xaml
/// </summary>
public partial class RenameViewsView : Window
{
    private readonly ViewModels.RenameViewsViewModel _viewModel;

    public RenameViewsView()
    {
        InitializeComponent();

        //var viewModel = new MainViewModel();
        //this.DataContext = viewModel;

        //enable this line of code if XAML Behaviors is used
        //var _ = new Microsoft.Xaml.Behaviors.DefaultTriggerAttribute(typeof(Trigger), typeof(Microsoft.Xaml.Behaviors.TriggerBase), null);

        _viewModel = (ViewModels.RenameViewsViewModel)this.DataContext;
        _viewModel.ClosingRequest += (sender, e) => this.Close();
    }
}
