﻿using System.Collections;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
    public GUIInteractableIcon ZoomSpeedUp;
    public GUIInteractableIcon ZoomSpeedDown;
    public CameraPosition CameraCtrl;

    private void Awake()
    {
        ZoomSpeedUp.OnClickEvents += CameraCtrl.IncreaseZoom;
        ZoomSpeedDown.OnClickEvents += CameraCtrl.DecreaseZoom;
    }

}