using System;

namespace Films.GUIApp.Models;

public enum OsType
{
    Unknown, Linux, Windows
}

public class Os
{
    public OsType Type { get; }

    public Os()
    {
        if (OperatingSystem.IsLinux())
        {
            Type = OsType.Linux;
        }
        else if (OperatingSystem.IsWindows())
        {
            Type = OsType.Windows;
        }
        else
        {
            Type = OsType.Unknown;
        }
    }
}
