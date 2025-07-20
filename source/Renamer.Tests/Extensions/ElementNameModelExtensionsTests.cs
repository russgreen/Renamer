using Xunit;
using FluentAssertions;
using Renamer.Extensions;
using Renamer.Models;
using Moq;
using Autodesk.Revit.DB;

namespace Renamer.Tests.Extensions;

public class ElementNameModelExtensionsTests
{
    private ElementNameModel CreateModel(string name)
    {
        return new ElementNameModel { Name = name };
    }

    [Fact]
    public void UpdateNewName_ShouldThrowArgumentNullException_WhenModelIsNull()
    {
        ElementNameModel model = null;
        Action act = () => model.UpdateNewName(false, 0, "", false, "", "", false, 0, "", false, false);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void UpdateNewName_ShouldReturnOriginalName_WhenNoOptionsSet()
    {
        var model = CreateModel("OriginalName");
        var result = model.UpdateNewName(false, 0, "", false, "", "", false, 0, "", false, false);
        result.Should().Be("OriginalName");
    }

    [Fact]
    public void UpdateNewName_ShouldReplaceText_WhenFindReplaceIsTrue()
    {
        var model = CreateModel("TestNameTest");
        var result = model.UpdateNewName(false, 0, "", true, "Test", "Demo", false, 0, "", false, false);
        result.Should().Be("DemoNameDemo");
    }

    [Fact]
    public void UpdateNewName_ShouldNotReplace_WhenTextToFindOrReplaceIsEmpty()
    {
        var model = CreateModel("TestName");
        var result1 = model.UpdateNewName(false, 0, "", true, "", "Demo", false, 0, "", false, false);
        var result2 = model.UpdateNewName(false, 0, "", true, "Test", "", false, 0, "", false, false);
        result1.Should().Be("TestName");
        result2.Should().Be("TestName");
    }

    [Fact]
    public void UpdateNewName_ShouldAddPrefix_WhenEnabledAndNotAlreadyPresent()
    {
        var model = CreateModel("Name");
        var result = model.UpdateNewName(true, 0, "Pre_", false, "", "", false, 0, "", false, false);
        result.Should().Be("Pre_Name");
    }

    [Fact]
    public void UpdateNewName_ShouldRemovePrefixChars_WhenAddingPrefix()
    {
        var model = CreateModel("PrefixName");
        var result = model.UpdateNewName(true, 6, "Pre_", false, "", "", false, 0, "", false, false);
        result.Should().Be("Pre_Name");
    }

    [Fact]
    public void UpdateNewName_ShouldNotAddPrefix_IfAlreadyPresent()
    {
        var model = CreateModel("Pre_Name");
        var result = model.UpdateNewName(true, 0, "Pre_", false, "", "", false, 0, "", false, false);
        result.Should().Be("Pre_Name");
    }

    [Fact]
    public void UpdateNewName_ShouldAddSuffix_WhenEnabledAndNotAlreadyPresent()
    {
        var model = CreateModel("Name");
        var result = model.UpdateNewName(false, 0, "", false, "", "", true, 0, "_Suf", false, false);
        result.Should().Be("Name_Suf");
    }

    [Fact]
    public void UpdateNewName_ShouldRemoveSuffixChars_WhenAddingSuffix()
    {
        var model = CreateModel("NameSuffix");
        var result = model.UpdateNewName(false, 0, "", false, "", "", true, 6, "_Suf", false, false);
        result.Should().Be("Name_Suf");
    }

    [Fact]
    public void UpdateNewName_ShouldNotAddSuffix_IfAlreadyPresent()
    {
        var model = CreateModel("Name_Suf");
        var result = model.UpdateNewName(false, 0, "", false, "", "", true, 0, "_Suf", false, false);
        result.Should().Be("Name_Suf");
    }

    [Fact]
    public void UpdateNewName_ShouldConvertToTitleCase_WhenEnabled()
    {
        var model = CreateModel("test name");
        var result = model.UpdateNewName(false, 0, "", false, "", "", false, 0, "", true, false);
        result.Should().Be("Test Name");
    }

    [Fact]
    public void UpdateNewName_ShouldConvertToPascalCase_WhenEnabled()
    {
        var model = CreateModel("test name");
        var result = model.UpdateNewName(false, 0, "", false, "", "", false, 0, "", false, true);
        result.Should().Be("TestName");
    }

    [Fact]
    public void UpdateNewName_ShouldHandleAllOptionsCombined()
    {
        var model = CreateModel("oldname");
        var result = model.UpdateNewName(
            addPrefix: true,
            prefixCharsToRemove: 3,
            prefix: "Pre_",
            findReplace: true,
            textToFind: "name",
            textToReplace: "item",
            addSuffix: true,
            suffixCharsToRemove: 2,
            suffix: "_Suf",
            toTitleCase: false,
            toPascalCase: true
        );
        // Steps: Remove first 3 chars ("oldname" -> "name"), Replace "name" with "item" ("item"), Add prefix ("Pre_item"), Remove last 2 chars ("Pre_it"), Add suffix ("Pre_it_Suf"), PascalCase ("PreIt_Suf")
        result.Should().Be("Pre_It_Suf");
    }
}