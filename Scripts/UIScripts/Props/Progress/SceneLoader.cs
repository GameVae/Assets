using Generic.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGameTask;

namespace UI
{
    public sealed class SceneLoader : MonoSingle<SceneLoader>,
        IGameTask
    {
        private AsyncOperation asyncOperation;
        private int loadAtIndex;

        public bool IsDone { get; private set; }
        public float Progress { get; private set; }
       
        private IEnumerator LoadSceneAt(int index)
        {
            IsDone = false;
            Progress = 0.0f;

            asyncOperation = SceneManager.LoadSceneAsync(index);
            asyncOperation.allowSceneActivation = true;

            while (Progress < 1f || !asyncOperation.isDone)
            {
                Progress = (asyncOperation.progress / 0.9f);
                yield return null;
            }

            IsDone = true;
            asyncOperation = null;

            yield break;
        }

        public IEnumerator Action()
        {
            return LoadSceneAt(loadAtIndex);
        }

        public void AutoScene()
        {
            asyncOperation.allowSceneActivation = true;
        }

        public void SetLoadScene(int index)
        {
            loadAtIndex = index;
        }
    }
}