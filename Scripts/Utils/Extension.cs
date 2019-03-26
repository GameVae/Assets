using CustomAttr;
using EnumCollect;
using Json;
using Json.Interface;
using Network.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public static T[] IgnoreInstanceComponent<T>(this T[] arr, T comp) where T : Component
    {
        List<T> newArr = new List<T>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].gameObject.GetInstanceID() != comp.gameObject.GetInstanceID())
                newArr.Add(arr[i]);
        }
        return newArr.ToArray();
    }

    #endregion

    #region List
    public static List<T> Invert<T>(this List<T> list)
    {
        List<T> rs = new List<T>();
        int count = list.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            rs.Add(list[i]);
        }
        return rs;
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

    public static bool SerPositionValidate(this string value)
    {
        return value.Parse3Int() != Generic.Contants.Constants.InvalidPosition;
    }

    public static Vector3Int Parse3Int(this string value)
    {
        try
        {
            string[] xyz = value.Split(',');
            return new Vector3Int(int.Parse(xyz[0]), int.Parse(xyz[1]), int.Parse(xyz[2]));
        }
        catch //(Exception e)
        {
#if UNITY_EDITOR
            // Debug.Log(e.ToString());
#endif
            return Vector3Int.one * -1;
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

    public static Vector3Int ToClientPosition(this Vector3Int value)
    {
        return value + Generic.Contants.Constants.ToClientPosition;
    }

    public static Vector3Int ToSerPosition(this Vector3Int value)
    {
        return value + Generic.Contants.Constants.ToSerPosition;
    }
    #endregion

    #region Vector3
    public static Vector3 AddY(this Vector3 vector3, float y)
    {
        vector3.y += y;
        return vector3;
    }

    public static Vector3 Truncate(this Vector3 vector3, float maxMagnitude)
    {
        return vector3.magnitude < maxMagnitude ? vector3 : vector3.normalized * maxMagnitude;
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

    public static T Wrap<T>(this T value, T min, T max)
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }

    public static void Log<T>(this T values, string separator = "")
        where T : IEnumerable
    {
        string msg = "";
        bool first = true;
        foreach (var item in values)
        {
            if (first)
            {
                msg = item.ToString();
                first = false;
            }
            else
                msg += separator + item.ToString();
        }
        Debugger.Log(msg);
    }
    #endregion

    #region Camera
    public static bool CalculateFrustumOnPlane(this Camera Cam,
        ref Vector3[] conners,
        ref Vector3 center,
        ref float width,
        ref float height)
    {
        if (Physics.Raycast(
            origin: Cam.transform.position,
            direction: Cam.transform.forward,
            hitInfo: out RaycastHit hitInfo,
            maxDistance: Cam.farClipPlane))
        {
            float distance = Vector3.Distance(hitInfo.point, Cam.transform.position);
            float frustumHeight = 2.0f * distance * Mathf.Tan(Cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * Cam.aspect;

            Vector3 botLeft = hitInfo.point - new Vector3(frustumWidth * 0.5f, 0, frustumHeight * 0.5f);
            Vector3 topLeft = botLeft + new Vector3(0, 0, frustumHeight);
            Vector3 topRight = topLeft + new Vector3(frustumWidth, 0, 0);
            Vector3 botRight = topRight - new Vector3(0, 0, frustumHeight);

            conners = new Vector3[]
            { botLeft,
             topLeft,
             topRight,
             botRight };

            width = frustumWidth;
            height = frustumHeight;
            center = hitInfo.point;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region GameObject
    public static T AddComponentNotExist<T>(this GameObject gameObject)
        where T : Component
    {
        T checker = gameObject.GetComponent<T>();
        if (checker == null)
            return gameObject.AddComponent<T>();
        return checker;
    }
    #endregion

    #region Algorithm

    #region Binary Search and Binary Insert Sort

    private static int SearchInsertIndex<T>(List<T> list, int low, int high, object value)
        where T : IComparable
    {
        if (low == high) return low;
        int mid = low + (high - low) / 2;

        int comparer = list[mid].CompareTo(value);
        if (comparer > 0)
            return SearchInsertIndex(list, low, mid, value);
        else if (comparer < 0)
            return SearchInsertIndex(list, mid + 1, high, value);
        return mid;
    }

    public static int BinarySearch<T>(this List<T> list, int low, int high, object value)
        where T : IComparable
    {
        return SearchInsertIndex(list, low, high, value);
    }

    public static void BinarySort<T>(this List<T> list)
        where T : IComparable
    {
        int count = list.Count;
        for (int i = 1; i < count; i++)
        {
            T value = list[i];
            int index = SearchInsertIndex(list, 0, i, value);

            list.RemoveAt(i);
            list.Insert(index, value);
        }
    }

    #endregion

    #endregion


}