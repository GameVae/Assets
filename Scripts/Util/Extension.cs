using CustomAttr;
using EnumCollect;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Extension
{
    public static T TryGet<T>(this object[] data, int index)
    {
        try
        {
            return (T)data[index];
        }
        catch
        {
            return default(T);
        }
    }

    public static bool HasAttribute<T>(this Type type,object value) where T : Attribute
    {
        MemberInfo info = type.GetMember(value.ToString()).FirstOrDefault();
        return info != null && Attribute.IsDefined(info,typeof(T));
    }

    public static bool HasAttribute<T>(this Enum value) where T : Attribute
    {
        return value.GetType().HasAttribute<T>(value);
    }

    public static bool IsUpgrade(this ListUpgrade value)
    {
        return HasAttribute<UpgradeAttribute>(value);
    }

    public static string InsertSpace(this string value)
    {
        return Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
    }

    public static Vector3Int Parse3Int(this string value)
    {
        try
        {
            string[] xyz = value.Split(',');
            return new Vector3Int(int.Parse(xyz[0]), int.Parse(xyz[1]), int.Parse(xyz[2]));
        }
        catch { return Vector3Int.zero; }
    }

    public static Vector3 Parse3Float(this string value)
    {
        try
        {
            string[] xyz = value.Split(',');
            return new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
        }
        catch { return Vector3.zero; }
    }
}