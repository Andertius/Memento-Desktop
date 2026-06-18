using System;

namespace Memento.Avalonia.Helpers;

public static class ConfigDirectoryHelper
{
    public static string GetAppSettingsDirectory()
    {
#if DEBUG
        return AppDomain.CurrentDomain.BaseDirectory;
#elif RELEASE
        return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "Memento");
#endif
    }
}
