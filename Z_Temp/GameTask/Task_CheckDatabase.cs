using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityGameTask;

public class Task_CheckDatabase : IGameTask
{
    private bool isDone = false;
    private float progress;

    private CopyAssetsAndroid androidCopy;
    private SQLiteLocalLink sqliteLinks;

    public bool IsDone
    {
        get { return isDone; }
        private set { isDone = value; }
    }
    public float Progress
    {
        get { return progress; }
        private set { progress = value; }
    }

    public CopyAssetsAndroid AndroidCopy
    {
        get
        {
            return androidCopy ?? (androidCopy = Singleton.Instance<CopyAssetsAndroid>());
        }
    }

    public Task_CheckDatabase(SQLiteLocalLink links)
    {
        sqliteLinks = links;
    }

    public IEnumerator Action()
    {
        List<SQLiteConnectFactory.Link> links = sqliteLinks.Links;
        int completed = 0;
        int capacity = links.Count;

        IsDone = false;
        Progress = 0.0f;

        while (completed < capacity)
        {
            string assetPath = links[completed].DBPath;
            string persistentPath = UnityPath.Combinate(assetPath, UnityPath.AssetPath.Persistent);

            if (!UnityPath.Exist(persistentPath))
            {
                yield return AndroidCopy.Copy(assetPath, persistentPath);
            }

            completed++;
            Progress = completed * 1.0f / capacity;
            //Debugger.Log("Copy progress: " + Progress);
        }
        IsDone = true;
        yield break;
    }
}
