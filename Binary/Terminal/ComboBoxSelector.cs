using Endscript.Commands;

using System;
using System.Drawing;

namespace Binary.Terminal;

internal class ComboBoxSelector
{
    public int Process()
    {
        Init();
        while (true) {
            this.DisplayOptions();

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    _selected = Math.Max(0, _selected - 1);
                    break;
                case ConsoleKey.DownArrow:
                    _selected = Math.Min(this.Cmd.Options.Length - 1, _selected + 1);
                    break;
                case ConsoleKey.Enter:
                    this.Clear();
                    return _selected;
            }
        }
    }

    void DisplayOptions()
    {
        Console.SetCursorPosition(_optionsStartPoint.X, _optionsStartPoint.Y);
        for(int i = 0; i < this.Cmd.Options.Length; i++)
        {
            if (i == _selected) Console.Write("\x1B[45m");
            else Console.Write("\x1B[40m");

            Console.WriteLine($"  {this.Cmd.Options[i].Name}  ");
        }
    }

    void Init()
    {
        Screen.UseAlternateScreen();

        _selected = 0;

        Console.WriteLine($"{this.Cmd.Description} \x1B[1m\x1B[4m< Select with ↑ / ↓ arrows >\x1B[0m");
        _optionsStartPoint = new(Console.CursorLeft, Console.CursorTop);
    }
    void Clear()
    {
        Console.Write("\x1B[40m");
        Console.Clear();
        Screen.UseMainScreen();
    }

    public ComboBoxSelector(ComboboxCommand cmd)
    {
        this.Cmd = cmd;
    }

    public ComboboxCommand Cmd { get; }
   
    Point _optionsStartPoint;
    int _selected;
}
