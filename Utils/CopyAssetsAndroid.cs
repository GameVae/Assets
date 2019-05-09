#if UNITY_ANDROID
using Generic.Singleton;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class CopyAssetsAndroid : MonoSingle<CopyAssetsAndroid>
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from">file inside streaming assets folder</param>
    /// <param name="to">create or override file at</param>
    /// <returns></returns>
    private IEnumerator CopyFileOnAndroid(string from, string to)
    {
        UnityWebRequest loader = UnityWebRequest.Get(UnityPath.Combinate(from, UnityPath.AssetPath.StreamingAsset));
        yield return loader.SendWebRequest();

        byte[] readbytes = loader.downloadHandler.data;
        loader.Dispose();

        UnityPath.CreateFileAnywhere(to);
        DirectoryInfo dirInfo = new DirectoryInfo(UnityPath.GetDirectory(to))
        {
            Attributes = FileAttributes.Hidden
        };


        if (UnityPath.FileOrDirectory(to) == 0) // target path is file
        {
            File.WriteAllBytes(to, readbytes);
            Debugger.Log("Copy from: " + from + " to: " + to + " data size: " + readbytes.Length);
        }
    }

    /// <summary>
    /// Copy file inside .apk file and save at specified target
    /// </summary>
    /// <param name="from">file inside streaming assets folder</param>
    /// <param name="to">create or override file at</param>
    /// <returns></returns>
    public IEnumerator Copy(string from, string to)
    {
        yield return CopyFileOnAndroid(from, to);
    }
}
#endif
