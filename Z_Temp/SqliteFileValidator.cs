using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityGameTask;

public class SqliteFileValidator : IGameTask
{
    private bool isDone = false;
    private float progress;

    private CopyAssetsAndroid androidCopy;
    private SQLiteLocalLink sqliteLinks;


    public bool IsDone
    {
        get
        {
            return isDone;
        }
        private set { isDone = value; }
    }  
    public float Progress
    {
        get
        {
            return progress;
        }
        private set
        {
            progress = value;
        }
    }
    public CopyAssetsAndroid AndroidCopy
    {
        get
        {
            return androidCopy ?? (androidCopy = Singleton.Instance<CopyAssetsAndroid>());
        }
    }

    public SqliteFileValidator(SQLiteLocalLink links)
    {
        sqliteLinks = links;
    }
    public IEnumerator Action()
    {
        List<SQLiteConnectFactory.Link> links = sqliteLinks.Links;
        int linkCount = links.Count - 1;
        int capacity = linkCount + 1;

        IsDone = false;
        Progress = 0.0f;

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
            Progress = 1.0f - ((linkCount + 1) * 1.0f / capacity);
            Debugger.Log("Sqlite: " + Progress);
            yield return null;
        }

        IsDone = true;
        Progress = 1.0f;
        yield break;
    }
}
