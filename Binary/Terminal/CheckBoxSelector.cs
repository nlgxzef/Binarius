using Endscript.Commands;

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binary.Terminal;

internal class CheckBoxSelector
{
    public int Process()
    {
        Init();
        while (true)
        {
            this.Display();

            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Spacebar:
                    _selected ^= 0x01;
                    break;
                case ConsoleKey.Y:
                    _selected = 0x01;
                    break;
                case ConsoleKey.N:
                    _selected = 0x00;
                    break;
                case ConsoleKey.Enter:
                    this.Clear();
                    return _selected;
            }
        }
    }

    void Display()
    {
        Console.SetCursorPosition(_point.X, _point.Y);
        Console.Write($"[{(_selected == 1 ? 'X' : ' ')}] ");
        Console.WriteLine($"{this.Cmd.Description} \x1B[1m\x1B[4m< Toggle with SPACE >\x1B[0m");
    }

    void Init()
    {
        Screen.UseAlternateScreen();

        _selected = 0;
        _point = new(Console.CursorLeft, Console.CursorTop);
    }
    void Clear()
    {
        Console.Write("\x1B[40m");
        Console.Clear();
        Screen.UseMainScreen();
    }

    public CheckBoxSelector(CheckboxCommand cmd)
    {
        this.Cmd = cmd;
    }

    public CheckboxCommand Cmd { get; }

    Point _point;
    int _selected;
}
