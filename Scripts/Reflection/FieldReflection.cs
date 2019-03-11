using Generic.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public sealed class FieldReflection : ISingleton
{
    private FieldReflection()
    {
        catching = new Dictionary<Type, Dictionary<string, FieldInfo>>();
    }

    private Dictionary<Type, Dictionary<string, FieldInfo>> catching;

    public T GetField<T>(object obj, string fieldName, BindingFlags bindingFlags)
    {
        Type type = obj.GetType();
        if (TryGet(type, fieldName, out FieldInfo info))
        {
            return (T)info.GetValue(obj);
        }
        else
        {
            info = type.GetField(fieldName, bindingFlags);
            if (info != null)
            {
                Catch(type, fieldName, info);
            }
            return (T)info.GetValue(obj);
        }
    }

    private void Catch(Type type, string fieldName, FieldInfo info)
    {
        if(!catching.TryGetValue(type, out Dictionary<string,FieldInfo> dict))
        {
            catching[type] = new Dictionary<string, FieldInfo>();
        }
        catching[type][fieldName] = info;
    }

    private bool TryGet(Type type, string fieldName, out FieldInfo info)
    {
        if (catching.TryGetValue(type, out Dictionary<string, FieldInfo> dict))
        {
            if (dict.TryGetValue(fieldName, out info))
            {
                return true;
            }
        }
        info = null;
        return false;
    }
}
