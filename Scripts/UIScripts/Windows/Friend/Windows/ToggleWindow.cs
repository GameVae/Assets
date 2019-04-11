using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToggleWindow : BaseWindow
{
    public WindowToggleGroup Manager;

    public abstract override void Load(params object[] input);
    protected abstract override void Init();       
}
