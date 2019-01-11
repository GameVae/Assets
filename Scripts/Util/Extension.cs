﻿using CustomAttr;
using EnumCollect;
using Json;
using Json.Interface;
using Network.Sync;
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

    public static bool HasAttribute<T>(this Type type, object value) where T : Attribute
    {
        MemberInfo info = type.GetMember(value.ToString()).FirstOrDefault();
        return info != null && Attribute.IsDefined(info, typeof(T));
    }

    public static bool HasAttribute<T>(this Enum value) where T : Attribute
    {
        return value.GetType().HasAttribute<T>(value);
    }

    public static bool IsDefined(this ListUpgrade e)
    {
        return Enum.IsDefined(typeof(ListUpgrade), e);
    }

    public static bool IsUpgrade(this ListUpgrade value)
    {
        return HasAttribute<UpgradeAttribute>(value);
    }

    public static string InsertSpace(this string value)
    {
        return Regex.Replace(value, "([a-z])([A-Z])", "$1 $2");
    }

    public static bool IsJsonArray(this string json)
    {
        if (json.ValidateJson())
        {
            return json.Trim().StartsWith("[");
        }
        else
        {
            return false;
        }
    }

    public static bool ValidateJson(this string value)
    {
        value = value.Trim();
        bool start = value.StartsWith("[") || value.StartsWith("{");
        bool end = value.EndsWith("]") || value.EndsWith("}");
        return start && end;
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

    public static Vector2 RealSize(this RectTransform trans)
    {
        return new Vector2(trans.rect.width, trans.rect.height);
    }
}