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

        public void Update(float deltaTime)
        {
            BaseInfo.UpdateTime(deltaTime,Levels);
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

        public int ResRemainingInt;
        public int UpgRemainingInt;

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

        /*===============Util Funs==================*/
        private float researchTimer;
        private float upgradeTimer;

        public string GetResTimeString()
        {
            return ResearchWait_ID.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(ResRemainingInt)).ToString();
        }

        public string GetUpgTimeString()
        {
            return UpgradeWait_ID.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(UpgRemainingInt)).ToString();
        }

        public bool ResIsDone()
        { return ResRemainingInt <= 0; }

        public bool UpgIsDone()
        { return UpgRemainingInt <= 0; }

        public void UpdateTime(float deltaTime,Level levels)
        {
            if (ResRemainingInt > 0)
            {
                researchTimer += deltaTime;
                if (researchTimer >= 1.0f)
                {
                    ResRemainingInt -= 1;
                    researchTimer -= 1.0f;
                    if (ResIsDone())
                    {
                        ResRemainingInt = 0;
                        levels.CurrentResearchLv++;
                    }
                }
            }
            if (UpgRemainingInt > 0)
            {
                upgradeTimer += deltaTime;
                if (upgradeTimer >= 1.0f)
                {
                    UpgRemainingInt -= 1;
                    upgradeTimer -= 1.0f;
                    if (UpgIsDone())
                    {
                        UpgRemainingInt = 0;
                        levels.CurrentUpgradeLv++;
                    }
                }
            }
        }
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