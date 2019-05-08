#if UNITY_ANDROID
using Generic.Singleton;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class CopyAssetsAndroid : MonoSingle<CopyAssetsAndroid>
{
    private bool isDone = true;
    public bool IsDone
    {
        get
        {
            return isDone;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from">file inside streaming assets folder</param>
    /// <param name="to">create or override file at</param>
    /// <returns></returns>
    private IEnumerator CopyFileOnAndroid(string from,string to)
    {
        isDone = false;
        UnityWebRequest loader = UnityWebRequest.Get(UnityPath.Combinate(from, UnityPath.AssetPath.StreamingAsset));
        yield return loader.SendWebRequest();

        byte[] readbytes = loader.downloadHandler.data;
        loader.Dispose();

        UnityPath.CreateFileAnywhere(to);
        DirectoryInfo dirInfo = new DirectoryInfo(UnityPath.GetDirectory(to));
        dirInfo.Attributes = FileAttributes.Hidden;


        if(UnityPath.FileOrDirectory(to) == 0) // target path is file
        {
            File.WriteAllBytes(to, readbytes);
            Debugger.Log("Copy from: " + from + " to: " + to + " data size: " + readbytes.Length);
        }
        isDone = true;
    }

    /// <summary>
    /// Copy file inside .apk file and save at specified target
    /// </summary>
    /// <param name="from">file inside streaming assets folder</param>
    /// <param name="to">create or override file at</param>
    /// <returns></returns>
    public void Copy(string from,string to)
    {
        if (IsDone)
        {
            StartCoroutine(CopyFileOnAndroid(from, to));
        }
    }
}
#endif
