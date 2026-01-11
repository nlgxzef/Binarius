using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Binary.Args;

public class ArgParser
{
    public ArgParser AddOption(string name, string description, string usage, char? alias = null, bool required = false, IArgType valueType = null)
    {
        if (alias != null) _nameSet.Add(alias.ToString(), _options.Count);
        _nameSet.Add(name, _options.Count);

        _options.Add(new(name, description, usage, alias, required, valueType));
        return this;
    }

    public RuntimeOptions Parse(string[] args)
    {
        var values = new Dictionary<string, object>();
        for (int i = 0; i < args.Length;)
        {
            string arg = args[i];
            if (!arg.StartsWith("-"))
                throw new ArgumentException("Expected argument");

            if (arg.StartsWith("--")) arg = arg.Substring(2);
            else
            {
                arg = arg.Substring(1);
                if (arg.Length > 1)
                    throw new ArgumentException("Invalid argument format");
            }
            if (arg.Length == 0) throw new ArgumentException("Invalid argument format");

            if (!_nameSet.TryGetValue(arg, out var idx))
                throw new ArgumentException($"Argument \"{args[i]}\" not found");

            i++;

            var opt = _options[idx];
            if (opt.ValueType != null)
            {
                string value = args[i];
                if (value.StartsWith("-"))
                    throw new ArgumentException($"Expected value for argument \"{args[i - 1]}\"");

                if (!opt.ValueType.TryParse(value, out var obj))
                    throw new ArgumentException($"Invalid value for argument \"{args[i - 1]}\"");

                if (!values.TryGetValue(opt.Name, out var list))
                    values[opt.Name] = obj;
                else
                    throw new ArgumentException($"Argument \"{args[i - 1]}\" doesn't accept multiple values");

                i++;
            }
        }

        var options = new RuntimeOptions(_options, values);
        options.Validate(this);

        return options;
    }

    public void ShowHelp()
    {
        var usageStr = new StringBuilder();
        var optStr = new StringBuilder();

        foreach(var opt in this.Options)
        {
            if(opt.Required)
            {
                if (usageStr.Length > 0) usageStr.Append(' ');
                usageStr.Append($"--{opt.Name} {opt.Usage}");
            }

            optStr.Append($"  --{opt.Name}");
            if (opt.Usage != null && opt.Usage.Length != 0) optStr.Append($" {opt.Usage}");

            optStr.AppendLine($" | {opt.Description}");
        }
        string name = Path.GetFileNameWithoutExtension(typeof(Program).Assembly.Location);
        Console.WriteLine($"Usage: {name} {usageStr}");
        Console.WriteLine(optStr);
    }

    public int OptionCount => _options.Count;
    public IEnumerable<OptionDescriptor> Options => _options;
    public OptionDescriptor this[int index] => _options[index];
    public OptionDescriptor this[string name] => _options[_nameSet[name]];

    readonly List<OptionDescriptor> _options = new();
    private readonly Dictionary<string, int> _nameSet = new();
}

public class OptionDescriptor
{
    public override string ToString() => $"--{this.Name}";
    public OptionDescriptor(string name, string description, string usage, char? alias, bool required, IArgType valueType)
    {
        this.Name = name;
        this.Description = description;
        this.Usage = usage;
        this.Alias = alias;
        this.Required = required;
        this.ValueType = valueType;
    }

    public string Name { get; }
    public string Description { get; }
    public string Usage { get; }
    public char? Alias { get; }
    public bool Required { get; }
    public IArgType ValueType { get; }
}
