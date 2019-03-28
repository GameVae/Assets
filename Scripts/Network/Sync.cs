using System;
using System.Linq;
using ManualTable;
using ManualTable.Row;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public int User_ID = 0;
        public int CurBaseIndex = 0;

        public UserInfoRow MainUser;

        public BaseUpgradeJSONTable CurrentBaseUpgrade
        {
            get { return BaseUpgrade[CurBaseIndex]; }
        }
        public BaseDefendJSONTable CurrentBaseDefend
        {
            get { return BaseDefends[CurBaseIndex]; }
        }
        public BaseInfoRow CurrentMainBase
        {
            get { return (BaseInfoRow)BaseInfos[CurBaseIndex]; }
        }
        public BaseUpgradeRow CurrentUpgrade
        {
            get
            {
                return CurrentBaseUpgrade[CurrentMainBase.UpgradeWait_ID];
            }
        }
        public BaseUpgradeRow CurrentResearch
        {
            get
            {
                return CurrentBaseUpgrade[CurrentMainBase.ResearchWait_ID];
            }
        }

        public RSS_PositionJSONTable RSS_Position;
        public PositionJSONTable Position;

        public UserInfoJSONTable UserInfos;
        public BaseInfoJSONTable BaseInfos;

        public BaseUpgradeJSONTable[] BaseUpgrade;
        public BaseDefendJSONTable[] BaseDefends;

        public UnitJSONTable UnitTable;
        public BasePlayerJSONTable BasePlayerTable;

        public void SyncUpdate()
        {
            for (int i = 0; i < BaseInfos?.Rows?.Count; i++)
            {
                UpdateBaseInfo(i, MainUser);
            }
        }

        private void UpdateBaseInfo(int i, UserInfoRow user)
        {
            BaseInfoRow baseInfo = BaseInfos.Rows[i];
            BaseUpgradeJSONTable baseUpgrade = BaseUpgrade[i];

            baseInfo.Update(this);
            #region old
            //if (baseInfo?.UpgradeTime > 0.0f)
            //{
            //    baseInfo.UpgradeTime -= elapsedTime;
            //    if (baseInfo.UpgradeTime <= 0)
            //    {
            //        baseInfo.UpgradeTime = 0;
            //        baseUpgrade[baseInfo.UpgradeWait_ID].Level++;
            //        user.Might += baseInfo.UpgradeWait_Might;
            //        baseInfo.UpgradeWait_ID = 0;
            //    }
            //}
            //if (baseInfo?.ResearchTime > 0.0f)
            //{
            //    baseInfo.ResearchTime -= elapsedTime;
            //    if (baseInfo.ResearchTime <= 0)
            //    {
            //        baseInfo.ResearchTime = 0;
            //        baseUpgrade[baseInfo.ResearchWait_ID].Level++;
            //        user.Might += baseInfo.ResearchWait_Might;
            //        baseInfo.ResearchWait_ID = 0;
            //    }
            //}
            //if (baseInfo?.TrainingTime > 0.0f)
            //{
            //    // Debug.Log("bef: " + baseInfo?.TrainingTime + " -  e" + elapsedTime);
            //    baseInfo.TrainingTime -= elapsedTime;
            //    if (baseInfo.TrainingTime <= 0)
            //    {
            //        TranningDone(i, user);
            //    }
            //    //Debug.Log("af: " + baseInfo?.TrainingTime);

            //}
            #endregion
        }

        private void TranningDone(int baseIndex, UserInfoRow user)
        {
            BaseInfoRow baseInfo = (BaseInfoRow)BaseInfos[baseIndex];

            BaseDefendJSONTable baseDefend = BaseDefends[baseIndex];
            BaseDefendRow defendRow = baseDefend.Rows.FirstOrDefault(r => r.ID_Unit == baseInfo.TrainingUnit_ID);
            if (defendRow == null)
            {
                baseDefend.Rows.Add(new BaseDefendRow()
                {
                    BaseNumber = baseInfo.BaseNumber,
                    ID_Unit = baseInfo.TrainingUnit_ID,
                    Quality = baseInfo.TrainingQuality,
                });
            }
            else
            {
                defendRow.Quality += baseInfo.TrainingQuality;
            }

            user.Might += baseInfo.Training_Might;
            baseInfo.TrainingTime = 0.0f;
            baseInfo.TrainingUnit_ID = 0;
            baseInfo.TrainingQuality = 0;
            baseInfo.Training_Might = 0;
        }

        public void LoadUserInfo(JSONObject serMsg)
        {
            serMsg[0].GetField(ref User_ID, "ID_User");

            UserInfos.LoadTable(serMsg);
            MainUser = UserInfos.GetUser(User_ID);
        }
    }

}