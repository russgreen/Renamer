using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.Options;
using Renamer.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Renamer.ViewModels;
internal partial class RenameFiltersViewModel : BaseRenameViewModel
{
    public RenameFiltersViewModel() : base("Filters", Host.GetService<ILogger<RenameFiltersViewModel>>())
    {
    }

    protected override void LoadElements()
    {
        var families = App.RevitDocument.GetElements()
            .OfClass(typeof(FilterElement))
            .Cast<FilterElement>();

        var models = families.Select(filter => new ElementNameModel
        {
            Element = filter,
            Category = GetCategory(filter),
            Name = filter.Name,
            NewName = filter.Name
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        // no specific actions for filters        
    }

    protected override void PerformElementSpecificActionsAfterTransaction(ElementNameModel item)
    {
        // no specific actions for filters
    }

    private string GetCategory(FilterElement filter)
    {
        if (filter is SelectionFilterElement)
        {
            return "Selection filter";
        }

        if (filter is ParameterFilterElement)
        {
            return "Parameter filter";
        }

        return "Unknown";
    }
}
