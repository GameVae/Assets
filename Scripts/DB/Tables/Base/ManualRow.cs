using EnumCollect;
using Json;
using ManualTable.Interface;
using UnityEngine;

namespace ManualTable.Row
{
    public enum DBRowType
    {
        MainBase,
        Version,
        Military
    }

    [System.Serializable]
    public class MainBaseRow : IManualRow
    {
        public int Level;
        public int MightBonus;
        public int FoodCost;
        public int WoodCost;
        public int StoneCost;
        public int MetalCost;
        public string TimeMin;
        public int TimeInt;
        public int Required;
        public string Unlock;
        public ListUpgrade Unlock_ID;

        public int FieldCount
        {
            get { return 11; }
        }

        public string ValuesSequence
        {
            get
            {
                string result = "";
                result += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin ?? "0", TimeInt, Required, Unlock ?? "0",Unlock_ID);
                return result;
            }
        }

        public string KeyValuePairs
        {
            get
            {
                string result = "";
                result += string.Format("Level = {0}," +
                                        "MightBonus = {1}," +
                                        "FoodCost = {2}," +
                                        "WoodCost = {3}," +
                                        "StoneCost = {4}," +
                                        "MetalCost = {5}," +
                                        "TimeMin = \"{6}\"," +
                                        "TimeInt = {7}," +
                                        "Required = {8}," +
                                        "Unlock = \"{9}\"," +
                                        "Unlock_ID = {10}",
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin, TimeInt, Required, Unlock,Unlock_ID);
                return result;
            }
        }
    }

    [System.Serializable]
    public class VersionRow : IManualRow
    {
        public int Id;
        public string Task;
        public string Content;
        public string Comment;

        public int FieldCount { get { return 4; } }

        public string ValuesSequence
        {
            get
            {
                string result = "";
                result += string.Format("{0},{1},{2},{3}",
                    Id, Task ?? "0", Content ?? "0", Comment ?? "0");
                return result;
            }
        }

        public string KeyValuePairs
        {
            get
            {
                string result = "";
                result += string.Format("Id = {0}," +
                                        "Task = \"{1}\"," +
                                        "Content = \"{2}\"," +
                                        "Comment = \"{3}\"", Id, Task, Content, Comment);
                return result;
            }
        }
    }   

    [System.Serializable]
    public class MilitaryRow : IManualRow
    {
        public int FieldCount { get { return 12; } }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }

        public int Level;
        public int TrainingTime;
        public int MightBonus;
        public float Attack;
        public float Defend;
        public float Health;
        public int Food;
        public int Wood;
        public int Stone;
        public int Metal;
        public string ResearchTime;
        public int TimeInt;
        public string Required;
    }

    //======================================//
    [System.Serializable]
    public class RSS_PositionRow : JSONBase, IManualRow
    {
        public int ID;
        public int RssType;
        public int Level;
        public string Position;
        public int Quality;
        public int Region_Position;
        public int ID_Player;
        public int ID_Base;
        public int ID_Unit;
        public string TimePrepare;
        public string TimeHarvestFinish;
        public string TimeRemove;

        public int FieldCount
        {
            get
            {
                return 12;
            }
        }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }
    }

    [System.Serializable]
    public class PositionRow : JSONBase, IManualRow
    {
        public int ID;
        public string Position_Transform;
        public string Position_Cell;
        public string ID_Type;
        public string Comment;

        public int FieldCount { get { return 5; } }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }
    }

    [System.Serializable]
    public class BaseUpgradeRow : JSONBase, IManualRow
    {
        public int FieldCount { get { return 4; } }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }

        public ListUpgrade ID;
        public string Name_Upgrade;
        public int Level;
        public int UpgradeType;
    }

    [System.Serializable]
    public class BaseInfoRow : JSONBase, IManualRow
    {
        public int FieldCount { get { return 22; } }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }


        public int ID_User;
        public int BaseNumber;
        public string Position;

        public int Farm;
        public int Wood;
        public int Stone;
        public int Metal;

        public ListUpgrade UpgradeWait_ID;
        public string UpgradeWait_Might;
        public float UpgradeTime;

        public ListUpgrade ResearchWait_ID;
        public string ResearchWait_Might;
        public float ResearchTime;

        public string UnitTransferType;
        public string UnitTransferQuality;
        public string UnitTransferTime;
        public string UnitTransfer_ID_Base;
        public string TrainingUnit_ID;
        public string TrainingTime;
        public string TrainingQuality;
        public string Training_Might;
        public int SumUnitQuality;

        ///*===============Util Funs==================*/
        public void RecordElapsedTime(float elapsedTime, BaseUpgradeJSONTable baseUpgrade)
        {
            if (UpgradeTime > 0)
            {
                UpgradeTime -= elapsedTime;
                if (UpgradeTime <= 0)
                {
                    UpgradeTime = 0;
                    baseUpgrade[UpgradeWait_ID].Level++;
                    UpgradeWait_ID = 0;
                }
            }
            if(ResearchTime > 0)
            {
                ResearchTime -= elapsedTime;
                if (ResearchTime <= 0)
                {
                    ResearchTime = 0;
                    baseUpgrade[ResearchWait_ID].Level++;
                    ResearchWait_ID = 0;
                }
            }
        }

        public string GetResTimeString()
        {
            return ResearchWait_ID.ToString().InsertSpace() + " " +
                System.TimeSpan.FromSeconds(Mathf.RoundToInt(ResearchTime)).ToString();
        }

        public string GetUpgTimeString()
        {
            return UpgradeWait_ID.ToString().InsertSpace() + " " +
                System.TimeSpan.FromSeconds(Mathf.RoundToInt(UpgradeTime)).ToString();
        }

        public bool ResIsDone()
        { return ResearchTime <= 0; }

        public bool UpgIsDone()
        { return UpgradeTime <= 0; }
    }

    [System.Serializable]
    public class UserInfoRow : JSONBase, IManualRow
    {
        public int FieldCount { get { return 9; } }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }

        public int ID_User;
        public string Server_ID;
        public string NameInGame;
        public string ChatWorldColor;
        public string Guild_ID;
        public string Guild_Name;
        public string LastGuildID;
        public int Might;
        public int Killed;
    }
}