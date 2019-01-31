using Json.Interface;
using UnityEngine;

namespace Json
{
    public abstract class JSONBase : IJSON
    {      
        public string ToJSON()
        {
            return JsonUtility.ToJson(this);
        }
    }
}