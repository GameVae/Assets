using EnumCollect;
using Network.Sync;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataTable.Row
{
    [System.Serializable]
    public sealed class TrainingCostRow : ISQLiteData
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

        public int CompareTo(object obj)
        {
            TrainingCostRow other = obj as TrainingCostRow;
            return MightBonus.CompareTo(other.MightBonus);
        }
    }

    #region DB row
    [System.Serializable]
    public class MainBaseRow : ISQLiteData
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

        public int CompareTo(object obj)
        {
            MainBaseRow other = obj as MainBaseRow;
            return Level.CompareTo(other.Level);
        }
    }

    [System.Serializable]
    public class VersionRow : ISQLiteData
    {
        public int Id;
        public string Task;
        public string Content;
        public string Comment;

        public int CompareTo(object obj)
        {
            VersionRow other = obj as VersionRow;
            return Task.CompareTo(other.Task);
        }
    }

    [System.Serializable]
    public class MilitaryRow : ISQLiteData
    {
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

        public int CompareTo(object obj)
        {
            MilitaryRow other = obj as MilitaryRow;
            return Level.CompareTo(other.Level);
        }
    }
    #endregion

    //======================================//

    [System.Serializable]
    public class RSS_PositionRow : ITableData
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

        public int CompareTo(object obj)
        {
            RSS_PositionRow other = obj as RSS_PositionRow;
            return ID.CompareTo(other.ID);
        }
    }

    [System.Serializable]
    public class PositionRow : ITableData
    {
        public int ID;
        public string Position_Transform;
        public string Position_Cell;
        public string ID_Type;
        public string Comment;

        public int CompareTo(object obj)
        {
            PositionRow other = obj as PositionRow;
            return ID.CompareTo(other.ID);
        }
    }

    [System.Serializable]
    public class BaseUpgradeRow : ITableData
    {
        public ListUpgrade ID;
        public string Name_Upgrade;
        public int Level;
        public int UpgradeType;

        public int CompareTo(object obj)
        {
            BaseUpgradeRow other = obj as BaseUpgradeRow;
            return ID.CompareTo(other.ID);
        }
    }

    [System.Serializable]
    public class BaseInfoRow : ITableData
    {
        public int ID_User;
        public int BaseNumber;
        public string Position;

        public int Farm;
        public int Wood;
        public int Stone;
        public int Metal;

        public ListUpgrade UpgradeWait_ID;
        public int UpgradeWait_Might;
        public double UpgradeTime;

        public ListUpgrade ResearchWait_ID;
        public int ResearchWait_Might;
        public double ResearchTime;

        public string UnitTransferType;
        public string UnitTransferQuality;
        public string UnitTransferTime;
        public string UnitTransfer_ID_Base;
        public ListUpgrade TrainingUnit_ID;
        public double TrainingTime;
        public int TrainingQuality;
        public int Training_Might;
        public int SumUnitQuality;

        public bool IsUpgradeDone { get; private set; }
        public bool IsResearchDone { get; private set; }
        public bool IsTrainingDone { get; private set; }

        private AsyncCounter upgCounter;
        private AsyncCounter researchCounter;
        private AsyncCounter trainingCounter;

        public void Initalize()
        {
            researchCounter = new AsyncCounter();
            IsResearchDone = !(ResearchTime > 0);
            if (!IsResearchDone)
                researchCounter.Start(ResearchTime);

            upgCounter = new AsyncCounter();
            IsUpgradeDone = !(UpgradeTime > 0);
            if (!IsUpgradeDone)
                upgCounter.Start(UpgradeTime);

            trainingCounter = new AsyncCounter();
            IsTrainingDone = !(TrainingTime > 0);
            if (!IsTrainingDone)
                trainingCounter.Start(TrainingTime);
        }

        public void Update(Sync sync)
        {
            UpdateResearchTime(sync);

            UpdateUpgradeTime(sync);

            UpdateTrainingTime(sync);
        }

        private void UpdateResearchTime(Sync sync)
        {
            if (!IsResearchDone && researchCounter != null)
            {
                ResearchTime = researchCounter.RefTime;
                if (ResearchTime <= 0)
                {
                    ResearchTime = 0;
                    IsResearchDone = true;

                    ResearchDone(sync);
                }
            }
        }
        private void UpdateUpgradeTime(Sync sync)
        {
            if (!IsUpgradeDone && upgCounter != null)
            {
                UpgradeTime = upgCounter.RefTime;
                if (UpgradeTime <= 0)
                {
                    UpgradeTime = 0;
                    IsUpgradeDone = true;

                    UpgradeDone(sync);
                }
            }
        }
        private void UpdateTrainingTime(Sync sync)
        {
            if (!IsTrainingDone && trainingCounter != null)
            {
                TrainingTime = trainingCounter.RefTime;
                if (TrainingTime <= 0)
                {
                    TrainingTime = 0;
                    IsTrainingDone = true;

                    TrainingDone(sync);
                }
            }
        }

        private void ResearchDone(Sync sync)
        {
            UserInfoRow user = sync.MainUser;
            JSONTable_BaseUpgrade baseUpgrade = sync.CurrentBaseUpgrade;

            baseUpgrade[ResearchWait_ID].Level++;
            user.Might += ResearchWait_Might;
            ResearchWait_ID = 0;
        }
        private void UpgradeDone(Sync sync)
        {
            UserInfoRow user = sync.MainUser;
            JSONTable_BaseUpgrade baseUpgrade = sync.CurrentBaseUpgrade;

            baseUpgrade[UpgradeWait_ID].Level++;
            user.Might += UpgradeWait_Might;

            UpgradeWait_ID = 0;
        }
        private void TrainingDone(Sync sync)
        {
            UserInfoRow user = sync.MainUser;
            JSONTable_BaseDefend baseDefend = sync.CurrentBaseDefend;
            BaseDefendRow defendRow = baseDefend.Rows.FirstOrDefault(r => r.ID_Unit == TrainingUnit_ID);

            if (defendRow == null)
            {
                baseDefend.Rows.Add(new BaseDefendRow()
                {
                    BaseNumber = BaseNumber,
                    ID_Unit = TrainingUnit_ID,
                    Quality = TrainingQuality,
                });
            }
            else
            {
                defendRow.Quality += TrainingQuality;
            }

            user.Might += Training_Might;
            TrainingUnit_ID = 0;
            TrainingQuality = 0;
            Training_Might = 0;
        }

        public void SetUpgradeTime(double time)
        {
            UpgradeTime = time;
            IsUpgradeDone = false;

            upgCounter.Start(UpgradeTime);
        }
        public void SetResearchTime(double time)
        {
            ResearchTime = time;
            IsResearchDone = false;

            researchCounter.Start(ResearchTime);
        }
        public void SetTrainingTime(double time)
        {
            TrainingTime = time;
            IsTrainingDone = false;

            trainingCounter.Start(TrainingTime);
        }

        public bool IsEnoughtResource(int farm, int wood, int stone, int metal)
        {
            return (Farm >= farm && Wood >= wood && Stone >= stone && Metal >= metal);
        }

        public int CompareTo(object obj)
        {
            // TODO: 
            return 0;
        }
    }

    [System.Serializable]
    public class UserInfoRow : ITableData, System.IComparable
    {
        public int ID_User;
        public string Server_ID;
        public string NameInGame;
        public string ChatWorldColor;
        public string Guild_ID;
        public string Guild_Name;
        public string LastGuildID;
        public int Might;
        public int Killed;

        public int CompareTo(object obj)
        {
            UserInfoRow other = obj as UserInfoRow;
            return ID_User.CompareTo(other.ID_User);
        }
    }

    [System.Serializable]
    public class BaseDefendRow : ITableData
    {
        public int ID;
        public int BaseNumber;
        public ListUpgrade ID_Unit;
        public int Quality;

        public int CompareTo(object obj)
        {
            BaseDefendRow other = obj as BaseDefendRow;
            return ID.CompareTo(other.ID);
        }
    }

    [System.Serializable]
    public class UnitRow : ITableData, System.IComparable
    {
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
        public List<MoveStep> ListMove;
        public int Status;
        public int Attack_Base_ID;
        public int Attack_Unit_ID;
        public bool AttackedBool;

        public AgentStatus AgentStatus
        {
            get { return (AgentStatus)Status; }
        }

        public int CompareTo(object obj)
        {
            UnitRow other = obj as UnitRow;
            return ID.CompareTo(other.ID);
        }
    }

    [System.Serializable]
    public class MoveStep
    {
        public string Position_Cell;
        public string Next_Cell;
        public float TimeMoveNextCell;

        #region Properties
        public Vector3Int Position
        {
            get { return Position_Cell.Parse3Int().ToClientPosition(); }
        }

        public Vector3Int NextPosition
        {
            get { return Next_Cell.Parse3Int().ToClientPosition(); }
        }

        public float TimeSecond
        {
            get { return TimeMoveNextCell / 1000.0f; }
        }
        #endregion
    }

    [System.Serializable]
    public class BasePlayerRow : ITableData
    {
        public int ID_User;
        public string NameInGame;
        public string Position;
        public int BaseNumber;
        public int Level;

        public int CompareTo(object obj)
        {
            BasePlayerRow other = obj as BasePlayerRow;
            return ID_User.CompareTo(other.ID_User);
        }
    }
}