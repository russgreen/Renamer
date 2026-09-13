using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Syncfusion.UI.Xaml.Grid;

namespace Renamer.Views;

/// <summary>
/// Builds the SfDataGrid shared by the Rename*View windows, since every one of them
/// wires up the same base grid behaviour and only differs in its sort/value columns.
/// </summary>
internal static class RenameGridFactory
{
    public static SfDataGrid Create(string[] sortColumnNames, params GridColumn[] valueColumns)
    {
        var grid = new SfDataGrid
        {
            AutoGenerateColumns = false,
            AllowEditing = false,
            AllowGrouping = false,
            AllowResizingColumns = true,
            AllowFiltering = true,
            NavigationMode = NavigationMode.Cell,
            SelectionMode = GridSelectionMode.Extended,
            GridValidationMode = GridValidationMode.InView,
            ColumnSizer = GridLengthUnitType.AutoWithLastColumnFill,
        };

        grid.SetBinding(SfDataGrid.ItemsSourceProperty, new Binding("Elements"));
        grid.SetBinding(SfDataGrid.SelectedItemsProperty, new Binding("SelectedElements"));

        foreach (var columnName in sortColumnNames)
        {
            grid.SortColumnDescriptions.Add(new SortColumnDescription
            {
                ColumnName = columnName,
                SortDirection = ListSortDirection.Ascending
            });
        }

        grid.Columns.Add(new GridCheckBoxSelectorColumn
        {
            MappingName = "SelectorColumn",
            HeaderText = "",
            AllowCheckBoxOnHeader = true,
            Width = 34
        });

        foreach (var column in valueColumns)
        {
            grid.Columns.Add(column);
        }

        grid.Columns.Add(CreateNewNameColumn());

        return grid;
    }

    public static GridTextColumn CreateTextColumn(string mappingName, string headerText) => new()
    {
        MappingName = mappingName,
        HeaderText = headerText,
        Padding = new Thickness(0, 0, 10, 0)
    };

    private static GridTextColumn CreateNewNameColumn()
    {
        var column = CreateTextColumn("NewName", "New Name");

        var style = new Style(typeof(GridCell));
        var renamedTrigger = new DataTrigger
        {
            Binding = new Binding("NewName"),
            Value = "[Renamed]"
        };
        renamedTrigger.Setters.Add(new Setter(Control.ForegroundProperty, Brushes.DarkGreen));
        style.Triggers.Add(renamedTrigger);
        column.CellStyle = style;

        return column;
    }
}
