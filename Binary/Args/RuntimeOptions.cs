using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
namespace Binary.Args;

public class RuntimeOptions
{
    public void Validate(ArgParser parser)
    {
        foreach(var opt in parser.Options)
        {
            if (opt.Required && !_nameSet.ContainsKey(opt.Name))
                throw new ArgumentException($"Missing argument \"--{opt.Name}\"");
        }
    }
    public bool TryGetOption(string name, out RuntimeOption option)
    {
        if (!_nameSet.TryGetValue(name, out var idx))
        {
            option = null;
            return false;
        }
        option = _options[idx];
        return true;
    }

    public RuntimeOptions(IEnumerable<OptionDescriptor> descriptors, IEnumerable<KeyValuePair<string, object>> values)
    {
        foreach(var kv in values)
        {
            var desc = descriptors.First(d => d.Name == kv.Key);
            _nameSet[desc.Name] = _options.Count;
            if(desc.Alias != null) _nameSet[desc.Alias.ToString()] = _options.Count;

            _options.Add(new RuntimeOption(desc, kv.Value));
        }
    }

    public RuntimeOption this[int index] => _options[index];
    public RuntimeOption this[string name]
    {
        get
        {
            if (!_nameSet.TryGetValue(name, out var idx)) return null;
            return _options[idx];
        }
    }

    readonly Dictionary<string, int> _nameSet = new();
    readonly List<RuntimeOption> _options = new();
}

public class RuntimeOption
{
    public T GetValue<T>() => (T)this.Value;
    public override string ToString() => $"--{this.Descriptor.Name}";

    public RuntimeOption(OptionDescriptor descriptor, object value)
    {
        this.Descriptor = descriptor;
        this.Value = value;
    }

    public OptionDescriptor Descriptor { get; }
    public object Value { get; }

    readonly object _value = new();
}
