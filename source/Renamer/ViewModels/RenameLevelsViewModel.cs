using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Renamer.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Renamer.ViewModels;
internal partial class RenameLevelsViewModel : BaseRenameViewModel
{
  
    public RenameLevelsViewModel() : base("Levels", Host.GetService<ILogger<RenameLevelsViewModel>>())
    {
    }

    protected override void LoadElements()
    {
        var levels = App.RevitDocument.GetElements()
            .OfClass(typeof(Level))
            .Cast<Level>();

        var models = levels.Select(level => new ElementNameModel
        {
            Element = level,
            Category = level.Category?.Name ?? "No Category",
            Name = level.Name,
            NewName = level.Name
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        //no specific actions for levels
    }
}