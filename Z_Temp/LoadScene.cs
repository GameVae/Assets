using Generic.Pooling;
using Generic.Singleton;
using System.Collections;
using UnityGameTask;
using UI;

public class LoadScene : IGameTask, IPoolable
{
    public bool IsDone
    {
        get{ return sceneLoader.IsActiveDone; }
    }
    public int ManagedId
    {
        get; private set;
    }
    public float Progress
    {
        get; private set;
    }

    private int refIndex;
    private SceneLoader sceneLoader;

    public LoadScene(int index,SceneLoader _sceneLoader)
    {
        Progress = 0.0f;
        refIndex = index;
        sceneLoader = _sceneLoader;
    }

    public IEnumerator Action()
    {
        sceneLoader.LoadScene(refIndex);
        sceneLoader.ActiveScene();

        while (!sceneLoader.IsActiveDone)
        {
            Progress = sceneLoader.Progress;
            yield return null;
        }
        Progress = 1.0f;
        yield break;
    }

    public void Dispose()
    {
        Progress = 0.0f;
        refIndex = -1;
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
        Progress = 0.0f;
    }

    public void SetRefIndex(int index)
    {
        refIndex = index;
    }
}
