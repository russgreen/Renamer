using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using CommunityToolkit.Mvvm.ComponentModel;
using IniParser;
using Microsoft.Extensions.Logging;
using Nice3point.Revit.Extensions;
using Renamer.Models;
using Renamer.Services;
using Renamer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Renamer.ViewModels;

internal partial class RenameMaterialsViewModel : BaseRenameViewModel
{

    [ObservableProperty]
    private bool _findReplaceTextures = false;

    private StringBuilder _failures = new();
        
    private readonly IMessageBoxService _messageBox = Host.GetService<IMessageBoxService>();

    public RenameMaterialsViewModel() : base("Materials", Host.GetService<ILogger<RenameMaterialsViewModel>>())
    {
    }

    protected override void LoadElements()
    {
        var materials = App.RevitDocument.GetElements()
            .OfClass(typeof(Material))
            .ToList();

        var appearanceAssets = App.RevitDocument.GetElements()
            .OfClass(typeof(AppearanceAssetElement))
            .ToList();

        var physicalAssets = App.RevitDocument.GetElements()
            .OfClass(typeof(PropertySetElement))
            .ToList();

        var allElements = new List<Element>();
        allElements.AddRange(materials);
        allElements.AddRange(appearanceAssets);
        allElements.AddRange(physicalAssets);

        var models = allElements.Select(material => new ElementNameModel
        {
            Element = material,
            Category = GetCategoryName(material),
            Name = material.Name,
            NewName = material.Name,
        });

        Elements = new ObservableCollection<ElementNameModel>(models);
    }

    private string GetCategoryName(Element element)
    {
        if(element is Material)
        {
            // For Material, we can use the material's category name
            return "Material";
        }

        if (element is AppearanceAssetElement)
        {
            return "Appearance Asset";
        }
        
        if (element is PropertySetElement)
        {
            return "Physical Asset";
        }

        return "Unknown";
    }

    protected override void PerformElementSpecificActions(ElementNameModel item)
    {
        try
        {
            if (item.Category == "Material")
            {
                // For Material, we can set the BIMObjectName parameter
                SetBIMObjectName(item);
            }

            if (item.Category == "Appearance Asset")
            {
                if (FindReplaceTextures)
                {
                    ChangeRenderingAssetTextureName(item.Element, FindText, ReplaceText);
                }
            }

            _logger.LogInformation("Renamed {Name} to {NewName}", item.Name, item.NewName);

            item.NewName = "[Renamed]";

            DispatcherHelper.DoEvents();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rename {Name} to {NewName}", item.Name, item.NewName);
        }
    }

    protected override void PerformElementSpecificActionsAfterTransaction(ElementNameModel item)
    {
        // no specific actions for materials
    }

    private void ChangeRenderingAssetTextureName(Element element, string oldString, string newString)
    {
        try
        {
            var asset = element as AppearanceAssetElement;

            using (var editScope = new AppearanceAssetEditScope(App.RevitDocument))
            {
                // returns an editable copy of the appearance asset
                var editableAsset = editScope.Start(asset.Id);

                RenameAsset(editableAsset, "opaque_albedo", UnifiedBitmap.UnifiedbitmapBitmap, oldString, newString);
                RenameAsset(editableAsset, "opaque_f0", UnifiedBitmap.UnifiedbitmapBitmap, oldString, newString);
                RenameAsset(editableAsset, "metal_f0", UnifiedBitmap.UnifiedbitmapBitmap, oldString, newString);
                RenameAsset(editableAsset, "surface_albedo", UnifiedBitmap.UnifiedbitmapBitmap, oldString, newString);
                RenameAsset(editableAsset, "surface_roughness", UnifiedBitmap.UnifiedbitmapBitmap, oldString, newString);
                RenameAsset(editableAsset, "surface_normal", BumpMap.BumpmapBitmap, oldString, newString);

                editScope.Commit(true);
            }
        }
        catch 
        {

        }
    }

    private void RenameAsset(Asset asset, string propertyName, string connectedPropertyName, string oldString, string newString)
    {
        var textureMapProperty = asset.FindByName(propertyName);
        if (textureMapProperty != null)
        {
            var connectedAsset = textureMapProperty.GetSingleConnectedAsset();
            if (connectedAsset != null)
            {
                var textureMapBitmapProperty = connectedAsset.FindByName(connectedPropertyName) as AssetPropertyString;
                if (textureMapBitmapProperty != null)
                {
                    var newValue = Regex.Replace(textureMapBitmapProperty.Value, oldString, newString, RegexOptions.IgnoreCase); // texturemapBitmapProperty.Value.Replace(oldString, newString);                        

                    try
                    {
                        textureMapBitmapProperty.Value = newValue;
                    }
                    catch
                    {
                        try
                        {
                            var oldPath = GetImageFullPath(textureMapBitmapProperty.Value);
                            var fullPath = Regex.Replace(oldPath, oldString, newString, RegexOptions.IgnoreCase);

                            if (!File.Exists(fullPath))
                            {
                                if (File.Exists(oldPath))
                                {
                                    File.Copy(oldPath, fullPath, true);
                                }
                            }

                            textureMapBitmapProperty.Value = fullPath;
                        }
                        catch
                        {
                            _failures.Append($"{asset.Name} : {textureMapBitmapProperty.Value} \n");
                        }

                    }
                }
            }
        }
    }

    private string GetImageFullPath(string newValue)
    {
#if REVIT2022
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2022\Revit.ini";
#elif REVIT2023
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2023\Revit.ini";
#elif REVIT2024
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2024\Revit.ini";
#elif REVIT2025
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2025\Revit.ini";
#elif REVIT2026
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2026\Revit.ini";
#elif REVIT2027
        var revitIniFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Autodesk\Revit\Autodesk Revit 2027\Revit.ini";
#endif


        var parser = new FileIniDataParser();
        var revitIniData = parser.ReadFile(revitIniFile);

        var appearancePaths = revitIniData["Directories"]["AdditionalRenderAppearancePaths"].Split('|');

        if(appearancePaths.Length != 0)
        {
            foreach (var path in appearancePaths)
            {
                var fullPath = Path.Combine(path.Trim(), newValue);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
        }

        return newValue;
    }

    private void SetBIMObjectName(ElementNameModel item)
    {
        try
        {
            var param = item.Element.FindParameter("BIMObjectName_mtrl");

            param?.Set(item.NewName);
        }
        catch
        {

        }
    }
}
