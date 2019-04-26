using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

using UI.Keyboard;
using static UI.Keyboard.Keyboard;


public class KeyboardFactory : ISingleton
{   
    private Dictionary<KeyboardType, Keyboard> keyboards;
    private KeyboardFactory()
    {
        keyboards = new Dictionary<KeyboardType, Keyboard>();
    }

    public Keyboard GetKeyboard(KeyboardType type)
    {
        keyboards.TryGetValue(type, out Keyboard keyboard);
        if (keyboard == null)
        {
            keyboard = CreateKeyboard(type);
            if (keyboard != null)
                keyboards[type] = keyboard;
        }
        return keyboard;
    }

    private Keyboard CreateKeyboard(KeyboardType type)
    {        
        switch (type)
        {
            case KeyboardType.Standalone:
                return new GameObject("Keyboard::" + type.ToString())
                    .AddComponent<StandaloneKeyboard>();
            case KeyboardType.MobileNative:
                return new GameObject("Keyboard::" + type.ToString())
                    .AddComponent<MobileNativeKeyboard>();
            case KeyboardType.Numpad:
                return Resources.FindObjectsOfTypeAll<Numpad>()[0];
        }
        return null;
    }
}
