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
public class CommandRenameAnnoationStyles : ExternalCommand
{
    private readonly ILogger<CommandRenameAnnoationStyles> _logger = Host.GetService<ILogger<CommandRenameAnnoationStyles>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameAnnoationStyles));

        using (LogContext.PushProperty("UsageTracking", true))
        {
            _logger.LogInformation("{command}", nameof(CommandRenameAnnoationStyles));
        }

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var window = new Views.RenameAnnotationStyleView();
        window.ShowDialog();
    }
}
