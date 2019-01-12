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
            get { return BaseInfo[CurBaseIndex]; }
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
                BaseInfo[i].RecordElapsedTime(deltaTime, BaseUpgrade); // test --> every BaseInfo corresponding with a BaseUpgrade
            }
        }
    }

}