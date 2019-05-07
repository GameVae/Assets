using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqliteFileValidator : MonoBehaviour
{
    private bool isDone = false;
    private CopyAssetsAndroid androidCopy;

    public SQLiteLocalLink SqliteLinks;

    public bool IsDone
    {
        get
        {
            return isDone;
        }
        private set { isDone = value; }
    }
    public CopyAssetsAndroid AndroidCopy
    {
        get
        {
            return androidCopy ?? (androidCopy = Singleton.Instance<CopyAssetsAndroid>());
        }
    }

    public void OnStared()
    {
        StartCoroutine(CheckFileIsExist());
    }

    private IEnumerator CheckFileIsExist()
    {
        List<SQLiteConnectFactory.Link> links = SqliteLinks.Links;
        int linkCount = links.Count - 1;

        while (linkCount >= 0)
        {
            string assetPath = links[linkCount].DBPath;
            string persistentPath = UnityPath.Combinate(assetPath, UnityPath.AssetPath.Persistent);

            if (!UnityPath.Exist(persistentPath))
            {
                AndroidCopy.Copy(assetPath, persistentPath);
                while (!AndroidCopy.IsDone)
                {
                    yield return null;
                }
            }
            linkCount--;
        }

        IsDone = true;
        yield break;
    }
}
