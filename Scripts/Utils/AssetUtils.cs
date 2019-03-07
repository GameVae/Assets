using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class AssetUtils : MonoSingle<AssetUtils>
    {
        public Object[] Load(string path)
        {
            Object[] allAssets = Resources.LoadAll(path);
            return allAssets;
        }

        //        public Object GetAsset(string name)
        //        {
        //            Object asset;
        //            assets.TryGetValue(name.GetHashCode(), out asset);
        //#if UNITY_EDITOR
        //            if (asset == null) Debugger.Log("Not found asset");
        //#endif
        //            return asset;

        //        }
    }
}