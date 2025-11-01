using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using Serilog.Context;
using System;

namespace Renamer.Commands;
[Transaction(TransactionMode.Manual)]
public class CommandRenameViews : ExternalCommand
{
    private readonly ILogger<CommandRenameViews> _logger = Host.GetService<ILogger<CommandRenameViews>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameViews));

        using (LogContext.PushProperty("UsageTracking", true))
        {
            _logger.LogInformation("{command}", nameof(CommandRenameViews));
        }

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var window = new Views.RenameViewsView();
        window.ShowDialog();
    }
}
