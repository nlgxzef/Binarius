using System;
namespace Binary.Args;

public interface IArgType
{
    public Type Type { get; }
    public bool TryParse(string str, out object value);
}

public static class ArgTypes
{
    public static readonly IArgType String = new StringArgType();
    public static readonly IArgType Int32 = new Int32ArgType();
    public static readonly IArgType Boolean = new BooleanArgType();
}

public class StringArgType : IArgType
{
    public Type Type => typeof(string);
    public bool TryParse(string str, out object value)
    {
        value = str;
        return true;
    }
}
public class Int32ArgType : IArgType
{
    public Type Type => typeof(int);
    public bool TryParse(string str, out object value)
    {
        if(!int.TryParse(str, out var intValue))
        {
            value = null;
            return false;
        }

        value = intValue;
        return true;
    }
}
public class BooleanArgType : IArgType
{
    public Type Type => typeof(bool);
    public bool TryParse(string str, out object value)
    {
        if (!bool.TryParse(str, out var boolValue))
        {
            value = null;
            return false;
        }
        value = boolValue;
        return true;
    }
}
public class EnumArgType<T> : IArgType where T : Enum
{
    public Type Type => typeof(T);
    public bool TryParse(string str, out object value)
    {
        if (!Enum.TryParse(typeof(T), str, true, out var enumValue))
        {
            value = null;
            return false;
        }
        value = enumValue;
        return true;
    }
}
