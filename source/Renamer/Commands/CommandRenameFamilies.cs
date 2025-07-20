using System;
using System.Runtime.InteropServices;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Toolkit;
using Nice3point.Revit.Toolkit.External;

namespace Renamer.Commands
{
    [Transaction(TransactionMode.Manual)]

    public class CommandRenameFamilies : ExternalCommand
    {
        private ILogger<CommandRenameFamilies> _logger = Host.GetService<ILogger<CommandRenameFamilies>>();

        public override void Execute()
        {
            _logger.LogInformation("{command}", nameof(CommandRenameTypes));

            App.CachedUiApp = Context.UiApplication;
            App.RevitDocument = Context.ActiveDocument;

            var window = new Views.RenameFamiliesView();
            window.ShowDialog();
        }
    }
}
