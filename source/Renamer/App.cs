using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using Renamer.Commands;
using Renamer.Controllers;
using Serilog;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace Renamer
{
    public class App : ExternalApplication
    {
        // get the absolute path of this assembly
        public static readonly string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        // class instance
        public static App ThisApp;

        public static UIControlledApplication CachedUiCtrApp;
        public static UIApplication CachedUiApp;
        public static ControlledApplication CtrApp;
        public static Autodesk.Revit.DB.Document RevitDocument;

        private AppDocEvents _appEvents;
        private ILogger<App> _logger;

        public override void OnStartup()
        {
            ThisApp = this;
            CachedUiCtrApp = Application;
            CtrApp = Application.ControlledApplication;

            Host.StartHost();

            _logger = Host.GetService<ILogger<App>>();

            var panel = RibbonPanel(CachedUiCtrApp);

            Syncfusion.Licensing.SyncfusionLicenseProvider
                .RegisterLicense("##SyncfusionLicense##");

            AddAppDocEvents();
        }

        public override void OnShutdown()
        {
            RemoveAppDocEvents();

            Host.StopHost();
            Log.CloseAndFlush();

            base.OnShutdown();
        }

        #region Event Handling
        private void AddAppDocEvents()
        {
            _appEvents = new AppDocEvents();
            _appEvents.EnableEvents();
        }
        private void RemoveAppDocEvents()
        {
            _appEvents.DisableEvents();
        }
        #endregion

        #region Ribbon Panel

        private RibbonPanel RibbonPanel(UIControlledApplication application)
        {

            RibbonPanel panel = CachedUiCtrApp.CreateRibbonPanel("Renamer_Panel");
            panel.Title = "Renamer";

            var pullButton = panel.AddPullDownButton("Renamer_PullDown", "Renamer");
            pullButton.LargeImage = PngImageSource("Renamer.Resources.Renamer_Button.png");
            
            pullButton.AddPushButton<CommandRenameFamilies>("Rename families")
                .SetAvailabilityController<CommandAvailabilityProject>();

            pullButton.AddPushButton<CommandRenameTypes>("Rename types")
                .SetAvailabilityController<CommandAvailabilityProject>();

            pullButton.AddSeparator();

            pullButton.AddPushButton<CommandRenameMaterials>("Rename materials")
                .SetAvailabilityController<CommandAvailabilityProject>();

            pullButton.AddSeparator();

            pullButton.AddPushButton<CommandRenameLevels>("Rename levels")
                .SetAvailabilityController<CommandAvailabilityProject>();

            pullButton.AddPushButton<CommandRenameViews>("Rename views")
                .SetAvailabilityController<CommandAvailabilityProject>();

            pullButton.AddPushButton<CommandRenameFilters>("Rename filters")
                .SetAvailabilityController<CommandAvailabilityProject>();

            return panel;
        }

        private System.Windows.Media.ImageSource PngImageSource(string embeddedPath)
        {
            var stream = GetType().Assembly.GetManifestResourceStream(embeddedPath);
            System.Windows.Media.ImageSource imageSource;
            try
            {
                imageSource = BitmapFrame.Create(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading image from embedded resource");
                imageSource = null;
            }

            return imageSource;
        }
        #endregion
    }
}
