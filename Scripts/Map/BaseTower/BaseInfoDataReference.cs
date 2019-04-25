using Generic.Singleton;
using DataTable;
using Map;
using Network.Sync;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using DataTable.Row;

public class BaseInfoDataReference : MonoSingle<BaseInfoDataReference>
{
    public Sync SyncData;
    public TowerSpawnManager TowerSpawner;
    public PlayerInfo Player;

    private JSONTable_BasePlayer basePlayer;

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

        ReadOnlyCollection<BasePlayerRow> rows = basePlayer.ReadOnlyRows;
        for (int i = 0; i < basePlayer.Count; i++)
        {
            CreateBase(rows[i].NameInGame, rows[i].Level, rows[i].Position);
        }
    }
}
