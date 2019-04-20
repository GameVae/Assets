using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAsyncHandler
{
    Json.AJPHelper.Operation Start(object info);
}
