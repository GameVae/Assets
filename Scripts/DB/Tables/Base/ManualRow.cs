using ManualTable.Interface;
using UnityEngine;

namespace ManualTable.Row
{
    public enum RowType
    {
        MainBase,
        Version,
        Soldier
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

        public int FieldCount
        {
            get { return 10; }
        }

        public string ValuesSequence
        {
            get
            {
                string result = "";
                result += string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin ?? "0", TimeInt, Required, Unlock ?? "0");
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
                                        "Unlock = \"{9}\"",
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin, TimeInt, Required, Unlock);
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
    public class RSS_PositionRow : IManualRow
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
    public class SoldierRow : IManualRow
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
}