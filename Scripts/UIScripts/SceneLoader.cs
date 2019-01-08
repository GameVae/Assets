using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }
        private AsyncOperation asyncOperation;

        public bool IsActiveDone { get; private set; }
        public float Progress { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(Instance.gameObject);
        }

        public void LoadScene(int index)
        {
            IsActiveDone = false;
            Progress = 0.0f;
            StartCoroutine(StartLoadScene(index));
        }

        private IEnumerator StartLoadScene(int index)
        {
            asyncOperation = SceneManager.LoadSceneAsync(index);
            asyncOperation.allowSceneActivation = false;

            while (Progress < 1f || !asyncOperation.isDone)
            {
                Progress = (asyncOperation.progress / 0.9f);
                yield return null;
            }

            IsActiveDone = true;
            yield break;
        }

        public void ActiveScene()
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
}