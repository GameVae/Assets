using System.Collections.Generic;
using System.Reflection;

namespace Generic.Singleton
{
    public class Singleton
    {
        private static Dictionary<int, ISingleton> singletons;

        /// <summary>
        /// Get Singleton Instance for parameter type
        /// </summary>
        /// <typeparam name="T">Implement ISingleton</typeparam>
        /// <returns></returns>
        public static T Instance<T>() where T : ISingleton
        {
            if (singletons == null)
                singletons = new Dictionary<int, ISingleton>();
            singletons.TryGetValue(typeof(T).GetHashCode(), out ISingleton value);

            if (value == null)
            {
                System.Type type = typeof(T);
                if (type.IsSubclassOf(typeof(UnityEngine.MonoBehaviour)))
                {
                    value = CompInstance<T>();
                }
                else
                {
                    value = NormalInstance<T>();
                }
                singletons[type.GetHashCode()] = value;
            }
            return (T)value;
        }

        #region Get/Create new Singleton for Unity Component
        /// <summary>
        /// Get/Create new Singleton for Unity Component
        /// </summary>
        /// <typeparam name="T">Type of class that delivered from MonoSingle<T> </typeparam>
        /// <returns></returns>
        public static T CompInstance<T>(System.Type type = null) where T : ISingleton
        {
            if (type == null)
                type = typeof(T);

            PropertyInfo property = type.GetProperty("Instance",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.GetProperty);
            if (property == null)
            {
                System.Type baseType = type.BaseType;
                return baseType == null ? default(T) : CompInstance<T>(baseType);
            }
            else
            {
                ISingleton value = (ISingleton)property.GetValue(null, null);
#if UNITY_EDITOR
                UnityEngine.Debug.Log(value + " found: " + type.GetHashCode());
#endif
                return (T)value;
            }
        }
        #endregion

        #region Get/Create new Singleton for normal class
        /// <summary>
        /// Get/Create new Singleton for normal class
        /// </summary>
        /// <typeparam name="T">Type of class that contain PRIVATE CONSTRUCTOR</typeparam>
        /// <returns></returns>
        private static T NormalInstance<T>() where T : ISingleton
        {
            System.Type type = typeof(T);
            ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            if (constructors.Length == 0)
            {
                throw new System.Exception("Private constructor not found");
            }
            else
            {
                ISingleton value = (ISingleton)constructors[0].Invoke(new object[] { });
                return (T)value;
            }
        }
        #endregion
    }
}