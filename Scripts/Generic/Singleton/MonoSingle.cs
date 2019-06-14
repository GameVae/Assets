
using Utils;

namespace Generic.Singleton
{
    public abstract class MonoSingle<T> : UnityEngine.MonoBehaviour, ISingleton 
        where T : UnityEngine.Component 
    {
        private static ISingleton instance;
        public static ISingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    UnityEngine.Object ins = FindObjectOfType(typeof(T));
                    if (ins == null)
                        instance = (ISingleton)new UnityEngine.GameObject(typeof(T).ToString() + "Singleton",typeof(DontDestroyGO)).AddComponent<T>();
                    else
                        instance = (ISingleton)ins;
                }
                return instance;
            }
        }

        private void GuaranteeOnlyIns()
        {
            T[] insts = FindObjectsOfType<T>();
            //UnityEngine.Debug.Log("ins: "  + instance + " - " + Instance);
            for (int i = 0; i < insts.Length; i++)
            {
                if (i == 0) instance = (ISingleton)insts[i];
                else Destroy(insts[i].gameObject);
            }

        }

        protected virtual void Awake()
        {
            GuaranteeOnlyIns();
        }


        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("Singleton: " + GetType().ToString() + " destroyed");
#endif
        }
    }
}