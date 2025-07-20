namespace Renamer.Services;

internal interface IMessageBoxService
{
    /// <summary>
    /// Display a message box with OK button
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <returns>True of Ok clicked, else false</returns>
    bool ShowOk(string title, string message);
}

