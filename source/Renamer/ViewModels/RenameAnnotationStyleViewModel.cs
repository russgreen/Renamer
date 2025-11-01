using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Renamer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Renamer.ViewModels;
internal partial class RenameAnnotationStyleViewModel : BaseRenameViewModel
{
    public RenameAnnotationStyleViewModel() : base("Text", Host.GetService<ILogger<RenameAnnotationStyleViewModel>>())
    {
        
    }

    protected override void LoadElements()
    {
        var textNoteTypes = App.RevitDocument.GetElements()
            .OfClass(typeof(TextNoteType))
            .ToList();

        var dimensionTypes = App.RevitDocument.GetElements()
            .OfClass(typeof(DimensionType))
            .ToList();

        List<Element> allElements = new List<Element>();
        allElements.AddRange(textNoteTypes);
        allElements.AddRange(dimensionTypes);

        var models = allElements.Select(element => new ElementNameModel
        {
            Element = element,
            Category = GetCategoryName(element),
            Name = element.Name,
            NewName = element.Name
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    private string GetCategoryName(Element element)
    {
        if(element is TextNoteType textNoteType)
        {
            return "Text Style";
        }

        if (element is DimensionType dimensionType)
        {
            return dimensionType.FamilyName;
        }

        return "unknown";
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        // no specific actions for styles
    }

    protected override void PerformElementSpecificActionsAfterTransaction(ElementNameModel item)
    {
        // no specific actions for styles
    }
}
