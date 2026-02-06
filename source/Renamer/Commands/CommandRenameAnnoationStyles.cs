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
public class CommandRenameAnnotationStyles : ExternalCommand
{
    private readonly ILogger<CommandRenameAnnotationStyles> _logger = Host.GetService<ILogger<CommandRenameAnnotationStyles>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameAnnotationStyles));

        using (LogContext.PushProperty("UsageTracking", true))
        {
            _logger.LogInformation("{command}", nameof(CommandRenameAnnotationStyles));
        }

        App.CachedUiApp = RevitContext.UiApplication;
        App.RevitDocument = RevitContext.ActiveDocument;

        var window = new Views.RenameAnnotationStyleView();
        window.ShowDialog();
    }
}
