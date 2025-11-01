using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Renamer.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Renamer.ViewModels;
internal partial class RenameViewsViewModel : BaseRenameViewModel
{
    public RenameViewsViewModel() : base("Views", Host.GetService<ILogger<RenameViewsViewModel>>())
    {
    }

    protected override void LoadElements()
    {
        var levels = App.RevitDocument.GetElements()
            .OfClass(typeof(View))
            .WhereElementIsNotElementType()
            .Where(v => v.Category != null)
            .Where(v => !v.Name.StartsWith("<Revision Schedule>"))
            .Cast<View>();

        var models = levels.Select(view => new ElementNameModel
        {
            Element = view,
            Category = GetCategory(view),
            Name = view.Name,
            NewName = view.Name
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    private string GetCategory(View view)
    {
        _logger.LogDebug(view.GetType().Name);

        if(view.IsTemplate)
        {
            return "View Template";
        }

        switch (view.ViewType)
        {
            case ViewType.FloorPlan:
                return "Floor Plan";
            case ViewType.CeilingPlan:
                return "Ceiling Plan";
            case ViewType.Elevation:
                return "Elevation";
            case ViewType.Detail:
                return "Detail View";
            case ViewType.DraftingView:
                return "Drafting View";
            case ViewType.Section:
                return "Section";
            case ViewType.ThreeD:
                return "3D View";
            case ViewType.Schedule:
                return "Schedule";
            case ViewType.DrawingSheet:
                return "Sheet";
            default:
                break;
        }

        if (view.GetType().Equals(typeof(ViewFamilyType)))
        {
            return "View Type";
        }


        return view.Category?.Name ?? "No Category";
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        //no specific actions for views
    }

    protected override void PerformElementSpecificActionsAfterTransaction(ElementNameModel item)
    {
        // no specific actions for views
    }
}