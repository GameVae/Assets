using Generic.Singleton;
using System.Collections.Generic;
using System.Reflection;

public class SQLiteHelper : MonoSingle<SQLiteHelper>
{
    private Dictionary<int, List<FieldInfo>> fieldInfoStored;
    protected Dictionary<int, List<FieldInfo>> FieldInfoStored
    {
        get { return fieldInfoStored ?? (fieldInfoStored = new Dictionary<int, List<FieldInfo>>()); }
    }

    private void GetOrCreateFieldInfos(out List<FieldInfo> objFields, System.Type type)
    {
        FieldInfoStored.TryGetValue(type.GetHashCode(), out objFields);
        if (objFields == null)
        {
            objFields = new List<FieldInfo>(type.GetFields());
            FieldInfoStored[type.GetHashCode()] = objFields;
        }
    }

    private string CreateValuesSequenceFrom(List<FieldInfo> infos, object obj)
    {
        string rs = "";
        int count = infos.Count;
        for (int i = 0; i < count; i++)
        {
            rs += infos[i].GetValue(obj) + ((i < count - 1) ? ", " : "");
        }
        return rs;
    }

    private string CreateColumnSequenceFrom(List<FieldInfo> infos, object obj)
    {
        string rs = "";
        int count = infos.Count;
        for (int i = 0; i < count; i++)
        {
            rs += infos[i].Name + ((i < count - 1) ? ", " : "");
        }
        return rs;
    }

    private string CreateUpdateValuesFrom(List<FieldInfo> infos, object obj)
    {
        string rs = "";
        int count = infos.Count;
        for (int i = 0; i < count; i++)
        {
            rs += infos[i].Name + "=" + infos[i].GetValue(obj);
            if (i < count - 1)
                rs += ", ";
        }
        return rs;
    }
  
    /// <summary>
    /// Return sequence for UPDATE
    /// </summary>
    /// <param name="type">Type of DB row</param>
    /// <param name="obj">object</param>
    /// <returns> Return string values for INSERT into SQLite DB</returns>
    public string CreateUpdateValuesFrom(System.Type type, object obj)
    {
        GetOrCreateFieldInfos(out List<FieldInfo> infos, type);
        return CreateUpdateValuesFrom(infos, obj);
    }
    public string CreateUpdateValuesFrom(object obj)
    {
        return CreateUpdateValuesFrom(obj.GetType(), obj);
    }

    /// <summary>
    /// </summary>
    /// <param name="type">Type of DB row</param>
    /// <param name="obj">object</param>
    /// <returns> Return string values for INSERT into SQLite DB</returns>
    public string CreateValuesSequenceFrom(System.Type type, object obj)
    {
        GetOrCreateFieldInfos(out List<FieldInfo> infos, type);
        return CreateValuesSequenceFrom(type, obj);
    }
    public string CreateValuesSequenceFrom(object obj)
    {
        return CreateValuesSequenceFrom(obj.GetType(), obj);
    }

    /// <summary>
    /// </summary>
    /// <param name="type">Type of DB row</param>
    /// <param name="obj">object</param>
    /// <returns> Return string column for INSERT into SQLite DB</returns>
    public string CreateColumnSequenceFrom(System.Type type, object obj)
    {
        GetOrCreateFieldInfos(out List<FieldInfo> infos, type);
        return CreateColumnSequenceFrom(type, obj);
    }
    public string CreateColumnSequenceFrom(object obj)
    {
        return CreateColumnSequenceFrom(obj.GetType(), obj);
    }
}
