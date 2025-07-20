using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using System;

namespace Renamer.Commands;
[Transaction(TransactionMode.Manual)]
public class CommandRenameTypes : ExternalCommand
{
    private readonly ILogger<CommandRenameTypes> _logger = Host.GetService<ILogger<CommandRenameTypes>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameTypes));

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var window = new Views.RenameTypesView();
        window.ShowDialog();
    }
}
