using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumUtil
{
    public static T[] Values<T>()
    {
        return Enum.GetValues( typeof(T) ) as T[];
    }

    public static int Count<T>()
    {
        return Enum.GetValues( typeof(T) ).Length;
    }

    public static TEnum Parse<TEnum>( string value )
    {
        return (TEnum) Enum.Parse( typeof(TEnum), value );
    }

    public static bool TryParse<TEnum>( string value, out TEnum result, bool ignoreCase = false ) where TEnum : struct
    {
        try
        {
            result = (TEnum) Enum.Parse( typeof(TEnum), value, ignoreCase );
            return true;
        }
        catch( ArgumentException )
        {
            result = default( TEnum );
            return false;
        }
    }
}
