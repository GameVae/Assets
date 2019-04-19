﻿using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;
using static Keyboard;

public class KeyboardProvider : ISingleton
{   
    private Dictionary<KeyboardType, Keyboard> keyboards;
    private KeyboardProvider()
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
        GameObject keyboardObject = new GameObject("Keyboard::" + type.ToString());
        switch (type)
        {
            case KeyboardType.Standalone:
                return keyboardObject.AddComponent<StandaloneKeyboard>();
            case KeyboardType.MobileNative:
                return keyboardObject.AddComponent<MobileNativeKeyboard>();
            case KeyboardType.Numpad:
                return Resources.FindObjectsOfTypeAll<Numpad>()[0];
        }
        return null;
    }
}