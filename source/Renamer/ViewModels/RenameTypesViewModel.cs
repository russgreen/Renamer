using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Renamer.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Renamer.ViewModels;
internal partial class RenameTypesViewModel : BaseRenameViewModel
{
    public RenameTypesViewModel() : base("Types", Host.GetService<ILogger<RenameTypesViewModel>>())
    {
    }

    protected override void LoadElements()
    {
        var types = App.RevitDocument.GetElements().WhereElementIsElementType()
            .Cast<ElementType>();

        var models = types.Select(element => new ElementNameModel
        {
            Element = element,
            Category = element.Category?.Name ?? GetNullCategoryName(element),
            Name = element.Name,
            NewName = element.Name,
            Name2 = element.FamilyName
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        SetBIMObjectName(item);
    }

    private string GetNullCategoryName(ElementType elementType)
    {
        if(elementType.Category == null)
        {
            _logger.LogDebug(elementType.GetType().Name);

            if (elementType.GetType().Equals(typeof(ViewFamilyType)))
            {
                return "View Types";
            }

            if (elementType.GetType().Equals(typeof(TextNoteType)))
            {
                return "Text Types";
            }

            if (elementType.GetType().Equals(typeof(DimensionType)) ||
                elementType.GetType().Equals(typeof(SpotDimensionType)))
            {
                return "Dimension Types";
            }


            if (elementType.GetType().Equals(typeof(BrowserOrganization)))
            {
                return "Browser Organization";
            }
        }

        return "No Category";
    }

    private void SetBIMObjectName(ElementNameModel item)
    {
        try
        {
            var param = item.Element.FindParameter("BIMObjectName");

            if(param != null)
            {
                param.Set(item.NewName);
            }
                
        }
        catch
        {

        }
    }
}
