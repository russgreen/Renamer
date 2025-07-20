using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;

namespace Renamer.Services;

internal class MessageBoxService : IMessageBoxService
{
    private readonly ILogger<MessageBoxService> _logger;

    public MessageBoxService(ILogger<MessageBoxService> logger)
    {
        _logger = logger;
    }

    public bool ShowOk(string title, string message)
    {
        var result = TaskDialog.Show(title, message, TaskDialogCommonButtons.Ok);

        if (result == TaskDialogResult.Ok)
        {
            return true;
        }

        return false;
    }
}

