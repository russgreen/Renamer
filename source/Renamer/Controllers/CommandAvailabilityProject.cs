using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Renamer.Controllers;
internal class CommandAvailabilityProject : IExternalCommandAvailability
{
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
    {
        if (applicationData.ActiveUIDocument != null)
        {
            return !applicationData.ActiveUIDocument.Document.IsFamilyDocument;
        }

        return false;
    }
}
