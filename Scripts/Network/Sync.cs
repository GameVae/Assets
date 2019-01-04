using EnumCollect;
using System;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync",menuName = @"Conn/Sync",order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public ResourceInfo ResInfo;
        public Level Levels;
        public UpgradeInfo UpgradeInfo;

        private float researchTimer;
        private float upgradeTimer;

        public void Update()
        {
            if(UpgradeInfo.ResearchRemainingInt > 0)
            {
                researchTimer += Time.deltaTime;
                if(researchTimer >= 1.0f)
                {
                    UpgradeInfo.ResearchRemainingInt -= 1;
                    researchTimer -= 1.0f;
                    if(UpgradeInfo.ResearchRemainingInt <= 0)
                    {
                        UpgradeInfo.ResearchRemainingInt = 0;
                        Levels.CurrentResearchLv++;
                    }
                }
            }

            if (UpgradeInfo.UpgradeRemainingInt > 0)
            {
                upgradeTimer += Time.deltaTime;
                if (upgradeTimer >= 1.0f)
                {
                    UpgradeInfo.UpgradeRemainingInt -= 1;
                    upgradeTimer -= 1.0f;
                    if(UpgradeInfo.UpgradeRemainingInt <= 0)
                    {
                        UpgradeInfo.UpgradeRemainingInt = 0;
                        Levels.CurrentUpgradeLv++;
                    }
                }
            }
        }
    }

    [Serializable]
    public class ResourceInfo
    {
        public int Food;
        public int Wood;
        public int Stone;
        public int Metal;
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
    public class UpgradeInfo
    {
        public ListUpgrade UpgradeType;
        public ListUpgrade ResearchType;

        public string ResearchRemainingStr;
        public string UpgradeRemainingStr;

        public int ResearchRemainingInt;
        public int UpgradeRemainingInt;
    }
}