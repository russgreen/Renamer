using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Renamer.Extensions;
using Renamer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Renamer.ViewModels;

internal abstract partial class BaseRenameViewModel : BaseViewModel
{
    public string WindowTitle { get; protected set; }

    [ObservableProperty]
    private bool _isCommandEnabled = false;

    [ObservableProperty]
    private ObservableCollection<ElementNameModel> _elements = new();

    [ObservableProperty]
    private ObservableCollection<object> _selectedElements;

    private HashSet<ElementNameModel> _previouslySelected = new HashSet<ElementNameModel>();

    [ObservableProperty]
    private bool _addPrefix = false;

    [ObservableProperty]
    private string _prefixText = string.Empty;

    [ObservableProperty]
    private int _prefixCharactersToRemove = 0;

    [ObservableProperty]
    private bool _findReplace = false;

    [ObservableProperty]
    private string _findText = string.Empty;

    [ObservableProperty]
    private string _replaceText = string.Empty;

    [ObservableProperty]
    private bool _addSuffix = false;

    [ObservableProperty]
    private string _suffixText = string.Empty;

    [ObservableProperty]
    private int _suffixCharactersToRemove = 0;

    [ObservableProperty]
    private bool _setTitleCase = false;

    [ObservableProperty]
    private bool _setPascalCase = false;

    protected readonly ILogger _logger;
    protected readonly string _elementTypeName;
    protected readonly string _transactionGroupName;

    protected BaseRenameViewModel(string elementTypeName, ILogger logger)
    {
        _elementTypeName = elementTypeName;
        _logger = logger;
        _transactionGroupName = $"Rename {elementTypeName}";

        var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        WindowTitle = $"Rename {elementTypeName} {informationVersion} ({App.RevitDocument.Title})";

        _logger.LogDebug("Initializing {viewModel}", GetType().Name);

        LoadElements();

        SelectedElements = new();
        SelectedElements.CollectionChanged += SelectedElements_CollectionChanged;
    }

    protected abstract void LoadElements();
    protected abstract void PerformElementSpecificActions(ElementNameModel item);

    private void SelectedElements_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        try
        {
            ValidateSelections();

            var currentSelected = SelectedElements.OfType<ElementNameModel>().ToHashSet();
            var newlySelected = currentSelected.Except(_previouslySelected);
            var deselected = _previouslySelected.Except(currentSelected);

            foreach (var model in newlySelected)
            {
                model.NewName = model.UpdateNewName(AddPrefix, PrefixCharactersToRemove, PrefixText,
                    FindReplace, FindText, ReplaceText, AddSuffix, SuffixCharactersToRemove, SuffixText,
                    SetTitleCase, SetPascalCase);
            }

            foreach (var model in deselected)
            {
                model.NewName = model.Element.Name;
            }

            _previouslySelected = currentSelected;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling selection change");
        }
    }

    // All the partial void methods for property changes
    partial void OnAddPrefixChanged(bool value) => RenameSelected();
    partial void OnPrefixCharactersToRemoveChanged(int value) => RenameSelected();
    partial void OnPrefixTextChanged(string value) => RenameSelected();
    partial void OnFindReplaceChanged(bool value) => RenameSelected();
    partial void OnFindTextChanged(string value) => RenameSelected();
    partial void OnReplaceTextChanged(string value) => RenameSelected();
    partial void OnAddSuffixChanged(bool value) => RenameSelected();
    partial void OnSuffixCharactersToRemoveChanged(int value) => RenameSelected();
    partial void OnSuffixTextChanged(string value) => RenameSelected();

    partial void OnSetTitleCaseChanged(bool value)
    {
        if (SetTitleCase) SetPascalCase = false;
        RenameSelected();
    }

    partial void OnSetPascalCaseChanged(bool value)
    {
        if (SetPascalCase) SetTitleCase = false;
        RenameSelected();
    }

    private void RenameSelected()
    {
        foreach (var item in SelectedElements.OfType<ElementNameModel>())
        {
            item.NewName = item.UpdateNewName(AddPrefix, PrefixCharactersToRemove, PrefixText,
                FindReplace, FindText, ReplaceText, AddSuffix, SuffixCharactersToRemove, SuffixText,
                SetTitleCase, SetPascalCase);
        }
    }

    private void ValidateSelections()
    {
        IsCommandEnabled = SelectedElements.Any();
    }

    [RelayCommand]
    private void RenameElements()
    {
        IsCommandEnabled = false;

        using (TransactionGroup transactionGroup = new TransactionGroup(App.RevitDocument))
        {
            transactionGroup.Start(_transactionGroupName);

            foreach (var item in SelectedElements.OfType<ElementNameModel>())
            {
                if (item.NewName != item.Name)
                {
                    using (Transaction transaction = new Transaction(App.RevitDocument))
                    {
                        transaction.Start($"Rename {_elementTypeName} {item.Name} to {item.NewName}");

                        try
                        {
                            item.Element.Name = item.NewName;
                            PerformElementSpecificActions(item);
                            transaction.Commit();

                            _logger.LogInformation($"Renamed {item.Name} to {item.NewName}");
                            item.NewName = "[Renamed]";
                            DispatcherHelper.DoEvents();
                        }
                        catch (Exception ex)
                        {
                            transaction.RollBack();
                            _logger.LogError(ex, $"Failed to rename {item.Name} to {item.NewName}");
                        }
                    }
                }
            }

            transactionGroup.Assimilate();
        }

        OnClosingRequest();
    }
}
