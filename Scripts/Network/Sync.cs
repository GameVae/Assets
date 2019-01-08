using EnumCollect;
using System;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public Level Levels;
        public UserInfo UserInfo;
        public BaseInfo BaseInfo;

        private float researchTimer;
        private float upgradeTimer;

        public void Update()
        {
            if (BaseInfo.ResearchRemainingInt > 0)
            {
                researchTimer += Time.deltaTime;
                if (researchTimer >= 1.0f)
                {
                    BaseInfo.ResearchRemainingInt -= 1;
                    researchTimer -= 1.0f;
                    if (BaseInfo.ResearchRemainingInt <= 0)
                    {
                        BaseInfo.ResearchRemainingInt = 0;
                        Levels.CurrentResearchLv++;
                    }
                }
            }

            if (BaseInfo.UpgradeRemainingInt > 0)
            {
                upgradeTimer += Time.deltaTime;
                if (upgradeTimer >= 1.0f)
                {
                    BaseInfo.UpgradeRemainingInt -= 1;
                    upgradeTimer -= 1.0f;
                    if (BaseInfo.UpgradeRemainingInt <= 0)
                    {
                        BaseInfo.UpgradeRemainingInt = 0;
                        Levels.CurrentUpgradeLv++;
                    }
                }
            }
        }
    }

    [Serializable]
    public class Level
    {
        public int MainbaseLevel;

        public int CurrentUpgradeLv;
        public int CurrentResearchLv;

        public int UpgradeRequire;
        public int ResearchRequire;
    }

    [Serializable]
    public class BaseInfo
    {
        public int ID_User;
        public int BaseNumber;
        public string Position;

        public ListUpgrade UpgradeWait_ID;
        public ListUpgrade ResearchWait_ID;

        public string ResearchTime;
        public string UpgradeTime;

        public string UpgradeWait_Might;
        public string ResearchWait_Might;

        public int ResearchRemainingInt;
        public int UpgradeRemainingInt;

        public int Farm;
        public int Wood;
        public int Stone;
        public int Metal;

        public string UnitTransferType;
        public string UnitTransferQuality;
        public string UnitTransferTime;
        public string UnitTransfer_ID_Base;
        public string TrainingUnit_ID;
        public string TrainingTime;
        public string TrainingQuality;
        public string Training_Might;
        public int SumUnitQuality;
    }

    [Serializable]
    public class UserInfo
    {
        public int ID_User;
        public string NameInGame;
        public string ChatWorldColor;
        public string Guild_ID;
        public string Guild_Name;
        public string LastGuildID;
        public int Might;
        public int Killed;
        public int Server_ID;
    }
}