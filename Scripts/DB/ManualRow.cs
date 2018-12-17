using ManualTable.Interface;
using UnityEngine;

namespace ManualTable.Row
{
    public enum RowType
    {
        MainBase,
        Version
    }

    [System.Serializable]
    public class MainBaseRow : IManualRow
    {
        [SerializeField] public int Level;
        [SerializeField] public int MightBonus;
        [SerializeField] public int FoodCost;
        [SerializeField] public int WoodCost;
        [SerializeField] public int StoneCost;
        [SerializeField] public int MetalCost;
        [SerializeField] public string TimeMin;
        [SerializeField] public int TimeInt;
        [SerializeField] public int Required;
        [SerializeField] public string Unlock;

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
        [SerializeField] public int Id;
        [SerializeField] public string Task;
        [SerializeField] public string Content;
        [SerializeField] public string Comment;

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
        [SerializeField] public int ID;
        [SerializeField] public int RssType;
        [SerializeField] public int Level;
        [SerializeField] public string Position;
        [SerializeField] public int Quality;
        [SerializeField] public int Region_Position;
        [SerializeField] public int ID_Player;
        [SerializeField] public int ID_Base;
        [SerializeField] public int ID_Unit;
        [SerializeField] public string TimePrepare;
        [SerializeField] public string TimeHarvestFinish;
        [SerializeField] public string TimeRemove;

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
}