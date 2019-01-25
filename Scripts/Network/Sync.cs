using ManualTable;
using ManualTable.Row;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        public int CurBaseIndex = 0;

        public BaseInfoRow CurrentMainBase
        {
            get { return (BaseInfoRow)BaseInfo[CurBaseIndex]; }
        }
        public BaseUpgradeRow CurrentUpgrade
        {
            get
            {
                return BaseUpgrade[CurrentMainBase.UpgradeWait_ID];
            }
        }
        public BaseUpgradeRow CurrentResearch
        {
            get
            {
                return BaseUpgrade[CurrentMainBase.ResearchWait_ID];
            }
        }

        public RSS_PositionJSONTable RSS_Position;
        public PositionJSONTable Position;

        public UserInfoJSONTable UserInfo;
        public BaseInfoJSONTable BaseInfo;
        public BaseUpgradeJSONTable BaseUpgrade; //  base 1

        public void SyncUpdate(float deltaTime)
        {
            for (int i = 0; i < BaseInfo.Rows.Count; i++)
            {
                UpdateBaseInfo(deltaTime, (BaseInfoRow)BaseInfo[i], (UserInfoRow)UserInfo[0]);
            }
        }

        private void UpdateBaseInfo(float elapsedTime, BaseInfoRow baseInfo, UserInfoRow user)
        {
            if (baseInfo.UpgradeTime > 0.0f)
            {
                baseInfo.UpgradeTime -= elapsedTime;
                if (baseInfo.UpgradeTime <= 0)
                {
                    baseInfo.UpgradeTime = 0;
                    BaseUpgrade[baseInfo.UpgradeWait_ID].Level++;
                    user.Might += baseInfo.UpgradeWait_Might;
                    baseInfo.UpgradeWait_ID = 0;
                }
            }
            if (baseInfo.ResearchTime > 0.0f)
            {
                baseInfo.ResearchTime -= elapsedTime;
                if (baseInfo.ResearchTime <= 0)
                {
                    baseInfo.ResearchTime = 0;
                    BaseUpgrade[baseInfo.ResearchWait_ID].Level++;
                    user.Might += baseInfo.ResearchWait_Might;
                    baseInfo.ResearchWait_ID = 0;
                }
            }
            if (baseInfo.TrainingTime > 0.0f)
            {
                baseInfo.TrainingTime -= elapsedTime;
                if (baseInfo.TrainingTime <= 0)
                {
                    baseInfo.TrainingTime = 0.0f;
                }
            }
        }
    }

}