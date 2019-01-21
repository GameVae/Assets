using Json.Interface;
using UnityEngine;

namespace Json
{
    public abstract class JSONBase : IJSON
    {
        public static T FromJSON<T>(string json) where T : new()
        {
            return JsonUtility.FromJson<T>(json);
        }

        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}