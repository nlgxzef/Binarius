using System;
using System.Runtime.InteropServices;

namespace Binary.Terminal;

internal static class Screen
{
    public static void UseAlternateScreen() => Console.Write("\x1B[?1049h");
    public static void UseMainScreen() => Console.Write("\x1B[?1049l");

    public static void Init()
    {
        GetConsoleMode(_hOut, out var mode);
        SetConsoleMode(_hOut, mode | ENABLE_PROCESSED_OUTPUT | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
    }

    static Screen()
    {
        _hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    }

    static readonly IntPtr _hOut;

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool GetConsoleMode(
        IntPtr hConsoleHandle,
        out uint lpMode
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetConsoleMode(
        IntPtr hConsoleHandle,
        uint lpMode
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);


    const int STD_INPUT_HANDLE = -10;
    const int STD_OUTPUT_HANDLE = -11;

    const int ENABLE_PROCESSED_OUTPUT = 0x0001;
    const int ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
}
