using ManualTable.Interface;
using UnityEngine;

namespace ManualTable.Row
{
    public enum RowType
    {
        MainBase,
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
}