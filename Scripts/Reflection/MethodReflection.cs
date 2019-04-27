using Generic.Singleton;
using System;
using System.Reflection;

public sealed class MethodReflection : ISingleton
{
    public readonly Type DelegateType;

    private MethodReflection()
    {
        DelegateType = typeof(Delegate);
    }

    /// <summary>
    /// Get method and create delegate base on <paramref name="delegateType"/>
    /// </summary>
    /// <param name="delegateType">type of Delegate</param>
    /// <param name="funcName">Unique method name</param>
    /// <param name="container">target</param>
    /// <param name="flags">Reflection binding flags</param>
    /// <returns></returns>
    public Delegate CreateDelegate(Type delegateType, string funcName, object container,BindingFlags flags)
    {
        Type containerType = container.GetType();
        if (delegateType.IsSubclassOf(DelegateType))
        {
            MethodInfo methodInfo = containerType.GetMethod(funcName, flags);
            return Delegate.CreateDelegate(delegateType, container, methodInfo.Name);
        }
        else
        {
            throw new Exception(delegateType + " is not type of Delegate");
        }
    }
}
