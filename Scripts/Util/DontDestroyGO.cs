using UnityEngine;

namespace Utils
{
    public sealed class DontDestroyGO : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}