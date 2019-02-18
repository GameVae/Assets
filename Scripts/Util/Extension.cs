using CustomAttr;
using EnumCollect;
using Json;
using Json.Interface;
using Network.Sync;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Extension
{
    #region Array
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

    public static string ToJson<T>(this T[] arr)
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        string result = "[";
        string format = typeof(T) == typeof(string) ? "\"{0}\"" : "{0}";

        for (int i = 0; i < arr.Length; i++)
        {
            result += string.Format(format, arr[i]);
            result += (i < arr.Length - 1) ? "," : "";
        }
        return result + "]";
    }

    public static string ToJson(this string[] arr)
    {
        string result = "[";
        string format = "\"{0}\"";

        for (int i = 0; i < arr.Length; i++)
        {
            result += string.Format(format, arr[i]);
            result += (i < arr.Length - 1) ? "," : "";
        }
        return result + "]";
    }

    public static T[] IgnoreInstanceComponent<T>(this T[] arr, T comp) where T : Component
    {
        System.Collections.Generic.List<T> newArr = new System.Collections.Generic.List<T>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].gameObject.GetInstanceID() != comp.gameObject.GetInstanceID())
                newArr.Add(arr[i]);
        }
        return newArr.ToArray();
    }

    #endregion

    #region Attribute
    public static bool HasAttribute<T>(this Type type, object value) where T : Attribute
    {
        MemberInfo info = type.GetMember(value.ToString()).FirstOrDefault();
        return info != null && Attribute.IsDefined(info, typeof(T));
    }

    public static bool HasAttribute<T>(this Enum value) where T : Attribute
    {
        return value.GetType().HasAttribute<T>(value);
    }
    #endregion

    #region ListUpgrade
    public static bool IsDefined(this ListUpgrade e)
    {
        return Enum.IsDefined(typeof(ListUpgrade), e);
    }

    public static bool IsUpgrade(this ListUpgrade value)
    {
        return HasAttribute<UpgradeAttribute>(value);
    }
    #endregion

    #region String
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
        catch(Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(e.ToString());
#endif
            return Vector3Int.zero;
        }
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
    #endregion

    #region Vector3Int
    public static Vector3Int ZToZero(this Vector3Int vectorInt)
    {
        vectorInt.z = 0;
        return vectorInt;
    }

    public static string ToPositionString(this Vector3Int value)
    {
        return string.Format("{0},{1},{2}", value.x, value.y, value.z);
    }
    #endregion

    #region Vector3
    public static Vector3 AddY(this Vector3 vector3, float y)
    {
        vector3.y += y;
        return vector3;
    }
    #endregion

    #region RectTransform
    public static Vector2 Size(this RectTransform trans)
    {
        return new Vector2(trans.rect.width, trans.rect.height);
    }

    public static void SetPivotWithoutChangePosition(this RectTransform trabs, Vector2 pivot)
    {
        Vector3 deltaPosition = (trabs.pivot - pivot) * trabs.Size();
        trabs.pivot = pivot;
        trabs.localPosition = trabs.localPosition - deltaPosition;
    }
    #endregion

    #region Generic Type
    public static void RemoveNull<T>(this T ilist) where T : IList
    {
        for (int i = ilist.Count - 1; i >= 0; i--)
        {
            if (ilist[i] == null)
                ilist.RemoveAt(i);
        }
    }

    #endregion

}