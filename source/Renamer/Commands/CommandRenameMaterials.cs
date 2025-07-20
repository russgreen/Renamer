using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using System;

namespace Renamer.Commands;
[Transaction(TransactionMode.Manual)]
public class CommandRenameMaterials : ExternalCommand
{
    private readonly ILogger<CommandRenameMaterials> _logger = Host.GetService<ILogger<CommandRenameMaterials>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameMaterials));

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var window = new Views.RenameMaterialsView();
        window.ShowDialog();
    }
}
