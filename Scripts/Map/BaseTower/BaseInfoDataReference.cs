using Generic.Singleton;
using ManualTable;
using Map;
using Network.Sync;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInfoDataReference : MonoSingle<BaseInfoDataReference>
{
    public Sync SyncData;
    public TowerSpawnManager TowerSpawner;
    public PlayerInfo Player;

    private BasePlayerJSONTable basePlayer;

    private void Start()
    {
        Player = FindObjectOfType<PlayerInfo>();
        TowerSpawner = Singleton.Instance<TowerSpawnManager>();
        basePlayer = SyncData.BasePlayerTable;
        InitBaseTower();
    }

    private void CreateBase(string name, int lv, string postion)
    {
        GameObject tower = TowerSpawner.GetTower(EnumCollect.TowerType.Base);
        tower.SetActive(true);

        tower.GetComponent<BaseTower>().SetPosition(postion.Parse3Int().ToClientPosition());

        tower.GetComponentInChildren<LookAt>().Target = Camera.main.transform;
        TowerLabel label = tower.GetComponent<TowerLabel>();

        label.Name.text = name;
        label.Lv.text = "Lv." + lv.ToString();

    }

    private void InitBaseTower()
    {
        CreateBase(Player.Info?.NameInGame,
            SyncData.CurrentBaseUpgrade[EnumCollect.ListUpgrade.MainBase].Level,
            Player.BaseInfo?.Position);
        for (int i = 0; i < basePlayer.Count; i++)
        {
            CreateBase(basePlayer.Rows[i].NameInGame, basePlayer.Rows[i].Level, basePlayer.Rows[i].Position);
        }
    }
}
