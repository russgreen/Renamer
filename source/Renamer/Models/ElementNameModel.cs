using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Renamer.Models;

internal partial class ElementNameModel : ObservableObject
{
    [ObservableProperty]
    private Element _element;

    [ObservableProperty]
    private string _category;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _newName;

    [ObservableProperty]
    private string _name2;
}
