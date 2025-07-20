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
        SetBIMObjectName(item);
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


    private void SetBIMObjectName(ElementNameModel item)
    {
        try
        {
            var family = (Family)item.Element;
            var formula = "\"" + item.NewName + "\"";

            SetFamilyParameterFormula(family, "BIMObjectName", formula);
        }
        catch
        {
            // If the parameter does not exist, we can skip this
        }
    }

    private void SetFamilyParameterFormula(Family family, string parameterName, string formula)
    {
        // Open the family for editing
        var famDoc = family.Document.EditFamily(family);
        var famManager = famDoc.FamilyManager;
        var param = famManager.get_Parameter(parameterName);

        if (param != null)
        {
            using (var trans = new Transaction(famDoc, "Set Formula"))
            {
                trans.Start();
                famManager.SetFormula(param, formula);
                trans.Commit();
            }

            famDoc.LoadFamily(App.RevitDocument, new FamilyLoadOptions());
            famDoc.Close(false);
        }
    }
}
