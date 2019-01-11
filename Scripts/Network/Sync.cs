using ManualTable;
using ManualTable.Row;
using UnityEngine;

namespace Network.Sync
{
    [CreateAssetMenu(fileName = @"Sync", menuName = @"Conn/Sync", order = 1)]
    public sealed class Sync : ScriptableObject
    {
        private int curBaseIndex = 0;

        public BaseInfoRow CurrentMainBase
        {
            get { return BaseInfo[curBaseIndex]; }
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

        public UserInfoJSONTable UserInfo;
        public BaseInfoJSONTable BaseInfo;
        public BaseUpgradeJSONTable BaseUpgrade; //  base 1

        public void SyncUpdate(float deltaTime)
        {
            CurrentMainBase.RecordElapsedTime(deltaTime);
        }
    }

}