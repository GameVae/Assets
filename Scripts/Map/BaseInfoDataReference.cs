using Generic.Singleton;
using ManualTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfoDataReference : MonoSingle<BaseInfoDataReference>
{
    public BaseInfoJSONTable BaseInfo;
    public TowerSpawnManager TowerSpawner;
}
