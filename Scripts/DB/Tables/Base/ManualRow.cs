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
        Military,
        TrainningCost
    }

    [System.Serializable]
    public sealed class TrainningCostRow : JSONBase, IManualRow
    {
        public int ID;
        public ListUpgrade ID_Unit;
        public string Unit;
        public float Range;
        public float Speed;
        public int MightBonus;
        public int Storage;
        public int FoodCost;
        public int WoodCost;
        public int StoneCost;
        public int MetalCost;

        public int FieldCount
        {
            get { return 11; }
        }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }
    }

    #region old not use
    //[System.Serializable]
    //public sealed class GenericUpgradeInfo : JSONBase
    //{
    //    public int Level;
    //    public int MightBonus;
    //    public int FoodCost;
    //    public int WoodCost;
    //    public int StoneCost;
    //    public int MetalCost;
    //    public string TimeMin;
    //    public int TimeInt;
    //    public int Required;
    //}
    #endregion

    #region DB row
    [System.Serializable]
    public class MainBaseRow : JSONBase, IManualRow
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
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin ?? "0", TimeInt, Required, Unlock ?? "0", Unlock_ID);
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
                    Level, MightBonus, FoodCost, WoodCost, StoneCost, MetalCost, TimeMin, TimeInt, Required, Unlock, Unlock_ID);
                return result;
            }
        }
    }

    [System.Serializable]
    public class VersionRow : JSONBase, IManualRow
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
    public class MilitaryRow : JSONBase, IManualRow
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
        public int FoodCost;
        public int WoodCost;
        public int StoneCost;
        public int MetalCost;
        public string TimeMin;
        public int TimeInt;
        public string Required;
        public int Required_ID;
        public int RequiredLevel;
        public int Unlock_ID;
    }
    #endregion

    //======================================//

    [System.Serializable]
    public abstract class ServerMessage : JSONBase, IManualRow
    {
        public abstract int FieldCount { get; }

        public string ValuesSequence { get { return ""; } }

        public string KeyValuePairs { get { return ""; } }
    }

    [System.Serializable]
    public class RSS_PositionRow : ServerMessage
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

        public override int FieldCount
        {
            get
            {
                return 12;
            }
        }
    }

    [System.Serializable]
    public class PositionRow : ServerMessage
    {
        public int ID;
        public string Position_Transform;
        public string Position_Cell;
        public string ID_Type;
        public string Comment;

        public override int FieldCount { get { return 5; } }
    }

    [System.Serializable]
    public class BaseUpgradeRow : ServerMessage
    {
        public override int FieldCount { get { return 4; } }

        public ListUpgrade ID;
        public string Name_Upgrade;
        public int Level;
        public int UpgradeType;
    }

    [System.Serializable]
    public class BaseInfoRow : ServerMessage
    {
        public override int FieldCount { get { return 22; } }

        public int ID_User;
        public int BaseNumber;
        public string Position;

        public int Farm;
        public int Wood;
        public int Stone;
        public int Metal;

        public ListUpgrade UpgradeWait_ID;
        public int UpgradeWait_Might;
        public float UpgradeTime;

        public ListUpgrade ResearchWait_ID;
        public int ResearchWait_Might;
        public float ResearchTime;

        public string UnitTransferType;
        public string UnitTransferQuality;
        public string UnitTransferTime;
        public string UnitTransfer_ID_Base;
        public ListUpgrade TrainingUnit_ID;
        public double TrainingTime;
        public int TrainingQuality;
        public int Training_Might;
        public int SumUnitQuality;

        ///*===============Util Funs==================*/
        public string GetResTimeString()
        {
            return ResearchWait_ID.ToString().InsertSpace() + " " +
                System.TimeSpan.FromSeconds(Mathf.RoundToInt(ResearchTime)).ToString().Replace(".", "d ");
        }

        public string GetUpgTimeString()
        {
            return UpgradeWait_ID.ToString().InsertSpace() + " " +
                System.TimeSpan.FromSeconds(Mathf.RoundToInt(UpgradeTime)).ToString().Replace(".", "d ");
        }

        public bool ResIsDone()
        { return ResearchTime <= 0; }

        public bool UpgIsDone()
        { return UpgradeTime <= 0; }

        public bool IsEnoughtResource(int farm, int wood, int stone, int metal)
        {
            //Debug.Log(farm + "/" + Farm + "\n" +
            //    wood + "/" + Wood + "\n" +
            //    stone + "/" + Stone + "\n" +
            //    metal + "/" + Metal + "\n");
            return (Farm >= farm && Wood >= wood && Stone >= stone && Metal >= metal);
        }

    }

    [System.Serializable]
    public class UserInfoRow : ServerMessage
    {
        public override int FieldCount { get { return 9; } }

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

    [System.Serializable]
    public class BaseDefendRow : ServerMessage
    {
        public override int FieldCount { get { return 4; } }

        public int ID;
        public int BaseNumber;
        public ListUpgrade ID_Unit;
        public int Quality;
    }

    [System.Serializable]
    public class UnitRow : ServerMessage
    {
        public override int FieldCount
        {
            get { return 10; }
        }

        public int ID;
        public ListUpgrade ID_Unit;
        public int ID_User;
        public int BaseNumber;
        public int Level;
        public int Quality;
        public float Hea_cur;
        public float Health;
        public string Position_Cell;
        public string Next_Cell;
        public string End_Cell;
        public float TimeMoveNextCell;
        public float TimeFinishMove;
        public string ListMove;
        public int Status;
        public int Attack_Base_ID;
        public int Attack_Unit_ID;
        public bool AttackedBool;
    }

    public class MoveStep : ServerMessage
    {
        public override int FieldCount { get { return 3; } }
        public string Position_Cell;
        public string Next_Cell;
        public float TimeMoveNextCell;

        private readonly Vector3Int addBy = new Vector3Int(5, 5, 0);
        public Vector3Int Position
        {
            get { return Position_Cell.Parse3Int() + addBy; }
        }

        public Vector3Int NextPosition
        {
            get { return Next_Cell.Parse3Int() + addBy; }
        }

        public float TimeSecond
        {
            get { return TimeMoveNextCell / 1000.0f; }
        }
    }

    [System.Serializable]
    public class BasePlayerRow : ServerMessage
    {
        public override int FieldCount { get { return 5; } }

        public int ID_User;
        public string NameInGame;
        public string Position;
        public int BaseNumber;
        public int Level;
    }
}