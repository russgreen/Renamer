using System;
using System.Diagnostics;

namespace Renamer.Extensions;
internal static class ElementNameModelExtensions
{

    internal static string UpdateNewName(this Models.ElementNameModel model, 
        bool addPrefix,
        int prefixCharsToRemove,
        string prefix, 
        bool findReplace,
        string textToFind,
        string textToReplace,
        bool addSuffix,
        int suffixCharsToRemove,
        string suffix,
        bool toTitleCase,
        bool toPascalCase)
    {
        if (model == null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var newName = model.Name;
        
        if (findReplace)
        {
            if(!string.IsNullOrEmpty(textToFind) & !string.IsNullOrEmpty(textToReplace))
            {
                newName = newName.Replace(textToFind, textToReplace);    
            }
        }

        if (addPrefix) 
        {
            var actualCharactersToRemove = Math.Min(prefixCharsToRemove, newName.Length);
            newName = newName.Remove(0, actualCharactersToRemove);

            if (!string.IsNullOrEmpty(prefix) && !newName.StartsWith(prefix))
            {
                newName = $"{prefix}{newName}";
            }
        }

        if (addSuffix)
        {
            var actualCharactersToRemove = Math.Min(suffixCharsToRemove, newName.Length);
            newName = newName.Remove(newName.Length - actualCharactersToRemove, actualCharactersToRemove);

            if (!string.IsNullOrEmpty(suffix) && !newName.EndsWith(suffix))
            {
                newName = $"{newName}{suffix}";
            }

        }

        if(toTitleCase)
        {
            newName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName);
        }

        if (toPascalCase)
        {
            newName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newName).Replace(" ", string.Empty);
        }

        return newName;
    }

}
