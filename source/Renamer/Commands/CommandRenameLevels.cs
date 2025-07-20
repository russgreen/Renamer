using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;
using System;

namespace Renamer.Commands;
[Transaction(TransactionMode.Manual)]
public class CommandRenameLevels : ExternalCommand
{
    private readonly ILogger<CommandRenameLevels> _logger = Host.GetService<ILogger<CommandRenameLevels>>();

    public override void Execute()
    {
        _logger.LogInformation("{command}", nameof(CommandRenameLevels));

        App.CachedUiApp = Context.UiApplication;
        App.RevitDocument = Context.ActiveDocument;

        var window = new Views.RenameLevelsView();
        window.ShowDialog();
    }
}
